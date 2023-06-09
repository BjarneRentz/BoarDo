using BoarDo.Server.Database;
using BoarDo.Server.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace BoarDo.Server.Repos;

public class AuthClientRepo : IAuthClientsRepo
{
    private const string GoogleClientKey = "Google";

    private readonly BoarDoContext _dbContext;

    public AuthClientRepo(BoarDoContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddGoogleClientAsync(string accessToken, string refreshToken, string clientId,
        string clientSecret)
    {
        var newClient = new OAuthClient
        {
            Id = GoogleClientKey,
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ClientSecret = clientSecret,
            ClientId = clientId
        };
        await _dbContext.OAuthClients.AddAsync(newClient);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<OAuthClient?> GetGoogleAccessTokenAsync()
    {
        return await _dbContext.OAuthClients.FindAsync(GoogleClientKey);
    }

    public Task<List<OAuthClient>> GetClientsAsync()
    {
        return _dbContext.OAuthClients.ToListAsync();
    }
}