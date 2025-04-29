using System.Collections.Generic;
using Duende.IdentityServer.Models;

namespace Play.Identity.API.Settings;

public class IdentityServerSettings
{
    public IReadOnlyCollection<ApiScope> ApiScopes { get; init; } = [];
    public IReadOnlyCollection<Client> Clients { get; init; } = [];
    public IReadOnlyCollection<IdentityResource> IdentityResources =>
        [
            new IdentityResources.OpenId()
        ];
}