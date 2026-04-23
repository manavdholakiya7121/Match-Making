using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface ILikeRepository
    {
        Task<MemberLike?> GetMemberLike(string sourceMemberId, string targetMemberId);

        Task<PaginatedResult<Member>> GetMemberLikes(LikesParams likesParams);

        Task<IReadOnlyList<string>> GetCurrentMemberLikesIds(string memberId);

        void AddLike(MemberLike like);

        void DeleteLike(MemberLike like);
    }
}
