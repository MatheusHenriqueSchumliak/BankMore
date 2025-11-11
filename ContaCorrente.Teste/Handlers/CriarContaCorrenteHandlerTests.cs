using ContaCorrente.Infrastructure.Security;
using ContaCorrente.Application.Commands;
using ContaCorrente.Application.Handlers;
using ContaCorrente.Domain.Interfaces;
using Moq;

namespace ContaCorrente.Test.Handlers;

public class CriarContaCorrenteHandlerTests
{
	[Fact]
	public async Task Handle_DeveRetornarNumeroContaEToken()
	{
		// Arrange
		var repoMock = new Mock<IContaCorrenteRepository>();
		repoMock.Setup(r => r.VerificaSeNumeroExiste(It.IsAny<int>())).ReturnsAsync(false);
		repoMock.Setup(r => r.Criar(It.IsAny<ContaCorrente.Domain.Entities.ContaCorrente>())).Returns(Task.CompletedTask);

		var jwtMock = new Mock<IGeradorTokenJwt>();
		jwtMock.Setup(j => j.GerarToken(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<string>())).Returns("token-fake");

		var handler = new CriarContaCorrenteHandler(repoMock.Object, jwtMock.Object);

		var command = new CriarContaCorrenteCommand
		{
			Cpf = "123.456.789-00",
			Nome = "Teste",
			Senha = "senha123"
		};

		// Act
		var result = await handler.Handle(command, default);

		// Assert
		Assert.False(string.IsNullOrEmpty(result.NumeroConta));
		Assert.Equal("token-fake", result.Token);
	}

	[Fact]
	public async Task Handle_DeveCriarContaQuandoNumeroContaNaoExiste()
	{
		// Arrange
		var repoMock = new Mock<IContaCorrenteRepository>();
		// Sempre retorna false, simulando que o número nunca existe
		repoMock.Setup(r => r.VerificaSeNumeroExiste(It.IsAny<int>())).ReturnsAsync(false);
		repoMock.Setup(r => r.Criar(It.IsAny<ContaCorrente.Domain.Entities.ContaCorrente>())).Returns(Task.CompletedTask);

		var jwtMock = new Mock<IGeradorTokenJwt>();
		jwtMock.Setup(j => j.GerarToken(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<string>())).Returns("token-fake");

		var handler = new CriarContaCorrenteHandler(repoMock.Object, jwtMock.Object);

		var command = new CriarContaCorrenteCommand
		{
			Cpf = "123.456.789-00",
			Nome = "Teste",
			Senha = "senha123"
		};

		// Act
		var result = await handler.Handle(command, default);

		// Assert
		Assert.False(string.IsNullOrEmpty(result.NumeroConta));
		Assert.Equal("token-fake", result.Token);
	}

	[Fact]
	public async Task Handle_DeveLancarExcecaoQuandoRepositorioFalha()
	{
		// Arrange
		var repoMock = new Mock<IContaCorrenteRepository>();
		repoMock.Setup(r => r.VerificaSeNumeroExiste(It.IsAny<int>())).ReturnsAsync(false);
		repoMock.Setup(r => r.Criar(It.IsAny<ContaCorrente.Domain.Entities.ContaCorrente>())).ThrowsAsync(new Exception("Erro ao persistir"));

		var jwtMock = new Mock<IGeradorTokenJwt>();
		jwtMock.Setup(j => j.GerarToken(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<string>())).Returns("token-fake");

		var handler = new CriarContaCorrenteHandler(repoMock.Object, jwtMock.Object);

		var command = new CriarContaCorrenteCommand
		{
			Cpf = "123.456.789-00",
			Nome = "Teste",
			Senha = "senha123"
		};

		// Act & Assert
		await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, default));
	}

}
