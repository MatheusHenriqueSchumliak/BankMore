using Microsoft.Data.Sqlite;
using System.Data;

namespace ContaCorrente.Infrastructure.Context;

public class DbConnectionFactory
{
	private readonly string _connectionString;
	public DbConnectionFactory(string connectionString)
	{
		_connectionString = connectionString;
	}

	public IDbConnection CreateConnection()
	{
		var connection = new SqliteConnection(_connectionString);
		
		connection.Open();
		
		return connection;
	}

}
