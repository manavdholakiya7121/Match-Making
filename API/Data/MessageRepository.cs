using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class MessageRepository(AppDbContext appDbContext) : IMessageRepository
    {
        public void AddMessage(Message message)
        {
            appDbContext.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            appDbContext.Messages.Remove(message);
        }

        public async Task<Message?> GetMessage(string messageId)
        {
           return await appDbContext.Messages.FindAsync(messageId);
        }

        public async Task<PaginatedResult<MessageDto>> GetMessagesForMember(MessageParams messageParams)
        {
            var query = appDbContext.Messages
                .OrderByDescending(m => m.MessageSent)
                .AsQueryable();

            query = messageParams.Container switch
            {
                "Outbox" => query.Where(m => m.SenderId == messageParams.MemberId),
                _ => query.Where(m => m.RecipientId == messageParams.MemberId)
            };

            var messageQuery = query.Select(MessageExtensions.ToDtoProjection());

            return await PaginationHelper.CreateAsync(messageQuery, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<IReadOnlyList<MessageDto>> GetMessageThread(string currentMemberId, string recipientId)
        {
            await appDbContext.Messages
                .Where(x => x.RecipientId == currentMemberId && x.SenderId == recipientId && x.DateRead == null)
                .ExecuteUpdateAsync(m => m.SetProperty(p => p.DateRead, DateTime.UtcNow));

            return await appDbContext.Messages
                .Where(x => (x.RecipientId == currentMemberId &&  x.SenderId == recipientId) || (x.SenderId == currentMemberId && x.RecipientId == recipientId))
                .OrderBy(m => m.MessageSent)
                .Select(MessageExtensions.ToDtoProjection())
                .ToListAsync();

        }

        public async Task<bool> SaveAllAsync()
        {
            return await appDbContext.SaveChangesAsync() > 0;
        }
    }
}
