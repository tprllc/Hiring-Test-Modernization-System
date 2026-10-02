namespace LegacyTestPlayer.Models;

public class Response
{
    public int Id { get; set; }
    public int AttemptId { get; set; }
    public int QuestionId { get; set; }
    public string SelectedChoice { get; set; } = "";
    public bool IsCorrect { get; set; }
    public Attempt? Attempt { get; set; }
    public Question? Question { get; set; }
}
