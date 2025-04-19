using Domain.Model;

namespace Domain.Service;

public interface ILlmService
{
    Task<Message> AnswerQuestion(List<Message> conversation, List<Fragment> context, Message rules);
}