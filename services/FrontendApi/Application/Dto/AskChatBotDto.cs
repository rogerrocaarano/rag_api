using Domain.Model;

namespace Application.Dto;

public class AskChatBotDto
{
    public Conversation Conversation { get; set; }

    public Message Response { get; set; }
}