namespace VoxLink.API.Domain.Entities;

public class ChatRoom : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public bool IsGroup { get; private set; }
    public ICollection<ChatRoomParticipant> Participants { get; private set; } = new List<ChatRoomParticipant>();
    public ICollection<Message> Messages { get; private set; } = new List<Message>();

    private ChatRoom(Guid id, string name, bool isGroup) : base(id)
    {
        Name = Guard.Against.NullOrEmpty(name);
        IsGroup = isGroup;
    }

    public static ChatRoom CreateGroupChat(string name, string? description = null)
    {
        var chatRoom = new ChatRoom(Guid.NewGuid(), name, true);
        chatRoom.Description = description;
        return chatRoom;
    }

    public static ChatRoom CreateDirectChat(string name)
    {
        return new ChatRoom(Guid.NewGuid(), name, false);
    }

    public void AddParticipant(User user, ChatRoomRole role = ChatRoomRole.Member)
    {
        if (Participants.Any(p => p.UserId == user.Id))
            return;

        var participant = ChatRoomParticipant.Create(user.Id, Id, role);
        Participants.Add(participant);
        UpdateTimestamp();
    }

    public void RemoveParticipant(Guid userId)
    {
        var participant = Participants.FirstOrDefault(p => p.UserId == userId);
        if (participant != null)
        {
            Participants.Remove(participant);
            UpdateTimestamp();
        }
    }

    public void UpdateDetails(string name, string? description = null)
    {
        Name = Guard.Against.NullOrEmpty(name);
        Description = description;
        UpdateTimestamp();
    }
}

public class ChatRoomParticipant : BaseEntity
{
    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;
    public Guid ChatRoomId { get; private set; }
    public ChatRoom ChatRoom { get; private set; } = null!;
    public ChatRoomRole Role { get; private set; }
    public DateTime JoinedAt { get; private set; }

    private ChatRoomParticipant(Guid id, Guid userId, Guid chatRoomId, ChatRoomRole role) : base(id)
    {
        UserId = userId;
        ChatRoomId = chatRoomId;
        Role = role;
        JoinedAt = DateTime.UtcNow;
    }

    public static ChatRoomParticipant Create(Guid userId, Guid chatRoomId, ChatRoomRole role)
    {
        return new ChatRoomParticipant(Guid.NewGuid(), userId, chatRoomId, role);
    }

    public void UpdateRole(ChatRoomRole newRole)
    {
        Role = newRole;
        UpdateTimestamp();
    }
}

public enum ChatRoomRole
{
    Admin,
    Moderator,
    Member
}
