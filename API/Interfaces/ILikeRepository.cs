using API.Entities;

namespace API.Interfaces
{
    public interface ILikeRepository
    {
        Task<MemberLike?> GetMemberLike(string sourceMemberId, string targetMemberId);

        Task<IReadOnlyList<Member>> GetMemberLikes(string predicate, string memberId);

        Task<IReadOnlyList<string>> GetCurrentMemberLikesIds(string memberId);

        void AddLike(MemberLike like);

        void DeleteLike(MemberLike like);

        Task<bool> SaveAllChanges();
    }
}
