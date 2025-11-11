using ContaCorrente.Application.Commands;

namespace ContaCorrente.API.Configuration;

public static class MediatRConfig
{
	public static IServiceCollection AdicionaConfiguracaoMediatR(this IServiceCollection services)
	{
		// Registra todos os comandos/handlers do assembly de comandos
		services.AddMediatR(cfg =>
		{
			cfg.RegisterServicesFromAssemblyContaining<CriarContaCorrenteCommand>();
			cfg.RegisterServicesFromAssemblyContaining<MovimentarContaCorrenteCommand>();
			cfg.RegisterServicesFromAssemblyContaining<LoginContaCorrenteCommand>();
			// outros comandos/handlers conforme necessário
		});

		return services;
	}
}
