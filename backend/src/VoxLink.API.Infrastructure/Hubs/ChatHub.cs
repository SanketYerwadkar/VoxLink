using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace VoxLink.API.Infrastructure.Hubs;

public class ChatHub : Hub
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<ChatHub> _logger;

    public ChatHub(IApplicationDbContext context, ILogger<ChatHub> logger)
    {
        _context = context;
        _logger = logger;
    }

    public override async Task OnConnectedAsync()
    {
        var userId = Context.GetUserId();
        if (userId.HasValue)
        {
            var user = await _context.Users.FindAsync(userId.Value);
            if (user != null)
            {
                user.SetOnlineStatus(true);
                await _context.SaveChangesAsync();
                await Clients.Others.SendAsync("UserConnected", new { userId = user.Id, userName = user.Name });
            }
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.GetUserId();
        if (userId.HasValue)
        {
            var user = await _context.Users.FindAsync(userId.Value);
            if (user != null)
            {
                user.SetOnlineStatus(false);
                await _context.SaveChangesAsync();
                await Clients.Others.SendAsync("UserDisconnected", new { userId = user.Id, userName = user.Name });
            }
        }

        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(string content, string type, Guid? chatRoomId, Guid? recipientId)
    {
        var userId = Context.GetUserId();
        if (!userId.HasValue)
        {
            await Clients.Caller.SendAsync("Error", "User not authenticated");
            return;
        }

        var user = await _context.Users.FindAsync(userId.Value);
        if (user == null)
        {
            await Clients.Caller.SendAsync("Error", "User not found");
            return;
        }

        var messageType = Enum.Parse<MessageType>(type);
        var message = Domain.Entities.Message.CreateTextMessage(content, userId.Value, chatRoomId, recipientId);

        _context.Messages.Add(message);
        await _context.SaveChangesAsync();

        var messageDto = new
        {
            id = message.Id,
            content = message.Content,
            type = message.Type.ToString(),
            userId = message.UserId,
            userName = user.Name,
            timestamp = message.CreatedAt,
            chatRoomId = message.ChatRoomId,
            recipientId = message.RecipientId
        };

        if (chatRoomId.HasValue)
        {
            await Clients.Group($"chatroom_{chatRoomId}").SendAsync("ReceiveMessage", messageDto);
        }
        else if (recipientId.HasValue)
        {
            await Clients.User(recipientId.Value.ToString()).SendAsync("ReceiveMessage", messageDto);
            await Clients.Caller.SendAsync("ReceiveMessage", messageDto);
        }
    }

    public async Task JoinChatRoom(Guid chatRoomId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"chatroom_{chatRoomId}");
    }

    public async Task LeaveChatRoom(Guid chatRoomId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"chatroom_{chatRoomId}");
    }

    public async Task Typing(Guid? chatRoomId, Guid? recipientId)
    {
        var userId = Context.GetUserId();
        if (!userId.HasValue) return;

        var user = await _context.Users.FindAsync(userId.Value);
        if (user == null) return;

        var typingDto = new { userId = user.Id, userName = user.Name, chatRoomId, recipientId };

        if (chatRoomId.HasValue)
        {
            await Clients.OthersInGroup($"chatroom_{chatRoomId}").SendAsync("UserTyping", typingDto);
        }
        else if (recipientId.HasValue)
        {
            await Clients.User(recipientId.Value.ToString()).SendAsync("UserTyping", typingDto);
        }
    }

    public async Task StopTyping(Guid? chatRoomId, Guid? recipientId)
    {
        var userId = Context.GetUserId();
        if (!userId.HasValue) return;

        var user = await _context.Users.FindAsync(userId.Value);
        if (user == null) return;

        var typingDto = new { userId = user.Id, userName = user.Name, chatRoomId, recipientId };

        if (chatRoomId.HasValue)
        {
            await Clients.OthersInGroup($"chatroom_{chatRoomId}").SendAsync("UserStopTyping", typingDto);
        }
        else if (recipientId.HasValue)
        {
            await Clients.User(recipientId.Value.ToString()).SendAsync("UserStopTyping", typingDto);
        }
    }
}

public static class HubExtensions
{
    public static Guid? GetUserId(this HubCallerContext context)
    {
        var userIdClaim = context.User?.FindFirst("UserId")?.Value;
        return Guid.TryParse(userIdClaim, out var guid) ? guid : null;
    }
}
