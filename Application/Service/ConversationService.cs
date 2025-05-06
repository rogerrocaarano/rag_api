using Application.Dto;
using Application.UseCase;
using Domain.Constant;
using Domain.Model;
using Domain.Repository;

namespace Application.Service;

public class ConversationService(
    IMessagesRepository messagesRepository,
    IConversationsRepository conversationsRepository,
    IFirebaseUsersRepository firebaseUsersRepository,
    AskLlmUseCase askLlm,
    GetContextUseCase getContext)
{
    public async Task<Message> AgentQuery(string message, Guid userId)
    {
        var conversationId = conversationsRepository.AddConversation("Consulta del código de tránsito", userId).Result.Id;
        return await AgentQuery(message, conversationId, userId);
    }

    public async Task<Message> AgentQuery(string message, Guid conversationId, Guid userId)
    {
        await messagesRepository.AddMessage(conversationId, message, MessageType.User);
        var conversationHistory = conversationsRepository.GetConversation(conversationId).Result.Messages.ToList();
        var context = await getContext.Execute(message);
        var llmResponse = await askLlm.Execute(conversationHistory, context);
        var answer = await messagesRepository.AddMessage(conversationId, llmResponse.Content, MessageType.Assistant);
        return answer;
    }

    public async Task<List<Conversation>> GetConversationsByFirebaseUserId(string firebaseUserId)
    {
        var userId = await firebaseUsersRepository.GetUserByFirebaseId(firebaseUserId);
        if (userId == null)
            throw new Exception("User not found");

        var conversations = await conversationsRepository.GetConversationsByUserId(userId.Value);
        return conversations;
    }

    public async Task<List<Message>> GetConversationMessages(string firebaseUserId, Guid conversationId)
    {
        var userId = await firebaseUsersRepository.GetUserByFirebaseId(firebaseUserId);
        if (userId == null)
            throw new Exception("User not found");

        var conversation = await conversationsRepository.GetConversation(conversationId);
        if (conversation == null || conversation.OwnerId != userId)
            throw new Exception("Conversation not found");

        return conversation.Messages.ToList();
    }

    public async Task<Guid> StartConversation(string conversationHeader, string firebaseUserId)
    {
        var userId = await firebaseUsersRepository.GetUserByFirebaseId(firebaseUserId);
        if (userId == null)
            throw new Exception("User not found");

        var conversation = await conversationsRepository.AddConversation(conversationHeader, userId);

        return conversation.Id;
    }

    public async Task<Guid> AddMessageToConversation(string message, Guid conversationId, string firebaseUserId)
    {
        var userId = await firebaseUsersRepository.GetUserByFirebaseId(firebaseUserId);
        if (userId == null)
            throw new Exception("User not found");

        var conversation = await conversationsRepository.GetConversation(conversationId);
        if (conversation == null || conversation.OwnerId != userId)
            throw new Exception("Conversation not found");

        var messageAdded = await messagesRepository.AddMessage(
            conversationId,
            message,
            MessageType.User
        );

        return messageAdded.Id;
    }
}