using BoarDo.Server.Database;
using BoarDo.Server.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace BoarDo.Server.Repos;

public class AuthClientRepo : IAuthClientsRepo
{

    private readonly List<OAuthClientProvider> _supportedAuthProviders = new() { OAuthClientProvider.Google, OAuthClientProvider.TickTick }; 

    private readonly BoarDoContext _dbContext;

    public AuthClientRepo(BoarDoContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddOrUpdateGoogleClientAsync(string accessToken, string refreshToken, string clientId,
        string clientSecret)
    {

        var clientExists = await _dbContext.OAuthClients.AnyAsync(c => c.Id ==OAuthClientProvider.Google);

        var client = new OAuthClient
        {
            Id = OAuthClientProvider.Google,
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
        return await _dbContext.OAuthClients.FindAsync(OAuthClientProvider.Google);
    }

    public async Task<Dictionary<OAuthClientProvider, bool>> GetClientsAsync()
    {
        var result = _supportedAuthProviders.ToDictionary(key => key, value => false);
        
        var configuredClients = await _dbContext.OAuthClients.ToListAsync();
        
        configuredClients.ForEach(c => result[c.Id] = true);

        return result;

    }

    public async Task<bool> DeleteClientAsync(OAuthClientProvider provider)
    {
        var client = await _dbContext.OAuthClients.FindAsync(provider);
        if (client == null)
            return false;

        _dbContext.OAuthClients.Remove(client);

        await _dbContext.SaveChangesAsync();

        return true;
    }
}