using API.Entities;
using API.Helpers;
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

        public async Task<IReadOnlyList<string>> GetCurrentMemberLikesIds(string memberId)
        {
            return await appDbContext.Likes
            .Where(x => x.SourceMemberId == memberId)
            .Select(x => x.TargetMemberId)
            .ToListAsync();
        }

        public async Task<MemberLike?> GetMemberLike(string sourceMemberId, string targetMemberId)
        {
            return await appDbContext.Likes.FindAsync(sourceMemberId, targetMemberId);
        }

        public async Task<PaginatedResult<Member>> GetMemberLikes(LikesParams likesParams)
        {
            var query = appDbContext.Likes.AsQueryable();
            IQueryable<Member> result;

            switch (likesParams.Predicate)
            {
                case "liked":
                    result = query
                        .Where(like => like.SourceMemberId == likesParams.MemberId)
                        .Select(like => like.TargetMember);
                    break;
                case "likedBy":
                    result = query
                        .Where(like => like.TargetMemberId == likesParams.MemberId)
                        .Select(like => like.SourceMember);
                    break;
                default: // mutual
                    var likeIds = await GetCurrentMemberLikesIds(likesParams.MemberId);

                    result = query
                        .Where(x => x.TargetMemberId == likesParams.MemberId
                            && likeIds.Contains(x.SourceMemberId))
                        .Select(x => x.SourceMember);
                    break;
            }

            return await PaginationHelper.CreateAsync(result,
                likesParams.PageNumber, likesParams.PageSize);
        }

        public async Task<bool> SaveAllChanges()
        {
            return await appDbContext.SaveChangesAsync() > 0;
        }
    }
}
