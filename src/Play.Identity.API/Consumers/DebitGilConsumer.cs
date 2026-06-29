using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Play.Identity.API.Entities;
using Play.Identity.API.Exceptions;
using Play.Identity.Contracts;

namespace Play.Identity.API.Consumers;

public class DebitGilConsumer(UserManager<ApplicationUser> userManager) : IConsumer<DebitGil>
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    
    public async Task Consume(ConsumeContext<DebitGil> context)
    {
        var message = context.Message;
        var user = await _userManager.FindByIdAsync(message.UserId.ToString());
        if (user is null)
        {
            throw new UnknownUserException(message.UserId);
        }

        user.Gil -= message.Gil;
        if (user.Gil < 0)
        {
            throw new InsufficientFundsException(message.UserId, message.Gil);
        }
        
        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
        {
            await context.Publish<GilDebited>(message.CorrelationId);
        }
    }
}
