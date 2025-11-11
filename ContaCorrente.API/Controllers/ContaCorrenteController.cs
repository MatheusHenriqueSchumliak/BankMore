using ContaCorrente.Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using ContaCorrente.Application.Commands;
using ContaCorrente.Domain.ValueObjects;
using ContaCorrente.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace ContaCorrente.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ContaCorrenteController : ControllerBase
{
	#region Construtor
	// Repositórios e serviços necessários
	private readonly IContaCorrenteRepository _contaCorrenteRepository;
	private readonly IMovimentoRepository _movimentoRepository;
	private readonly IGeradorTokenJwt _geradorTokenJwt;
	private readonly IMediator _mediator;

	// Construtor recebe dependências via injeção de dependência
	public ContaCorrenteController(
		IContaCorrenteRepository contaCorrenteRepository,
		IMovimentoRepository movimentoRepository,
		IGeradorTokenJwt geradorTokenJwt,
		IMediator mediator)
	{
		_contaCorrenteRepository = contaCorrenteRepository;
		_movimentoRepository = movimentoRepository;
		_geradorTokenJwt = geradorTokenJwt;
		_mediator = mediator;

	}
	#endregion

	/// <summary>
	/// Endpoint para cadastrar uma nova conta corrente.
	/// </summary>
	/// <param name="command">Dados para criação da conta corrente (CPF, senha, nome).</param>
	/// <returns>Número da conta criada ou erro de validação.</returns>
	[HttpPost]
	public async Task<IActionResult> CriarContaCorrente([FromBody] CriarContaCorrenteCommand command)
	{
		// Valida o CPF antes de enviar para o handler, para garantir resposta HTTP 400 adequada
		if (!ValidadorDeCpf.EhValido(command.Cpf))
		{
			// Retorna HTTP 400 com mensagem e tipo de falha conforme especificação
			return BadRequest(new
			{
				mensagem = "CPF inválido.",
				tipo = "INVALID_DOCUMENT"
			});
		}

		// Envia o comando para o handler via MediatR
		var resultado = await _mediator.Send(command);

		// Retorna HTTP 200 com o número da conta criada
		return Ok(resultado);
	}

	/// <summary>
	/// Endpoint para inativar a conta corrente do usuário autenticado.
	/// </summary>
	/// <param name="request">Senha da conta corrente.</param>
	[Authorize]
	[HttpPut("inativar")]
	public async Task<IActionResult> InativarContaCorrente([FromBody] InativarContaCorrenteCommand request)
	{
		// [IMPORTANTE] Aqui deveria ser feita a validação do token JWT.
		var numeroContaClaim = User.Claims.FirstOrDefault(c => c.Type == "account_number")?.Value;

		// Se token inválido/expirado, retornar 403:
		if (string.IsNullOrEmpty(numeroContaClaim) || !int.TryParse(numeroContaClaim, out var numeroConta))
		{
			return Forbid(); // Token inválido ou sem claim
		}

		// Passa o número da conta pelo command
		request.NumeroConta = numeroConta;

		var resultado = await _mediator.Send(request);

		if (!resultado.Sucesso)
		{
			if (resultado.Tipo == "INVALID_ACCOUNT")
				return BadRequest(new { mensagem = resultado.Mensagem, tipo = resultado.Tipo });
			if (resultado.Tipo == "USER_UNAUTHORIZED")
				return Unauthorized(new { mensagem = resultado.Mensagem, tipo = resultado.Tipo });
			return Forbid();
		}

		// Retorna HTTP 204 (No Content) em caso de sucesso
		return NoContent();
	}

	/// <summary>
	/// Endpoint para consultar o saldo da conta corrente do usuário autenticado.
	/// </summary>
	/// <returns>Informações da conta e saldo atual.</returns>
	[HttpGet("saldo")]
	public async Task<IActionResult> ConsultarSaldo()
	{
		// [IMPORTANTE] Aqui deveria ser feita a validação do token JWT.
		// Exemplo: var userId = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
		// Se token inválido/expirado, retornar 403:
		// return Forbid();

		// Para exemplo, simular um número de conta (em produção, obter do token)
		int numeroConta = 1234567; // Substitua por extração real do token

		// Busca a conta corrente pelo número
		var conta = await _contaCorrenteRepository.BuscarPorNumero(numeroConta);
		if (conta == null)
		{
			return BadRequest(new
			{
				mensagem = "Conta corrente não encontrada.",
				tipo = "INVALID_ACCOUNT"
			});
		}
		if (!conta.Ativo)
		{
			return BadRequest(new
			{
				mensagem = "Conta corrente inativa.",
				tipo = "INACTIVE_ACCOUNT"
			});
		}

		// Calcula o saldo usando o repositório de movimentos
		// (Ideal: Buscar todos os movimentos e somar créditos - débitos)
		var saldo = await _movimentoRepository.BuscarPorConta(conta.Id);

		// Monta o objeto de resposta
		var resposta = new
		{
			numeroConta = conta.Numero,
			nomeTitular = conta.Nome,
			dataConsulta = DateTime.UtcNow,
			valorSaldo = saldo
		};

		return Ok(resposta);
	}

}
