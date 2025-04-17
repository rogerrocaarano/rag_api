using Application.Dto;
using Domain.Model;
using Domain.Service;

namespace Application.UseCase;

public class AskLlmUseCase(ILlmService chat)
{
    public async Task<Message> Execute(List<Message> conversationHistory, GetContextDto context)
    {
        // TODO implementar reglas
        var rules = new Message();
        return await chat.AnswerQuestion(conversationHistory, context.Fragments, rules);
    }
}