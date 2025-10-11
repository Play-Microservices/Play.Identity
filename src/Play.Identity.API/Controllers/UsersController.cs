using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Play.Identity.API.Dtos;
using Play.Identity.API.Entities;
using static Duende.IdentityServer.IdentityServerConstants;

namespace Play.Identity.API.Controllers;

[ApiController]
[Authorize(Policy = LocalApi.PolicyName, Roles = Roles.Admin)]
[Route("users")]
public class UsersController(
    UserManager<ApplicationUser> userManager
    ) : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    [HttpGet]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    public ActionResult<UserDto> GetAllAsync()
    {
        var users = _userManager.Users.ToList();

        return Ok(users.Select(user => user.AsDto()));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDto>> GetByIdAsync(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());

        if (user is null)
        {
            return NotFound();
        }

        return Ok(user.AsDto());
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> PutAsync(Guid id, UpdateUserDto userDto)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());

        if (user is null)
        {
            return NotFound();
        }

        user.Email = userDto.Email;
        user.UserName = userDto.Email;
        user.Gil = userDto.Gil;

        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
        {
            return NoContent();
        }

        return BadRequest(result.Errors);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeleteAsync(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());

        if (user is null)
        {
            return NotFound();
        }

        var result = await _userManager.DeleteAsync(user);

        if (result.Succeeded)
        {
            return NoContent();
        }

        return BadRequest(result.Errors);
    }
}