using API.Entities;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class LikesController(ILikeRepository likeReository) : BaseApiController
    {
        [HttpPost("{targetMemberId}")]
        public async Task<ActionResult> ToggleLike(string targetMemberId)
        {
            var sourceMemberId = User.GetMemberId();

            if (sourceMemberId == targetMemberId)
            {
                return BadRequest("You cannot like yourself.");
            }

            var existingLike = await likeReository.GetMemberLike(sourceMemberId, targetMemberId);

            if (existingLike == null)
            {
                var newLike = new MemberLike
                {
                    SourceMemberId = sourceMemberId,
                    TargetMemberId = targetMemberId
                };
                likeReository.AddLike(newLike);
            }
            else
            {
                likeReository.DeleteLike(existingLike);
            }

            if (await likeReository.SaveAllChanges())
            {
                return Ok();
            }

            return BadRequest("Failed to update Like");
        }

        [HttpGet("list")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetCurrentMemberLikeIds()
        {
            return Ok(await likeReository.GetCurrentMemberLikesIds(User.GetMemberId()));
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Member>>> GetMemberLikes(string predicate)
        {
            var members = await likeReository.GetMemberLikes(predicate, User.GetMemberId());

            return Ok(members);
        }

    }
}
