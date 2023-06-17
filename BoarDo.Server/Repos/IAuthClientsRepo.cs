using BoarDo.Server.Database.Models;

namespace BoarDo.Server.Repos;

public interface IAuthClientsRepo
{
    public Task AddGoogleClientAsync(string accessToken, string refreshToken, string clientId, string clientSecret);

    public Task<OAuthClient?> GetGoogleAccessTokenAsync();

    public Task<Dictionary<string, bool>> GetClientsAsync();
}