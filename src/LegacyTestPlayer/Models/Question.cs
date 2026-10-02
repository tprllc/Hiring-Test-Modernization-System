namespace LegacyTestPlayer.Models;

public class Question
{
    public int Id { get; set; }
    public int SortOrder { get; set; }
    public string Skill { get; set; } = "";
    public int Difficulty { get; set; }
    public string Stem { get; set; } = "";
    public string ChoiceA { get; set; } = "";
    public string ChoiceB { get; set; } = "";
    public string ChoiceC { get; set; } = "";
    public string ChoiceD { get; set; } = "";
}
