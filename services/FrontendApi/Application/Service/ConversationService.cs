using Application.Dto;
using Domain.Model;

namespace Application.Service;

public class ConversationService {


    /// <summary>
    /// @param message
    /// </summary>
    public void AskChatBot(string message) {
        // TODO implement here
    }

    /// <summary>
    /// @param conversationId 
    /// @param message
    /// </summary>
    public void AskChatBot(Guid conversationId, string message) {
        // TODO implement here
    }

    /// <summary>
    /// @param conversationId 
    /// @param name 
    /// @return
    /// </summary>
    public AskChatBotDto UpdateConversationName(Guid conversationId, String name) {
        // TODO implement here
        return null;
    }

    /// <summary>
    /// @param conversationId 
    /// @return
    /// </summary>
    public Conversation UpdateConversationName(Guid conversationId) {
        // TODO implement here
        return null;
    }

}