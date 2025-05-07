using System.Collections.Generic;
using Duende.IdentityServer.Models;

namespace Play.Identity.API.Settings;

public class IdentityServerSettings
{
    public IReadOnlyCollection<ApiScope> ApiScopes { get; init; }
    public IReadOnlyCollection<ApiResource> ApiResources { get; init; }
    public IReadOnlyCollection<Client> Clients { get; init; }
    public IReadOnlyCollection<IdentityResource> IdentityResources =>
        [
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        ];
    public string ClientId { get; init; } = string.Empty;
    public IReadOnlyCollection<string> AllowedGrantTypes { get; init; } = [];
    public bool RequireClientSecret { get; init; } = true;
    public IReadOnlyCollection<string> RedirectUris { get; init; } = [];
    public IReadOnlyCollection<string> AllowedScopes { get; init; } = [];
    public bool AlwaysIncludeUserClaimsInIdToken { get; init; } = true;
}