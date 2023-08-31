using BoarDo.Server.Database.Models;

namespace BoarDo.Server.Repos;

public interface IAuthClientsRepo
{
    public Task AddOrUpdateGoogleClientAsync(string accessToken, string refreshToken, string clientId, string clientSecret);

    public Task<OAuthClient?> GetGoogleAccessTokenAsync();

    public Task<Dictionary<OAuthClientProvider, bool>> GetClientsAsync();

    public Task<bool> DeleteClientAsync(OAuthClientProvider provider);
}