using BoarDo.Server.Database;
using BoarDo.Server.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace BoarDo.Server.Repos;

public class AuthClientRepo : IAuthClientsRepo
{
    private const string GoogleClientKey = "Google";
    private const string TickTickClientKey = "TickTick";

    private readonly List<string> SupportedAuthProviders = new List<string>() { GoogleClientKey, TickTickClientKey }; 

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

    public async Task<Dictionary<string, bool>> GetClientsAsync()
    {
        var result = SupportedAuthProviders.ToDictionary(key => key, value => false);
        
        var configuredClients = await _dbContext.OAuthClients.ToListAsync();
        
        configuredClients.ForEach(c => result[c.Id] = true);

        return result;

    }
}