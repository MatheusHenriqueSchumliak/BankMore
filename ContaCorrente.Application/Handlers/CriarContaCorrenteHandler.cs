using ContaCorrente.Application.Commands;
using ContaCorrente.Domain.Interfaces;
using ContaCorrente.Domain.ValueObjects;
using ContaCorrente.Infrastructure.Security;
using MediatR;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace ContaCorrente.Application.Handlers;

public class CriarContaCorrenteHandler : IRequestHandler<CriarContaCorrenteCommand, CriarContaCorrenteResult>
{
	#region Construtor
	private readonly IContaCorrenteRepository _contaCorrenteRepository;
	public CriarContaCorrenteHandler(IContaCorrenteRepository contaCorrenteRepository)
	{
		_contaCorrenteRepository = contaCorrenteRepository;
	}
	#endregion

	public async Task<CriarContaCorrenteResult> Handle(CriarContaCorrenteCommand request, CancellationToken cancellationToken)
	{
		// Validação CPF método criado
		if (ValidadorDeCpf.EhValido(request.Cpf))
		{
			/*throw new InvalidDocumentException("CPF inválido", "INVALID_DOCUMENT");*/
			// Estudar como será melhor forma de lançar as exceções.
		}

		// Chamar Users service para criar user e obter userId
		// Aqui: simular criando GUID. Em produção, chamar service Users (não enviar CPF para outros microsserviços).
		var userId = Guid.NewGuid();

		// Gerar numero da conta (exemplo simples)
		int numeroConta;
		var tentativas = 0;
		do
		{
			numeroConta = RandomNumberGenerator.GetInt32(1_000_000, 9_999_999); // entre 1_000_000 (inclusive) e 9_999_999 (exclusive)
			tentativas++;
			if (tentativas >= 20) break; // fallback após muitas tentativas
		}
		while (await _contaCorrenteRepository.VerificaSeNumeroExiste(numeroConta));

		// fallback determinístico se as tentativas aleatórias falharem
		if (await _contaCorrenteRepository.VerificaSeNumeroExiste(numeroConta))
		{
			numeroConta = 1_000_000;
			while (await _contaCorrenteRepository.VerificaSeNumeroExiste(numeroConta))
			{
				numeroConta++;
			}
		}

		// Gerar hash e salt da senha
		var (hash, salt) = HashDeSenha.CriarHash(request.Senha);

		// Criar entidade corretamente (ordem de parâmetros conforme o construtor atual)
		var conta = new Domain.Entities.ContaCorrente(
			Guid.NewGuid(),               // id da conta
			numeroConta,                  // numero
			request.Nome ?? string.Empty, // nome
			request.Cpf,                  // cpf
			true,                         // ativo
			hash,                         // senha (hash)
			salt                          // salt
		);

		await _contaCorrenteRepository.Criar(conta);

		return new CriarContaCorrenteResult { NumeroConta = numeroConta.ToString() };
	}
}
