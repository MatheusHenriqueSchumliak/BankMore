using ContaCorrente.Application.Commands;

namespace ContaCorrente.Test.Commands;

public class InativarContaCorrenteCommandTests
{
	[Fact]
	public void Deve_Criar_Command_Com_Valores_Corretos()
	{
		// Arrange
		var numeroConta = 1234567;
		var senha = "senha123";

		// Act
		var command = new InativarContaCorrenteCommand
		{
			NumeroConta = numeroConta,
			Senha = senha
		};

		// Assert
		Assert.Equal(numeroConta, command.NumeroConta);
		Assert.Equal(senha, command.Senha);
	}

	[Fact]
	public void Deve_Criar_Command_Com_Valores_Default()
	{
		// Act
		var command = new InativarContaCorrenteCommand();

		// Assert
		Assert.Equal(0, command.NumeroConta);
		Assert.Equal(string.Empty, command.Senha);
	}
}
