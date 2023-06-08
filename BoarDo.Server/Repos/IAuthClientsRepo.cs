using BoarDo.Server.Database.Models;

namespace BoarDo.Server.Repos;

public interface IAuthClientsRepo
{
	public Task AddGooleClient(string accessToken, string refreshToken);

	public Task<List<OAuthClient>> GetClients();
}