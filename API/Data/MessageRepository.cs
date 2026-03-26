using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;

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

        public Task<PaginatedResult<MessageDto>> GetMessagesForMember()
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<MessageDto>> GetMessageThread(string currentMemberId, string recipientId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await appDbContext.SaveChangesAsync() > 0;
        }
    }
}
