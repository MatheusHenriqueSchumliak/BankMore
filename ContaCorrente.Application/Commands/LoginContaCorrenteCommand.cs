using MediatR;

namespace ContaCorrente.Application.Commands;

public class LoginContaCorrenteCommand : IRequest<LoginContaCorrenteResult>
{
	public string? NumeroConta { get; set; } // pode ser null se for login por CPF
	public string? Cpf { get; set; } // pode ser null se for login por número
	public string Senha { get; set; } = string.Empty;
}

public class LoginContaCorrenteResult
{
	public bool Sucesso { get; set; }
	public string? Token { get; set; }
	public string? Mensagem { get; set; }
	public string? Tipo { get; set; }
}
