using ContaCorrente.Application.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace ContaCorrente.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MovimentacaoController : ControllerBase
{
	#region Construtor
	private readonly IMediator _mediator;
	public MovimentacaoController(IMediator mediator)
	{
		_mediator = mediator;
	}
	#endregion

	[Authorize]
	[HttpPost("movimentar")]
	public async Task<IActionResult> MovimentarConta([FromBody] MovimentarContaCorrenteCommand command)
	{
		var numeroContaClaim = User.Claims.FirstOrDefault(c => c.Type == "account_number")?.Value;
		if (string.IsNullOrEmpty(numeroContaClaim) || !int.TryParse(numeroContaClaim, out var numeroContaUsuario))
			return Forbid();

		command.IdRequisicao = new Guid().ToString();
		command.NumeroContaUsuario = numeroContaUsuario;

		var resultado = await _mediator.Send(command);

		if (!resultado.Sucesso)
			return BadRequest(new { mensagem = resultado.Mensagem, tipo = resultado.Tipo });

		return NoContent();
	}

}
