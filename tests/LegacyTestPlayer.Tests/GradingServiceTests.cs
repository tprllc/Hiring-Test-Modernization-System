using LegacyTestPlayer.Services;
using Xunit;

namespace LegacyTestPlayer.Tests;

public class GradingServiceTests
{
    private readonly GradingService _grading = new();

    [Theory]
    [InlineData("B", "B", true)]
    [InlineData("b", "B", true)]
    [InlineData("A", "B", false)]
    [InlineData("", "B", false)]
    [InlineData(null, "B", false)]
    public void IsCorrect_compares_the_selected_choice_to_the_key(string? selected, string correct, bool expected)
    {
        Assert.Equal(expected, _grading.IsCorrect(selected, correct));
    }
}
