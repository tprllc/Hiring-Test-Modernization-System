namespace LegacyTestPlayer.Models;

public class ResultsViewModel
{
    public string StudentName { get; set; } = "";
    public int Correct { get; set; }
    public int Total { get; set; }
    public IReadOnlyList<SkillResultViewModel> Skills { get; set; } = Array.Empty<SkillResultViewModel>();
}

public class SkillResultViewModel
{
    public string Skill { get; set; } = "";
    public int Correct { get; set; }
    public int Total { get; set; }
}
