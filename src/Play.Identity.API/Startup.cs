using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Play.Common.Settings;
using Play.Identity.API.Entities;
using Play.Identity.API.HostedServices;
using Play.Identity.API.Settings;

namespace Play.Identity.API;

public class Startup(IConfiguration configuration)
{
    private const string AllowedOriginSettings = "AllowedOrigins";

    public IConfiguration Configuration { get; } = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
        var serviceSettings = Configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
        var mongoDbSettings = Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
        var identityServerSettings = Configuration.GetSection(nameof(IdentityServerSettings)).Get<IdentityServerSettings>();

        services.Configure<IdentitySettings>(Configuration.GetSection(nameof(IdentitySettings)))
            .AddDefaultIdentity<ApplicationUser>()
            .AddRoles<ApplicationRole>()
            .AddMongoDbStores<ApplicationUser, ApplicationRole, Guid>(
                mongoDbSettings.ConnectionString,
                serviceSettings.ServiceName);

        services.AddIdentityServer(options =>
            {
                options.Events.RaiseSuccessEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseErrorEvents = true;
            })
            .AddAspNetIdentity<ApplicationUser>()
            .AddInMemoryApiScopes(identityServerSettings.ApiScopes)
            .AddInMemoryApiResources(identityServerSettings.ApiResources)
            .AddInMemoryClients(identityServerSettings.Clients)
            .AddInMemoryIdentityResources(identityServerSettings.IdentityResources);

        services.AddLocalApiAuthentication();

        services.AddControllers();
        services.AddHostedService<IdentitySeedHostedService>();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Play.Identity.Service", Version = "v1" });
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Play.Identity.Service v1"));

            app.UseCors(builder =>
            {
                builder.WithOrigins(Configuration[AllowedOriginSettings])
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();

        app.UseRouting();

        app.UseIdentityServer();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapRazorPages();
        });
    }
}
