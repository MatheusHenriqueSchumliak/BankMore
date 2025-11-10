using ContaCorrente.API.Configuration;
using ContaCorrente.Application.Commands;
using ContaCorrente.Infrastructure.Configuration;
using ContaCorrente.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ContaCorrente.API;

public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		// Add services to the container.

		// adiciona a camada de infraestrutura
		builder.Services.AddInfraestructure(builder.Configuration);

		// Adiciona o MediatR (registre os comandos/handlers do seu projeto)
		builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CriarContaCorrenteCommand>());

		//Configurações do JWT		
		builder.Services.AdiconaAutenticacaoJwt(builder.Configuration);

		//Configuração do Swagger
		builder.Services.AdicionaConfiguracaoSwagger();

		builder.Services.AddControllers();
		// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen();

		var app = builder.Build();

		// Configure the HTTP request pipeline.
		if (app.Environment.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI();
		}

		app.UseHttpsRedirection();

		app.UseAuthentication();
		app.UseAuthorization();


		app.MapControllers();

		app.Run();
	}
}