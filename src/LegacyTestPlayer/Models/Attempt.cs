namespace LegacyTestPlayer.Models;

public class Attempt
{
    public int Id { get; set; }
    public string StudentName { get; set; } = "";
    public DateTime StartedAtUtc { get; set; }
    public List<Response> Responses { get; set; } = new();
}
