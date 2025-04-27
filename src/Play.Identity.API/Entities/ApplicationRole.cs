using System;
using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace Play.Identity.API.Entities;

[CollectionName("Roles")]
public class ApplicationRole : MongoIdentityRole<Guid>
{
    
}