namespace LegacyTestPlayer.Services;

public class GradingService
{
    public bool IsCorrect(string? selectedChoice, string correctChoice)
    {
        if (string.IsNullOrWhiteSpace(selectedChoice))
            return false;

        return string.Equals(selectedChoice.Trim(), correctChoice.Trim(), StringComparison.OrdinalIgnoreCase);
    }
}
