using ContaCorrente.Infrastructure.Security;
using ContaCorrente.Application.Commands;
using ContaCorrente.Domain.Interfaces;
using System.Security.Cryptography;
using MediatR;


namespace ContaCorrente.Application.Handlers;

// Handler responsável por processar o comando de criação de conta corrente
public class CriarContaCorrenteHandler : IRequestHandler<CriarContaCorrenteCommand, CriarContaCorrenteResult>
{
	// Repositório de conta corrente para persistência
	private readonly IContaCorrenteRepository _contaCorrenteRepository;
	// Serviço para geração de tokens JWT
	private readonly GeradorTokenJwt _geradorTokenJwt;

	// Construtor recebe o repositório via injeção de dependência
	public CriarContaCorrenteHandler(IContaCorrenteRepository contaCorrenteRepository, GeradorTokenJwt geradorTokenJwt)
	{
		_contaCorrenteRepository = contaCorrenteRepository;
		_geradorTokenJwt = geradorTokenJwt;
	}

	// Método principal que executa a lógica de criação da conta
	public async Task<CriarContaCorrenteResult> Handle(CriarContaCorrenteCommand request, CancellationToken cancellationToken)
	{
		// Não valida o CPF aqui, pois a validação já é feita no controller para garantir resposta HTTP adequada

		// Simula criação de usuário (em produção, integraria com serviço de usuários)
		var userId = Guid.NewGuid();

		// Gera um número de conta aleatório e garante unicidade
		int numeroConta;
		var tentativas = 0;
		do
		{
			numeroConta = RandomNumberGenerator.GetInt32(1_000_000, 9_999_999); // Gera número entre 1_000_000 e 9_999_999
			tentativas++;
			if (tentativas >= 20) break; // Fallback após muitas tentativas
		}
		while (await _contaCorrenteRepository.VerificaSeNumeroExiste(numeroConta));

		// Fallback determinístico se as tentativas aleatórias falharem
		if (await _contaCorrenteRepository.VerificaSeNumeroExiste(numeroConta))
		{
			numeroConta = 1_000_000;
			while (await _contaCorrenteRepository.VerificaSeNumeroExiste(numeroConta))
			{
				numeroConta++;
			}
		}

		// Gera hash e salt da senha para segurança
		var (hash, salt) = HashDeSenha.CriarHash(request.Senha);

		// Cria a entidade ContaCorrente com os dados recebidos e os gerados
		var conta = new Domain.Entities.ContaCorrente(
			Guid.NewGuid(),               // id da conta
			numeroConta,                  // numero
			request.Nome ?? string.Empty, // nome
			request.Cpf,                  // cpf
			true,                         // ativo
			hash,                         // senha (hash)
			salt                          // salt
		);

		// Persiste a conta no banco de dados
		await _contaCorrenteRepository.Criar(conta);

		// Gera token JWT para a nova conta
		var token = _geradorTokenJwt.GerarToken(conta.Id, conta.Numero, conta.Cpf);

		// Retorna o número da conta criada
		return new CriarContaCorrenteResult
		{
			NumeroConta = numeroConta.ToString(),
			Token = token
		};
	}
}
