using ContaCorrente.Infrastructure.Context;
using ContaCorrente.Domain.Interfaces;

namespace ContaCorrente.Infrastructure.Repositories;

public class ContaCorrenteRepository : IContaCorrenteRepository
{
	private readonly DbConnectionFactory _connectionString;

	public ContaCorrenteRepository(DbConnectionFactory connectionString)
	{
		_connectionString = connectionString;
	}


}
