using BoarDo.Server.Database;
using BoarDo.Server.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace BoarDo.Server.Repos;

public class AuthClientRepo : IAuthClientsRepo
{
    private const string GoogleClientKey = "Google";
    private const string TickTickClientKey = "TickTick";

    private readonly List<string> _supportedAuthProviders = new() { GoogleClientKey, TickTickClientKey }; 

    private readonly BoarDoContext _dbContext;

    public AuthClientRepo(BoarDoContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddOrUpdateGoogleClientAsync(string accessToken, string refreshToken, string clientId,
        string clientSecret)
    {

        var clientExists = await _dbContext.OAuthClients.AnyAsync(c => c.Id ==GoogleClientKey);

        var client = new OAuthClient
        {
            Id = GoogleClientKey,
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ClientSecret = clientSecret,
            ClientId = clientId
        };

        if (clientExists)
        {
            _dbContext.OAuthClients.Update(client);
        }
        else
        {
            await _dbContext.OAuthClients.AddAsync(client);
        }

        await _dbContext.SaveChangesAsync();
    }
    

    public async Task<OAuthClient?> GetGoogleAccessTokenAsync()
    {
        return await _dbContext.OAuthClients.FindAsync(GoogleClientKey);
    }

    public async Task<Dictionary<string, bool>> GetClientsAsync()
    {
        var result = _supportedAuthProviders.ToDictionary(key => key, value => false);
        
        var configuredClients = await _dbContext.OAuthClients.ToListAsync();
        
        configuredClients.ForEach(c => result[c.Id] = true);

        return result;

    }
}