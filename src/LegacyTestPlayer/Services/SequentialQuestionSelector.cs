using LegacyTestPlayer.Models;

namespace LegacyTestPlayer.Services;

public class SequentialQuestionSelector : IQuestionSelector
{
    public Question? Next(IReadOnlyList<Question> catalog, IReadOnlyList<Response> responses)
    {
        var answered = responses.Select(r => r.QuestionId).ToHashSet();
        return catalog
            .Where(q => !answered.Contains(q.Id))
            .OrderBy(q => q.SortOrder)
            .FirstOrDefault();
    }
}
