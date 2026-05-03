using Microsoft.EntityFrameworkCore;
using VoxLink.API.Domain.Entities;

namespace VoxLink.API.Application.Common;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<ChatRoom> ChatRooms { get; }
    DbSet<Message> Messages { get; }
    DbSet<ChatRoomParticipant> ChatRoomParticipants { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
