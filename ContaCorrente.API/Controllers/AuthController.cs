using ContaCorrente.Application.Commands;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace ContaCorrente.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
	#region Construtor
	private readonly IMediator _mediator;

	public AuthController(IMediator mediator)
	{
		_mediator = mediator;
	}
	#endregion

	[HttpPost("login")]
	public async Task<IActionResult> Login([FromBody] LoginContaCorrenteCommand command)
	{
		var result = await _mediator.Send(command);

		if (!result.Sucesso)
		{
			return Unauthorized(new
			{
				mensagem = result.Mensagem,
				tipo = result.Tipo
			});
		}

		return Ok(new { token = result.Token });
	}
}
