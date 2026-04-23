using API.Data;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class LikesController(IUnitOFWork uow) : BaseApiController
    {
        [HttpPost("{targetMemberId}")]
        public async Task<ActionResult> ToggleLike(string targetMemberId)
        {
            var sourceMemberId = User.GetMemberId();

            if (sourceMemberId == targetMemberId)
            {
                return BadRequest("You cannot like yourself.");
            }

            var existingLike = await uow.LikesRepository.GetMemberLike(sourceMemberId, targetMemberId);

            if (existingLike == null)
            {
                var newLike = new MemberLike
                {
                    SourceMemberId = sourceMemberId,
                    TargetMemberId = targetMemberId
                };
                uow.LikesRepository.AddLike(newLike);
            }
            else
            {
                uow.LikesRepository.DeleteLike(existingLike);
            }

            if (await uow.Complete())
            {
                return Ok();
            }

            return BadRequest("Failed to update Like");
        }

        [HttpGet("list")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetCurrentMemberLikeIds()
        {
            return Ok(await uow.LikesRepository.GetCurrentMemberLikesIds(User.GetMemberId()));
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedResult<Member>>> GetMemberLikes(
        [FromQuery] LikesParams likesParams)
        {
            likesParams.MemberId = User.GetMemberId();
            var members = await uow.LikesRepository.GetMemberLikes(likesParams);

            return Ok(members);
        }

    }
}
