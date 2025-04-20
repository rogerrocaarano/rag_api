using Application.Dto;
using Domain.Constant;
using Domain.Model;
using Domain.Service;

namespace Application.UseCase;

public class AskLlmUseCase(ILlmService chat, List<string> ruleSet)
{
    public async Task<Message> Execute(List<Message> conversationHistory, GetContextDto context)
    {
        var rules = new Message
        {
            Role = MessageType.Rule,
            ConversationId = conversationHistory[0].ConversationId,
            CreatedAt = DateTime.Now,
            Id = Guid.NewGuid(),
            Content = string.Join(Environment.NewLine, ruleSet)
        };
        return await chat.AnswerQuestion(conversationHistory, context.Fragments, rules);
    }
}