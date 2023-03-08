using Digitime.Server.Application.Abstractions;
using Digitime.Server.Domain.Users;
using Digitime.Server.Infrastructure.Entities;
using Digitime.Server.Infrastructure.MongoDb;
using Mapster;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Digitime.Server.Infrastructure.Http;

internal class UserRepository : MongoRepository<UserEntity>, IUserRepository
{
    private static Auth0ManagementClient _auth0;

    public UserRepository(IMongoDbSettings settings, IConfiguration config) : base(settings)
    {
        _auth0 = new Auth0ManagementClient(config);
        var database = new MongoClient(settings.ConnectionString).GetDatabase(settings.DatabaseName);
        Collection = database.GetCollection<UserEntity>(settings.UsersCollectionName);
    }

    public Task DeleteAsync(string id)
    {
        return Collection.DeleteOneAsync(x => x.Id == id);
    }

    public async Task<User> GetByEmail(string email)
    {
        // First, try to get the user from the DB if he exists
        var dbEntity = await Collection.Find(x => x.Email == email).SingleOrDefaultAsync();
        if (dbEntity is not null)
            return dbEntity.Adapt<User>();

        // If the user does not exist in the DB, try to get it from the IdProvider's API
        var idProviderUser = await _auth0.GetByEmail(email);
        // We save a backup in the DB for faster access later
        await base.InsertOneAsync(idProviderUser);
        dbEntity = await Collection.Find(x => x.ExternalId == idProviderUser.Id).SingleOrDefaultAsync();
        
        return idProviderUser.Adapt<User>();
    }

    public async Task<User> GetbyIdAsync(string id)
    {
        // First, try to get the user from the DB if he exists
        var dbEntity = await Collection.Find(x => x.ExternalId == id).SingleOrDefaultAsync();
        return dbEntity.Adapt<User>();
    }

    public async Task<User> GetbyExternalIdAsync(string externalId)
    {
        // First, try to get the user from the DB if he exists
        var dbEntity = await Collection.Find(x => x.ExternalId == externalId).SingleOrDefaultAsync();
        if (dbEntity is not null)
            return dbEntity.Adapt<User>();

        // If the user does not exist in the DB, try to get it from the IdProvider's API
        var idProviderUser = await _auth0.GetById(externalId);
        if (idProviderUser is null)
            throw new ApplicationException("The user does not exist in the DB nor in the IdProvider's database.");

        // We save a backup in the DB for faster access later
        await base.InsertOneAsync(idProviderUser);
        dbEntity = await Collection.Find(x => x.ExternalId == externalId).SingleOrDefaultAsync();

        return dbEntity.Adapt<User>();
    }

    public Task UpdateAsync(User user)
    {
        var entity = user.Adapt<UserEntity>();
        var filter = Builders<UserEntity>.Filter.Eq(doc => doc.Id, entity.Id);
        return Collection.FindOneAndReplaceAsync(filter, entity);
    }
}
