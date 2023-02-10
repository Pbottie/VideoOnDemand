using Microsoft.AspNetCore.Mvc;
using VOD.Common.Classes;
using VOD.Common.DTOs;

namespace VOD.Users.API.Controllers;


[ApiController]
public class UsersController : ControllerBase
{
    private readonly UserManager<VODUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;


    public UsersController(UserManager<VODUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;

    }

    private async Task<IResult> AddUser(string email, string password)
    {
        try
        {
            if (!ModelState.IsValid) return Results.BadRequest();

            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null) return Results.BadRequest();

            VODUser user = new()
            {
                Id = Guid.NewGuid().ToString(),
                Email = email,
                EmailConfirmed = true,
                UserName = email
            };

            var result = await _userManager.CreateAsync(user);

            if (result.Succeeded) return Results.Ok();
        }
        catch (Exception)
        {
        }
        return Results.BadRequest();
    }

    private async Task<IResult> AddRoles(string email, List<string> roles)
    {
        try
        {
            if (!ModelState.IsValid) return Results.BadRequest();

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) return Results.BadRequest();

            var result = await _userManager.AddToRolesAsync(user, roles);

            if (result.Succeeded) return Results.Ok();


        }
        catch (Exception)
        {

        }
        return Results.BadRequest();
    }

    [Route("api/users/seed")]
    [HttpPost]
    public async Task<IResult> Seed()
    {

        try
        {
            await _roleManager.CreateAsync(new IdentityRole { Id = "1", Name = UserRole.Admin });
            await _roleManager.CreateAsync(new IdentityRole { Id = "2", Name = UserRole.Customer });
            await _roleManager.CreateAsync(new IdentityRole { Id = "3", Name = UserRole.Registered });

            await AddUser("john@vod.com", "123Password");
            await AddUser("jane@vod.com", "321Password");

            await AddRoles("john@vod.com", new List<string> { UserRole.Admin, UserRole.Customer, UserRole.Registered });
            await AddRoles("jane@vod.com", new List<string> { UserRole.Customer, UserRole.Registered });

            return Results.Ok();

        }
        catch (Exception)
        { }

        return Results.BadRequest();

    }

    [Route("api/users/register")]
    [HttpPost]
    public async Task<IResult> Register(RegisterUserDTO registerUserDTO)
    {
        try
        {
            var result = await AddUser(registerUserDTO.Email, registerUserDTO.Password);

            if (result == Results.BadRequest()) return Results.BadRequest();

            result = await AddRoles(registerUserDTO.Email, registerUserDTO.Roles);

            return Results.Ok();

        }
        catch (Exception)
        {

        }
        return Results.BadRequest();

    }

    [Route("api/users/paid")]
    [HttpPost]
    public async Task<IResult> Paid(PaidCustomerDTO paidCustomerDTO)
    {
        return await AddRoles(paidCustomerDTO.Email, new List<string> { UserRole.Customer });
    }


}
