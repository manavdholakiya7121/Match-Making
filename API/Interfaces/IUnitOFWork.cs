namespace API.Interfaces
{
    public interface IUnitOFWork
    {
        IMemberRepository MemberRepository { get; }

        IMessageRepository MessageRepository { get; }

        ILikeRepository LikesRepository { get; }

        Task<bool> Complete();

        bool HasChanges();
    }
}
