using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class LikesRepository(AppDbContext appDbContext) : ILikeRepository
    {
        public void AddLike(MemberLike like)
        {
            appDbContext.Likes.Add(like);
        }

        public void DeleteLike(MemberLike like)
        {
            appDbContext.Likes.Remove(like);
        }

        public async Task<IReadOnlyList<string>> GetCurrentMemberLikes(string memberId)
        {
            return await appDbContext.Likes
                .Where(l => l.SourceMemberId == memberId)
                .Select(l => l.TargetMemberId)
                .ToListAsync();
        }

        public async Task<MemberLike?> GetMemberLike(string sourceMemberId, string targetMemberId)
        {
            return await appDbContext.Likes.FindAsync(sourceMemberId, targetMemberId);
        }

        public async Task<IReadOnlyList<Member>> GetMemberLikes(string predicate, string memberId)
        {
            var query = appDbContext.Likes.AsQueryable();

            switch(predicate)
            {
                case "liked":
                     return await query.Where(l => l.SourceMemberId == memberId).Select(x => x.TargetMember).ToListAsync();
                case "likedBy":
                    return await query.Where(l => l.TargetMemberId == memberId).Select(x => x.SourceMember).ToListAsync();
                default:
                    var likedIds = await GetCurrentMemberLikes(memberId);
                    return await query.Where(x => x.TargetMemberId == memberId && likedIds.Contains(x.SourceMemberId))
                        .Select(l => l.SourceMember)
                        .ToListAsync();
            }

        }

        public async Task<bool> SaveAllChanges()
        {
            return await appDbContext.SaveChangesAsync() > 0;
        }
    }
}
