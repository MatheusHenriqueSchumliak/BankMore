using ContaCorrente.Application.Commands;
using ContaCorrente.Domain.Interfaces;
using ContaCorrente.Domain.Entities;
using ContaCorrente.Domain.Enums;
using MediatR;

namespace ContaCorrente.Application.Handlers;

public class MovimentarContaCorrenteHandler : IRequestHandler<MovimentarContaCorrenteCommand, MovimentarContaCorrenteResult>
{
	private readonly IContaCorrenteRepository _contaRepo;
	private readonly IMovimentoRepository _movimentoRepo;

	public MovimentarContaCorrenteHandler(IContaCorrenteRepository contaRepo, IMovimentoRepository movimentoRepo)
	{
		_contaRepo = contaRepo;
		_movimentoRepo = movimentoRepo;
	}

	public async Task<MovimentarContaCorrenteResult> Handle(MovimentarContaCorrenteCommand request, CancellationToken cancellationToken)
	{
		var numeroConta = request.NumeroConta ?? request.NumeroContaUsuario;

		var conta = await _contaRepo.BuscarPorNumero(numeroConta);
		if (conta == null)
			return new() { Sucesso = false, Mensagem = "Conta corrente não encontrada.", Tipo = "INVALID_ACCOUNT" };

		if (!conta.Ativo)
			return new() { Sucesso = false, Mensagem = "Conta corrente inativa.", Tipo = "INACTIVE_ACCOUNT" };

		if (request.Valor <= 0)
			return new() { Sucesso = false, Mensagem = "Valor deve ser positivo.", Tipo = "INVALID_VALUE" };

		if (request.Tipo != "C" && request.Tipo != "D")
			return new() { Sucesso = false, Mensagem = "Tipo de movimento inválido.", Tipo = "INVALID_TYPE" };

		if (request.NumeroConta.HasValue && request.NumeroConta.Value != request.NumeroContaUsuario && request.Tipo != "C")
			return new() { Sucesso = false, Mensagem = "Apenas crédito permitido para contas de terceiros.", Tipo = "INVALID_TYPE" };

		var movimento = new Movimento(
			Guid.NewGuid(),
			conta.Id,
			DateTime.UtcNow,
			request.Tipo == "C" ? TipoMovimento.Credito : TipoMovimento.Debito,
			request.Valor
		);
		await _movimentoRepo.Adicionar(movimento);

		return new() { Sucesso = true };
	}
}