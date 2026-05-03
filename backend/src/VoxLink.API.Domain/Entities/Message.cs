using VoxLink.API.Domain.Entities.Common;

namespace VoxLink.API.Domain.Entities;

public class Message : BaseEntity
{
    public string Content { get; private set; } = string.Empty;
    public MessageType Type { get; private set; }
    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;
    public Guid? ChatRoomId { get; private set; }
    public ChatRoom? ChatRoom { get; private set; }
    public Guid? RecipientId { get; private set; }
    public User? Recipient { get; private set; }

    private Message(Guid id, string content, MessageType type, Guid userId) : base(id)
    {
        Content = Guard.Against.NullOrEmpty(content);
        Type = type;
        UserId = userId;
    }

    public static Message CreateTextMessage(string content, Guid userId, Guid? chatRoomId = null, Guid? recipientId = null)
    {
        var message = new Message(Guid.NewGuid(), content, MessageType.Text, userId);
        message.ChatRoomId = chatRoomId;
        message.RecipientId = recipientId;
        return message;
    }

    public static Message CreateFileMessage(string content, Guid userId, Guid? chatRoomId = null, Guid? recipientId = null)
    {
        var message = new Message(Guid.NewGuid(), content, MessageType.File, userId);
        message.ChatRoomId = chatRoomId;
        message.RecipientId = recipientId;
        return message;
    }

    public static Message CreateImageMessage(string content, Guid userId, Guid? chatRoomId = null, Guid? recipientId = null)
    {
        var message = new Message(Guid.NewGuid(), content, MessageType.Image, userId);
        message.ChatRoomId = chatRoomId;
        message.RecipientId = recipientId;
        return message;
    }
}

public enum MessageType
{
    Text,
    Image,
    File
}
