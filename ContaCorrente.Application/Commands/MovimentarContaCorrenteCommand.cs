using MediatR;

namespace ContaCorrente.Application.Commands;

public class MovimentarContaCorrenteCommand : IRequest<MovimentarContaCorrenteResult>
{
	public string? IdRequisicao { get; set; }
	public int? NumeroConta { get; set; }
	public decimal Valor { get; set; }
	public string Tipo { get; set; } = string.Empty; // "C" ou "D"
	public int NumeroContaUsuario { get; set; } // Preenchido pelo controller a partir do token
}

public class MovimentarContaCorrenteResult
{
	public bool Sucesso { get; set; }
	public string? Mensagem { get; set; }
	public string? Tipo { get; set; }
}
