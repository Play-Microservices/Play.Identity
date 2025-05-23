using System;
using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace Play.Identity.API.Entities;

[CollectionName("Users")]
public class ApplicationUser : MongoIdentityUser<Guid>
{
    public decimal Gil { get; set; }
}