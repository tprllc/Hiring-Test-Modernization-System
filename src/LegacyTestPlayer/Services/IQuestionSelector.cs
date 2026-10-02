using LegacyTestPlayer.Models;

namespace LegacyTestPlayer.Services;

public interface IQuestionSelector
{
    Question? Next(IReadOnlyList<Question> catalog, IReadOnlyList<Response> responses);
}
