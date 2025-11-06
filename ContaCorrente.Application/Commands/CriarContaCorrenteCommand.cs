using MediatR;

namespace ContaCorrente.Application.Commands;

public class CriarContaCorrenteCommand : IRequest<CriarContaCorrenteResult>
{
	public string Cpf { get; set; } = string.Empty;// recebido no endpoint, validar localmente
	public string Senha { get; set; } = string.Empty;
	public string Nome { get; set; } = string.Empty;
}

public class CriarContaCorrenteResult
{
	public string NumeroConta { get; set; } = string.Empty;
}
