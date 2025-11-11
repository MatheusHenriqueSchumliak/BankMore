using ContaCorrente.Infrastructure.Repositories;
using ContaCorrente.Infrastructure.Context;
using Microsoft.Data.Sqlite;
using Dapper;

namespace ContaCorrente.Test.Repositories;

public class ContaCorrenteRepositoryTests : IDisposable
{
	#region Construtor
	private static string InMemoryConnectionString => "Data Source=TestDb;Mode=Memory;Cache=Shared";
	private readonly SqliteConnection _sharedConnection;
	private readonly ContaCorrenteRepository _repo;
	public ContaCorrenteRepositoryTests()
	{
		// Abre e mantém a conexão compartilhada durante toda a execução dos testes
		_sharedConnection = new SqliteConnection(InMemoryConnectionString);
		_sharedConnection.Open();
		DbInitializer.Initialize(InMemoryConnectionString);

		var factory = new DbConnectionFactory(InMemoryConnectionString);
		_repo = new ContaCorrenteRepository(factory);
	}
	#endregion 

	[Fact]
	public async Task Criar_AdicionaContaNoBanco()
	{
		// Arrange
		var conta = new Domain.Entities.ContaCorrente(
			Guid.NewGuid(), 1234567, "Teste", "123.456.789-00", true, "hash", "salt"
		);

		// Act
		await _repo.Criar(conta);

		// Assert
		var result = await _sharedConnection.QuerySingleOrDefaultAsync("SELECT * FROM contacorrente WHERE numero = @numero", new { numero = 1234567 });
		Assert.NotNull(result);
		Assert.Equal("Teste", (string)result.nome);
	}

	[Fact]
	public async Task BuscarPorNumero_DeveRetornarContaExistente()
	{
		// Arrange
		var conta = new Domain.Entities.ContaCorrente(
			Guid.NewGuid(), 7654321, "Outro", "987.654.321-00", true, "hash2", "salt2"
		);
		await _repo.Criar(conta);

		// Act
		var encontrada = await _repo.BuscarPorNumero(7654321);

		// Assert
		Assert.NotNull(encontrada);
		Assert.Equal("Outro", encontrada.Nome);
		Assert.Equal("987.654.321-00", encontrada.Cpf);
	}

	[Fact]
	public async Task VerificaSeNumeroExiste_DeveRetornarTrueSeExistir()
	{
		// Arrange
		var conta = new Domain.Entities.ContaCorrente(
			Guid.NewGuid(), 1111111, "Teste", "111.111.111-11", true, "hash", "salt"
		);
		await _repo.Criar(conta);

		// Act
		var existe = await _repo.VerificaSeNumeroExiste(1111111);

		// Assert
		Assert.True(existe);
	}

	[Fact]
	public async Task Atualizar_DeveAlterarDadosDaConta()
	{
		// Arrange
		var conta = new Domain.Entities.ContaCorrente(
			Guid.NewGuid(), 2222222, "Antigo", "222.222.222-22", true, "hash", "salt"
		);
		await _repo.Criar(conta);

		conta.Nome = "NovoNome";
		conta.Ativo = false;
		await _repo.Atualizar(conta);

		// Assert
		var result = await _repo.BuscarPorNumero(2222222);
		Assert.NotNull(result);
		Assert.Equal("NovoNome", result.Nome);
		Assert.False(result.Ativo);
	}

	public void Dispose()
	{
		_sharedConnection?.Dispose();
	}

}
