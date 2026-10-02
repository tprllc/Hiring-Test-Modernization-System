namespace LegacyTestPlayer.Models;

public class AnswerKey
{
    public int QuestionId { get; set; }
    public string CorrectChoice { get; set; } = "";
    public Question? Question { get; set; }
}
