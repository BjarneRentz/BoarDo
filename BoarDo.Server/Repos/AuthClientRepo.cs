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

	public async Task AddGooleClient(string accessToken, string refreshToken)
	{
		var newClient = new OAuthClient
		{
			Id = GoogleClientKey,
			AccessToken = accessToken,
			RefreshToken = refreshToken
		};
		await _dbContext.OAuthClients.AddAsync(newClient);
		await _dbContext.SaveChangesAsync();
	}

	public Task<List<OAuthClient>> GetClients()
	{
		return _dbContext.OAuthClients.ToListAsync();
	}
}