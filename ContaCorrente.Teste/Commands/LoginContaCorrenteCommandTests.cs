using ContaCorrente.Application.Commands;

namespace ContaCorrente.Test.Commands;

public class LoginContaCorrenteCommandTests
{
	[Fact]
	public void Deve_Criar_Command_Com_Valores_Corretos()
	{
		// Arrange
		var numeroConta = "1234567";
		var cpf = "123.456.789-00";
		var senha = "senha123";

		// Act
		var command = new LoginContaCorrenteCommand
		{
			NumeroConta = numeroConta,
			Cpf = cpf,
			Senha = senha
		};

		// Assert
		Assert.Equal(numeroConta, command.NumeroConta);
		Assert.Equal(cpf, command.Cpf);
		Assert.Equal(senha, command.Senha);
	}

	[Fact]
	public void Deve_Criar_Command_Com_Valores_Default()
	{
		// Act
		var command = new LoginContaCorrenteCommand();

		// Assert
		Assert.Null(command.NumeroConta);
		Assert.Null(command.Cpf);
		Assert.Equal(string.Empty, command.Senha);
	}
}
