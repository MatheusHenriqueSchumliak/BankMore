using ContaCorrente.Infrastructure.Repositories;
using ContaCorrente.Infrastructure.Context;
using ContaCorrente.Domain.Entities;
using ContaCorrente.Domain.Enums;
using Microsoft.Data.Sqlite;
using Dapper;

namespace ContaCorrente.Test.Repositories;

public class MovimentoRepositoryTests : IDisposable
{
	#region Construtor 
	private static string InMemoryConnectionString => "Data Source=TestDbMovimento;Mode=Memory;Cache=Shared";
	private readonly SqliteConnection _sharedConnection;
	private readonly MovimentoRepository _repo;

	public MovimentoRepositoryTests()
	{
		_sharedConnection = new SqliteConnection(InMemoryConnectionString);
		_sharedConnection.Open();
		DbInitializer.Initialize(InMemoryConnectionString);

		var factory = new DbConnectionFactory(InMemoryConnectionString);
		_repo = new MovimentoRepository(factory);
	}
	#endregion

	[Fact]
	public async Task Adicionar_DeveInserirMovimentoNoBanco()
	{
		// Arrange
		var contaId = Guid.NewGuid();
		// Cria a conta corrente para satisfazer a FK
		await _sharedConnection.ExecuteAsync(
			@"INSERT INTO contacorrente (idcontacorrente, numero, nome, cpf, ativo, senha, salt)
          VALUES (@id, @numero, @nome, @cpf, @ativo, @senha, @salt)",
			new
			{
				id = contaId.ToString(),
				numero = 1234567,
				nome = "Teste",
				cpf = "123.456.789-00",
				ativo = 1,
				senha = "hash",
				salt = "salt"
			});

		var movimento = new Movimento(
			Guid.NewGuid(),
			contaId,
			DateTime.UtcNow,
			TipoMovimento.Credito,
			100.50m
		);

		// Act
		await _repo.Adicionar(movimento);

		// Assert
		var result = await _sharedConnection.QuerySingleOrDefaultAsync(
			"SELECT * FROM movimento WHERE idmovimento = @id",
			new { id = movimento.Id.ToString() }
		);
		Assert.NotNull(result);
		Assert.Equal(movimento.Valor, (decimal)result.valor);
		Assert.Equal("C", (string)result.tipomovimento);
	}

	[Fact]
	public async Task BuscarPorConta_DeveRetornarZeroQuandoNaoHaMovimento()
	{
		// Arrange
		var contaId = Guid.NewGuid();
		// Cria a conta corrente para satisfazer a FK, mesmo sem movimentos
		await _sharedConnection.ExecuteAsync(
			@"INSERT INTO contacorrente (idcontacorrente, numero, nome, cpf, ativo, senha, salt)
          VALUES (@id, @numero, @nome, @cpf, @ativo, @senha, @salt)",
			new
			{
				id = contaId.ToString(),
				numero = 7654321,
				nome = "SemMovimento",
				cpf = "000.000.000-00",
				ativo = 1,
				senha = "hash",
				salt = "salt"
			});

		// Act
		var saldo = await _repo.BuscarPorConta(contaId);

		// Assert
		Assert.Equal(0m, saldo);
	}

	public void Dispose()
	{
		_sharedConnection?.Dispose();
	}
}
