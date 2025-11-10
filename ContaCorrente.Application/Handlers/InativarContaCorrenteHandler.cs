using ContaCorrente.Infrastructure.Security;
using ContaCorrente.Application.Commands;
using ContaCorrente.Domain.Interfaces;
using MediatR;

namespace ContaCorrente.Application.Handlers;

public class InativarContaCorrenteHandler : IRequestHandler<InativarContaCorrenteCommand, InativarContaCorrenteResult>
{
	private readonly IContaCorrenteRepository _contaCorrenteRepository;
	public InativarContaCorrenteHandler(IContaCorrenteRepository contaCorrenteRepository)
	{
		_contaCorrenteRepository = contaCorrenteRepository;
	}

	public async Task<InativarContaCorrenteResult> Handle(InativarContaCorrenteCommand request, CancellationToken cancellationToken)
	{
		var conta = await _contaCorrenteRepository.BuscarPorNumero(request.NumeroConta);

		if (conta == null)
			return new()
			{
				Sucesso = false,
				Mensagem = "Conta corrente não encontrada.",
				Tipo = "INVALID_ACCOUNT"
			};

		if (!HashDeSenha.Verificar(request.Senha, conta.Senha, conta.Salt))
			return new()
			{
				Sucesso = false,
				Mensagem = "Senha inválida.",
				Tipo = "USER_UNAUTHORIZED"
			};

		conta.Ativo = false;
		await _contaCorrenteRepository.Atualizar(conta);

		return new() { Sucesso = true };
	}
}