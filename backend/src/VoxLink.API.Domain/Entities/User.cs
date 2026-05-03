namespace VoxLink.API.Domain.Entities;

public class User : BaseEntity
{
    public string Email { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string? AvatarUrl { get; private set; }
    public bool IsOnline { get; private set; }
    public DateTime? LastSeen { get; private set; }
    public string? PasswordHash { get; private set; }

    private User(Guid id, string email, string name) : base(id)
    {
        Email = Guard.Against.NullOrEmpty(email);
        Name = Guard.Against.NullOrEmpty(name);
        IsOnline = false;
    }

    public static User Create(string email, string name)
    {
        return new User(Guid.NewGuid(), email, name);
    }

    public void UpdateProfile(string name, string? avatarUrl = null)
    {
        Name = Guard.Against.NullOrEmpty(name);
        AvatarUrl = avatarUrl;
        UpdateTimestamp();
    }

    public void SetPassword(string passwordHash)
    {
        PasswordHash = Guard.Against.NullOrEmpty(passwordHash);
        UpdateTimestamp();
    }

    public void SetOnlineStatus(bool isOnline)
    {
        IsOnline = isOnline;
        if (!isOnline)
        {
            LastSeen = DateTime.UtcNow;
        }
        UpdateTimestamp();
    }
}
