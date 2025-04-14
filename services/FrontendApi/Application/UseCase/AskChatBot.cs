using Application.Dto;
using Domain.Service;

namespace Application.UseCase;

public class AskChatBot
{
    private ILlmService chat { get; set; }

    public AskChatBotDto Execute(SearchBySimilarityDto dto)
    {
        // TODO implement here
        return null;
    }
}