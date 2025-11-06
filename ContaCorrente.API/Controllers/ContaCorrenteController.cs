using ContaCorrente.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ContaCorrente.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ContaCorrenteController : ControllerBase
{
	#region Construtor
	private readonly IContaCorrenteRepository _contaCorrenteRepository;
	public ContaCorrenteController(IContaCorrenteRepository contaCorrenteRepository)
	{
		_contaCorrenteRepository = contaCorrenteRepository;
	}
	#endregion
}
