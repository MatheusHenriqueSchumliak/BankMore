using MediatR;

namespace ContaCorrente.Application.Commands;

public class InativarContaCorrenteCommand : IRequest<InativarContaCorrenteResult>
{
	public int NumeroConta { get; set; }
	public string Senha { get; set; } = string.Empty;
}
public class InativarContaCorrenteResult
{
	public bool Sucesso { get; set; }
	public string? Mensagem { get; set; }
	public string? Tipo { get; set; }
}