using ContaCorrente.Application.Commands;

namespace ContaCorrente.Test.Commands;

public class CriarContaCorrenteCommandTests
{
	[Fact]
	public void Deve_Criar_Command_Com_Valores_Corretos()
	{
		// Arrange
		var cpf = "123.456.789-00";
		var nome = "Matheus";
		var senha = "senha123";

		// Act
		var command = new CriarContaCorrenteCommand
		{
			Cpf = cpf,
			Nome = nome,
			Senha = senha
		};

		// Assert
		Assert.Equal(cpf, command.Cpf);
		Assert.Equal(nome, command.Nome);
		Assert.Equal(senha, command.Senha);
	}

	[Fact]
	public void Deve_Criar_Command_Com_Valores_Default()
	{
		// Act
		var command = new CriarContaCorrenteCommand();

		// Assert
		Assert.Equal(string.Empty, command.Cpf);
		Assert.Equal(string.Empty, command.Nome);
		Assert.Equal(string.Empty, command.Senha);
	}

}
