using ContaCorrente.Infrastructure.Configuration;

namespace ContaCorrente.API;

public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		// Add services to the container.

		// adiciona a camada de infraestrutura
		builder.Services.AddInfraestructure(builder.Configuration);

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

		app.UseAuthorization();


		app.MapControllers();

		app.Run();
	}
}