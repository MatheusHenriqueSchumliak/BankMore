using ContaCorrente.Infrastructure.Context;
using ContaCorrente.Domain.Interfaces;
using Dapper;

namespace ContaCorrente.Infrastructure.Repositories;

public class ContaCorrenteRepository : IContaCorrenteRepository
{
	#region Construtor
	private readonly DbConnectionFactory _connectionString;

	public ContaCorrenteRepository(DbConnectionFactory connectionString)
	{
		_connectionString = connectionString;
	}
	#endregion

	public async Task Criar(Domain.Entities.ContaCorrente contaCorrente)
	{
		const string query = @"
			INSERT INTO contacorrente (idcontacorrente, numero, nome, cpf, ativo, senha, salt)
			VALUES (@Id, @Numero, @Nome, @Cpf, @Ativo, @Senha, @Salt);           ";

		using var conexao = _connectionString.CreateConnection();
		await conexao.ExecuteAsync(query, new
		{
			Id = contaCorrente.Id.ToString(),
			Numero = contaCorrente.Numero,
			Nome = contaCorrente.Nome,
			Cpf = contaCorrente.Cpf,
			Ativo = contaCorrente.Ativo ? 1 : 0,
			Senha = contaCorrente.Senha,
			Salt = contaCorrente.Salt
		});
	}

	public async Task Atualizar(Domain.Entities.ContaCorrente contaCorrente)
	{
		const string query = @"
			UPDATE contaCorrente
			SET Nome = @Nome,
				Ativo = @Ativo,
				Senha = @Senha,
				Salt = @Salt
			WHERE idcontacorrente = @Id;           ";

		using var conexao = _connectionString.CreateConnection();
		await conexao.ExecuteAsync(query, new
		{
			Id = contaCorrente.Id.ToString(),
			Nome = contaCorrente.Nome,
			Ativo = contaCorrente.Ativo ? 1 : 0,
			Senha = contaCorrente.Senha,
			Salt = contaCorrente.Salt
		});
	}

	public async Task<Domain.Entities.ContaCorrente?> BuscarPorCpf(string cpf)
	{
		const string query = @"
			SELECT idcontacorrente  AS Id,
				   numero           AS Numero,
				   nome             AS Nome,
				   cpf              AS Cpf,
				   ativo            AS Ativo,
				   senha            AS Senha,
				   salt             AS Salt
			FROM contacorrente
			WHERE cpf = @Cpf;
		";

		using var conexao = _connectionString.CreateConnection();
		return await conexao.QuerySingleOrDefaultAsync<Domain.Entities.ContaCorrente>(query, new { Cpf = cpf });
	}

	public async Task<Domain.Entities.ContaCorrente?> BuscarPorId(string id)
	{
		const string query = @"
			SELECT idcontacorrente	AS Id,
				   numero           AS Numero,
				   nome             AS Nome,
				   cpf              AS Cpf,
				   ativo            AS Ativo,
				   senha            AS Senha,
				   salt             AS Salt
			FROM contacorrente
			WHERE idcontacorrente = @Id;
		";

		using var conexao = _connectionString.CreateConnection();
		return await conexao.QuerySingleOrDefaultAsync<Domain.Entities.ContaCorrente>(query, new { Id = id });
	}

	public async Task<Domain.Entities.ContaCorrente?> BuscarPorNumero(int numero)
	{
		const string sql = @"
			SELECT idcontacorrente,
				   numero,
				   nome,
				   cpf,
				   ativo,
				   senha,
				   salt
			FROM contacorrente
			WHERE numero = @Numero;
		";

		using var conexao = _connectionString.CreateConnection();
		var result = await conexao.QuerySingleOrDefaultAsync(sql, new { Numero = numero });

		if (result == null) return null;

		return new Domain.Entities.ContaCorrente(
			Guid.Parse((string)result.idcontacorrente),
			(int)result.numero,
			(string)result.nome,
			(string)result.cpf,
			((long)result.ativo) == 1,
			(string)result.senha,
			(string)result.salt
		);

	}

	public async Task<bool> VerificaSeNumeroExiste(int numero)
	{
		const string query = @"
			SELECT COUNT(1)
			FROM contacorrente
			WHERE numero = @Numero;           ";

		using var conexao = _connectionString.CreateConnection();
		var count = await conexao.ExecuteScalarAsync<int>(query, new { Numero = numero });

		return count > 0;
	}

}