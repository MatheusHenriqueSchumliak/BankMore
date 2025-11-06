using ContaCorrente.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using ContaCorrente.Infrastructure.Context;
using ContaCorrente.Infrastructure.Dapper;
using Microsoft.Extensions.Configuration;
using ContaCorrente.Domain.Interfaces;
using ContaCorrente.Domain.Enums;
using Dapper;

namespace ContaCorrente.Infrastructure.Configuration;

public static class DependencyInjection
{
	public static IServiceCollection AddInfraestructure(this IServiceCollection services, IConfiguration configuration)
	{
		//Pega a connection string
		var connectionString = configuration.GetConnectionString("DefaultConnection")
			?? throw new InvalidOperationException("Connection string 'DefaultConnection' não encontrada");

		//Inicializa o banco de dados (cria pasta, arquivo .db e tabelas)
		DbInitializer.Initialize(connectionString);

		//Registrando um TypeHandler para mapear o enum TipoMovimento no Dapper.
		SqlMapper.AddTypeHandler(typeof(TipoMovimento), new TipoMovimentoTypeHandler());

		//Registra o DbContext
		services.AddScoped<DbConnectionFactory>(_ => new DbConnectionFactory(connectionString));

		//Registra repositórios
		services.AddScoped<IContaCorrenteRepository, ContaCorrenteRepository>();
		services.AddScoped<IMovimentoRepository, MovimentoRepository>();

		return services;
	}
}