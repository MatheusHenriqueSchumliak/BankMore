using ContaCorrente.Application.Commands;
using ContaCorrente.Application.Handlers;
using ContaCorrente.Domain.Interfaces;
using Moq;

namespace ContaCorrente.Test.Handlers;

public class InativarContaCorrenteHandlerTests
{
	[Fact]
	public async Task Handle_DeveInativarConta_QuandoSenhaCorreta()
	{
		// Arrange
		var (hash, salt) = ContaCorrente.Domain.ValueObjects.HashDeSenha.CriarHash("senha123");
		var conta = new Domain.Entities.ContaCorrente(
			Guid.NewGuid(), 1234567, "Teste", "123.456.789-00", true, hash, salt
		);

		var repoMock = new Mock<IContaCorrenteRepository>();
		repoMock.Setup(r => r.BuscarPorNumero(1234567)).ReturnsAsync(conta);
		repoMock.Setup(r => r.Atualizar(It.IsAny<Domain.Entities.ContaCorrente>())).Returns(Task.CompletedTask);

		var handler = new InativarContaCorrenteHandler(repoMock.Object);

		var command = new InativarContaCorrenteCommand
		{
			NumeroConta = 1234567,
			Senha = "senha123"
		};

		// Act
		var result = await handler.Handle(command, default);

		// Assert
		Assert.True(result.Sucesso);
		Assert.Null(result.Mensagem);
		Assert.Null(result.Tipo);
	}

	[Fact]
	public async Task Handle_DeveRetornarErro_QuandoSenhaInvalida()
	{
		// Arrange
		var (hash, salt) = ContaCorrente.Domain.ValueObjects.HashDeSenha.CriarHash("senha123");
		var conta = new Domain.Entities.ContaCorrente(
			Guid.NewGuid(), 1234567, "Teste", "123.456.789-00", true, hash, salt
		);

		var repoMock = new Mock<IContaCorrenteRepository>();
		repoMock.Setup(r => r.BuscarPorNumero(1234567)).ReturnsAsync(conta);

		var handler = new InativarContaCorrenteHandler(repoMock.Object);

		var command = new InativarContaCorrenteCommand
		{
			NumeroConta = 1234567,
			Senha = "senhaErrada"
		};

		// Act
		var result = await handler.Handle(command, default);

		// Assert
		Assert.False(result.Sucesso);
		Assert.Equal("Senha inválida.", result.Mensagem);
		Assert.Equal("USER_UNAUTHORIZED", result.Tipo);
	}

	[Fact]
	public async Task Handle_DeveRetornarErro_QuandoContaNaoEncontrada()
	{
		// Arrange
		var repoMock = new Mock<IContaCorrenteRepository>();
		repoMock.Setup(r => r.BuscarPorNumero(It.IsAny<int>())).ReturnsAsync((Domain.Entities.ContaCorrente?)null);

		var handler = new InativarContaCorrenteHandler(repoMock.Object);

		var command = new InativarContaCorrenteCommand
		{
			NumeroConta = 9999999,
			Senha = "senha123"
		};

		// Act
		var result = await handler.Handle(command, default);

		// Assert
		Assert.False(result.Sucesso);
		Assert.Equal("Conta corrente não encontrada.", result.Mensagem);
		Assert.Equal("INVALID_ACCOUNT", result.Tipo);
	}

}
