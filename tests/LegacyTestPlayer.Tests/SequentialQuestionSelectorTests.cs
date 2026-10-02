using LegacyTestPlayer.Models;
using LegacyTestPlayer.Services;
using Xunit;

namespace LegacyTestPlayer.Tests;

public class SequentialQuestionSelectorTests
{
    private readonly SequentialQuestionSelector _selector = new();

    [Fact]
    public void Next_returns_the_first_unanswered_question_in_sort_order()
    {
        var catalog = new List<Question>
        {
            new() { Id = 2, SortOrder = 2, Stem = "Second" },
            new() { Id = 1, SortOrder = 1, Stem = "First" }
        };

        var next = _selector.Next(catalog, Array.Empty<Response>());

        Assert.Equal(1, next!.Id);
    }

    [Fact]
    public void Next_skips_questions_that_already_have_a_response()
    {
        var catalog = new List<Question>
        {
            new() { Id = 1, SortOrder = 1 },
            new() { Id = 2, SortOrder = 2 }
        };
        var responses = new List<Response> { new() { QuestionId = 1, SelectedChoice = "A" } };

        var next = _selector.Next(catalog, responses);

        Assert.Equal(2, next!.Id);
    }

    [Fact]
    public void Next_returns_null_when_every_question_has_been_answered()
    {
        var catalog = new List<Question> { new() { Id = 1, SortOrder = 1 } };
        var responses = new List<Response> { new() { QuestionId = 1 } };

        Assert.Null(_selector.Next(catalog, responses));
    }
}
