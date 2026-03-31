using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedUsers(UserManager<AppUser> userManager)
        {
            if (await userManager.Users.AnyAsync()) return;

            var memberData = await File.ReadAllTextAsync("Data/UserSeedData.json");
            var members = JsonSerializer.Deserialize<List<SeedUserDto>>(memberData);

            if (members == null)
            {
                Console.WriteLine("No members found in the seed data.");
                return;
            }
          
            foreach (var member in members)
            {
                var user = new AppUser
                {
                    Id = member.Id,
                    Email = member.Email,
                    DisplayName = member.DisplayName,
                    ImageUrl = member.ImageUrl,
                    UserName = member.Email,
                    Member = new Member
                    {
                        Id = member.Id,
                        DateOfBirth = member.DateOfBirth,
                        ImageUrl = member.ImageUrl,
                        DisplayName = member.DisplayName,
                        City = member.City,
                        Country = member.Country,
                        Description = member.Description,
                        Gender = member.Gender,
                        Created = member.Created,
                        LastActive = member.LastActive,
                    }
                };

                user.Member.Photos.Add(new Photo
                {
                    Url = member.ImageUrl!,
                    MemberId = member.Id,
                });

                var result = await userManager.CreateAsync(user, "Pa$$w0rd");
                if(!result.Succeeded)
                {
                    Console.WriteLine(result.Errors.First().Description);
                }
                await userManager.AddToRoleAsync(user, "Member");
            }

            var admin = new AppUser
            {
                UserName = "admin@test.com",
                Email = "admin@test.com",
                DisplayName = "Admin",
            };

            await userManager.CreateAsync(admin, "Pa$$w0rd");
            await userManager.AddToRolesAsync(admin, new[] { "Admin", "Moderator" });
        }
}
