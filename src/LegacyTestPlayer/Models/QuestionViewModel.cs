namespace LegacyTestPlayer.Models;

public class QuestionViewModel
{
    public int AttemptId { get; set; }
    public int QuestionId { get; set; }
    public int Number { get; set; }
    public int Total { get; set; }
    public string Skill { get; set; } = "";
    public int Difficulty { get; set; }
    public string Stem { get; set; } = "";
    public IReadOnlyList<ChoiceViewModel> Choices { get; set; } = Array.Empty<ChoiceViewModel>();
    public string? Error { get; set; }
}

public class ChoiceViewModel
{
    public string Letter { get; set; } = "";
    public string Text { get; set; } = "";
}
