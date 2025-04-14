using Domain.Model;

namespace Domain.Service;

public interface ILlmService
{
    Task<Message> AnswerQuestion(Conversation conversation);
}