using API.Data;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class MembersController(IMemberRepository memberRepository) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Member>>> GetMembers()
        {
            return Ok(await memberRepository.GetMemberAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Member>> GetMembers(string id)
        {
            var member = await memberRepository.GetMemberByIdAsync(id);

            return member is null ? NotFound() : member;
        }

        [HttpGet("{id}/photos")]
        public async Task<ActionResult<IReadOnlyList<Photo>>> GetPhotosForMember(string id)
        {
            var photos = await memberRepository.GetPhotosForMemberAsync(id);
            return photos is null ? NotFound() : Ok(photos);
        }
    }
}
