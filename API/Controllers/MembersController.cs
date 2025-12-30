using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public  class MembersController(AppDbContext context) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<AppUser>>> GetMembers()
        {
            var members = await context.Users.ToListAsync();

            return members;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetMembers(string id)
        {
            var member = await context.Users.FindAsync(id);

            return member is null ? NotFound() : member ;
        }
    }
}
