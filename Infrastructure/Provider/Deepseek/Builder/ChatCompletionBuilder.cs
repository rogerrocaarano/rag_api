using Domain.Constant;
using Domain.Model;
using Infrastructure.Provider.Deepseek.Constant;
using Infrastructure.Provider.Deepseek.Contract;
using Infrastructure.Provider.Deepseek.Model;

namespace Infrastructure.Provider.Deepseek.Builder;

public static class ChatCompletionBuilder
{
    public static ChatCompletionRequest BuildRequest(List<Message> conversation, List<Fragment> context, Message rules)
    {
        var messages = new List<DsMessage>();
        AddRules(messages, rules);
        AddContext(messages, context);
        AddConversation(messages, conversation);
        return new ChatCompletionRequest
        {
            Model = ChatModel.Chat,
            Messages = messages,
            Stream = false
        };
    }

    private static void AddRules(List<DsMessage> messages, Message rules)
    {
        messages.Add(new DsMessage
        {
            Content = rules.Content,
            Role = RoleMapping(rules.Role)
        });
    }

    private static void AddContext(List<DsMessage> messages, List<Fragment> context)
    {
        messages.AddRange(context.Select(fragment => new DsMessage
        {
            Content = fragment.Content,
            Role = MessageRole.System
        }));
    }

    private static void AddConversation(List<DsMessage> messages, List<Message> conversation)
    {
        messages.AddRange(conversation.Select(message => new DsMessage
        {
            Content = message.Content,
            Role = RoleMapping(message.Role)
        }));
    }

    private static string RoleMapping(string domainMessageType)
    {
        return domainMessageType switch
        {
            MessageType.Assistant => MessageRole.Assistant,
            MessageType.User => MessageRole.User,
            MessageType.Context => MessageRole.System,
            MessageType.Rule => MessageRole.System,
            _ => throw new ArgumentException()
        };
    }
}