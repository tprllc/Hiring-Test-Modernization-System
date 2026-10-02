using LegacyTestPlayer.Data;
using LegacyTestPlayer.Models;
using LegacyTestPlayer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LegacyTestPlayer.Controllers;

public class PracticeController : Controller
{
    private readonly TestDbContext _db;
    private readonly GradingService _grading;
    private readonly IQuestionSelector _selector;

    public PracticeController(TestDbContext db, GradingService grading, IQuestionSelector selector)
    {
        _db = db;
        _grading = grading;
        _selector = selector;
    }

    [HttpPost]
    public IActionResult Start(string studentName)
    {
        var name = (studentName ?? "").Trim();
        if (name.Length == 0 || name.Length > 80)
        {
            TempData["Error"] = "Enter a name, up to 80 characters.";
            return RedirectToAction("Index", "Home");
        }

        var attempt = new Attempt
        {
            StudentName = name,
            StartedAtUtc = DateTime.UtcNow
        };
        _db.Attempts.Add(attempt);
        _db.SaveChanges();
        return RedirectToAction(nameof(Question), new { attemptId = attempt.Id });
    }

    [HttpGet]
    public IActionResult Question(int attemptId)
    {
        var attempt = _db.Attempts
            .Include(a => a.Responses)
            .SingleOrDefault(a => a.Id == attemptId);
        if (attempt == null)
            return NotFound();

        var catalog = _db.Questions.AsNoTracking().OrderBy(q => q.SortOrder).ToList();
        var next = _selector.Next(catalog, attempt.Responses);
        if (next == null)
            return RedirectToAction(nameof(Results), new { attemptId });

        return View(ToViewModel(attempt, next, catalog.Count, null));
    }

    [HttpPost]
    public IActionResult Answer(int attemptId, int questionId, string? choice)
    {
        var attempt = _db.Attempts
            .Include(a => a.Responses)
            .SingleOrDefault(a => a.Id == attemptId);
        if (attempt == null)
            return NotFound();

        var catalog = _db.Questions.AsNoTracking().OrderBy(q => q.SortOrder).ToList();
        var expected = _selector.Next(catalog, attempt.Responses);
        if (expected == null)
            return RedirectToAction(nameof(Results), new { attemptId });

        if (expected.Id != questionId)
            return RedirectToAction(nameof(Question), new { attemptId });

        var key = _db.AnswerKeys.AsNoTracking().Single(k => k.QuestionId == questionId);
        var selected = (choice ?? "").Trim().ToUpperInvariant();
        if (selected is not ("A" or "B" or "C" or "D"))
        {
            return View(nameof(Question), ToViewModel(attempt, expected, catalog.Count, "Choose A, B, C, or D."));
        }

        _db.Responses.Add(new Response
        {
            AttemptId = attemptId,
            QuestionId = questionId,
            SelectedChoice = selected,
            IsCorrect = _grading.IsCorrect(selected, key.CorrectChoice)
        });
        _db.SaveChanges();
        return RedirectToAction(nameof(Question), new { attemptId });
    }

    [HttpGet]
    public IActionResult Results(int attemptId)
    {
        var attempt = _db.Attempts
            .AsNoTracking()
            .Include(a => a.Responses)
            .ThenInclude(r => r.Question)
            .SingleOrDefault(a => a.Id == attemptId);
        if (attempt == null)
            return NotFound();

        var skills = attempt.Responses
            .GroupBy(r => r.Question!.Skill)
            .OrderBy(g => g.Key)
            .Select(g => new SkillResultViewModel
            {
                Skill = g.Key,
                Correct = g.Count(r => r.IsCorrect),
                Total = g.Count()
            })
            .ToList();

        return View(new ResultsViewModel
        {
            StudentName = attempt.StudentName,
            Correct = attempt.Responses.Count(r => r.IsCorrect),
            Total = attempt.Responses.Count,
            Skills = skills
        });
    }

    private static QuestionViewModel ToViewModel(Attempt attempt, Question question, int total, string? error)
    {
        return new QuestionViewModel
        {
            AttemptId = attempt.Id,
            QuestionId = question.Id,
            Number = attempt.Responses.Count + 1,
            Total = total,
            Skill = question.Skill,
            Difficulty = question.Difficulty,
            Stem = question.Stem,
            Error = error,
            Choices = new[]
            {
                new ChoiceViewModel { Letter = "A", Text = question.ChoiceA },
                new ChoiceViewModel { Letter = "B", Text = question.ChoiceB },
                new ChoiceViewModel { Letter = "C", Text = question.ChoiceC },
                new ChoiceViewModel { Letter = "D", Text = question.ChoiceD }
            }
        };
    }
}
