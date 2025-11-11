using ContaCorrente.Infrastructure.Security;
using ContaCorrente.Application.Commands;
using ContaCorrente.Domain.ValueObjects;
using ContaCorrente.Domain.Interfaces;
using MediatR;

namespace ContaCorrente.Application.Handlers;

public class LoginContaCorrenteHandler : IRequestHandler<LoginContaCorrenteCommand, LoginContaCorrenteResult>
{
	private readonly IContaCorrenteRepository _contaRepo;
	private readonly IGeradorTokenJwt _jwt;

	public LoginContaCorrenteHandler(IContaCorrenteRepository contaRepo, IGeradorTokenJwt jwt)
	{
		_contaRepo = contaRepo;
		_jwt = jwt;
	}

	public async Task<LoginContaCorrenteResult> Handle(LoginContaCorrenteCommand request, CancellationToken cancellationToken)
	{
		Domain.Entities.ContaCorrente? conta = null;

		if (!string.IsNullOrWhiteSpace(request.NumeroConta) && int.TryParse(request.NumeroConta, out var numero))
			conta = await _contaRepo.BuscarPorNumero(numero);
		else if (!string.IsNullOrWhiteSpace(request.Cpf))
			conta = await _contaRepo.BuscarPorCpf(request.Cpf);

		if (conta == null)
		{
			return new LoginContaCorrenteResult
			{
				Sucesso = false,
				Mensagem = "Conta ou CPF não encontrado.",
				Tipo = "USER_UNAUTHORIZED"
			};
		}

		if (!HashDeSenha.Verificar(request.Senha, conta.Senha, conta.Salt))
		{
			return new LoginContaCorrenteResult
			{
				Sucesso = false,
				Mensagem = "Senha inválida.",
				Tipo = "USER_UNAUTHORIZED"
			};
		}

		var token = _jwt.GerarToken(conta.Id, conta.Numero, conta.Cpf);

		return new LoginContaCorrenteResult
		{
			Sucesso = true,
			Token = token
		};
	}
}