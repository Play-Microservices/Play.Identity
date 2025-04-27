using Play.Identity.API.Dtos;
using Play.Identity.API.Entities;

namespace Play.Identity.API;

public static class Extensions
{
    public static UserDto AsDto(this ApplicationUser user)
    {
        return new UserDto(
            user.Id, 
            user.UserName, 
            user.Email, 
            user.Gil, 
            user.CreatedOn);
    }
}