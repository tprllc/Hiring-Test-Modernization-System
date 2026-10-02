using LegacyTestPlayer.Models;

namespace LegacyTestPlayer.Data;

public static class CatalogSeeder
{
    public static void Seed(TestDbContext db)
    {
        if (db.Questions.Any())
            return;

        var items = new (Question Question, string Correct)[]
        {
            Item(1, "Equations", 1, "What is 6 + 7?", "11", "12", "13", "14", "C"),
            Item(2, "Equations", 2, "Solve for x: 2x + 3 = 11", "3", "4", "5", "7", "B"),
            Item(3, "Equations", 3, "A pen and a notebook cost $10 together. The notebook costs $4 more than the pen. How much is the pen?", "$2", "$3", "$4", "$6", "B"),
            Item(4, "Reading", 1, "In the sentence \"The lamp was dim,\" which word describes the lamp?", "The", "lamp", "was", "dim", "D"),
            Item(5, "Reading", 2, "A passage says the trail is closed after sunset. Which statement matches the passage?", "The trail is always closed.", "The trail is closed at night.", "The trail is closed at noon.", "The trail is new.", "B"),
            Item(6, "Reading", 3, "The author writes, \"The experiment failed, and that failure showed us what to try next.\" What is the author's point?", "Failure ends the work.", "The experiment should be hidden.", "A failed try can still teach something.", "The next try should be identical.", "C"),
            Item(7, "Grammar", 1, "Choose the sentence that is written correctly.", "She don't like the book.", "She doesn't like the book.", "She not like the book.", "She doesn't likes the book.", "B"),
            Item(8, "Grammar", 2, "Which word best completes the sentence? \"The students finished ___ essays.\"", "there", "their", "they're", "they", "B"),
            Item(9, "Equations", 2, "What is 15% of 80?", "8", "12", "15", "20", "B"),
            Item(10, "Reading", 1, "Which of these is a question?", "Close the door.", "The door is closed.", "Is the door closed?", "Closing the door.", "C"),
        };

        foreach (var item in items)
        {
            db.Questions.Add(item.Question);
            db.AnswerKeys.Add(new AnswerKey
            {
                QuestionId = item.Question.Id,
                CorrectChoice = item.Correct
            });
        }

        db.SaveChanges();
    }

    private static (Question Question, string Correct) Item(
        int sortOrder,
        string skill,
        int difficulty,
        string stem,
        string a,
        string b,
        string c,
        string d,
        string correct)
    {
        return (new Question
        {
            Id = sortOrder,
            SortOrder = sortOrder,
            Skill = skill,
            Difficulty = difficulty,
            Stem = stem,
            ChoiceA = a,
            ChoiceB = b,
            ChoiceC = c,
            ChoiceD = d
        }, correct);
    }
}
