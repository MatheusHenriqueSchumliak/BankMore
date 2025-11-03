using Microsoft.Data.Sqlite;

namespace ContaCorrente.Infrastructure.Context;

public static class DbInitializer
{
	public static void Initialize(string connectionString)
	{
		//Extrai o caminho do arquivo .db
		var dbPath = connectionString.Replace("Data Source=", "").Trim();
		var directory = Path.GetDirectoryName(dbPath);

		//Cria a pasta se não existir
		if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
		{
			Directory.CreateDirectory(directory);
		}

		//Cria a conexão (isso cria o arquivo .db automaticamente)
		using var connection = new SqliteConnection(connectionString);
		connection.Open();

		//Cria as tabelas se não existirem
		var createTablesCommand = connection.CreateCommand();

		createTablesCommand.CommandText = @"
			-- Tabela de Conta Corrente
			CREATE TABLE IF NOT EXISTS contacorrente (
				idcontacorrente TEXT(37) PRIMARY KEY,
				numero INTEGER(10) NOT NULL UNIQUE,
				nome TEXT(100) NOT NULL,
				ativo INTEGER(1) NOT NULL DEFAULT 0,
				senha TEXT(100) NOT NULL,
				salt TEXT(100) NOT NULL,
				CHECK (ativo IN (0, 1))
			);

			-- Tabela de Movimentos
			CREATE TABLE IF NOT EXISTS movimento (
				idmovimento TEXT(37) PRIMARY KEY,
				idcontacorrente TEXT(37) NOT NULL,
				datamovimento TEXT(25) NOT NULL,
				tipomovimento TEXT(1) NOT NULL,
				valor REAL NOT NULL,
				CHECK (tipomovimento IN ('C', 'D')),
				FOREIGN KEY(idcontacorrente) REFERENCES contacorrente(idcontacorrente)
			);

			-- Tabela de Idempotência
			CREATE TABLE IF NOT EXISTS idempotencia (
				chave_idempotencia TEXT(37) PRIMARY KEY,
				requisicao TEXT(1000),
				resultado TEXT(1000)
			);

		";

		createTablesCommand.ExecuteNonQuery();
	}
}
