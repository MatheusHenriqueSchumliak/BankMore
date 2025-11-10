namespace ContaCorrente.API.Configuration;

public static class SwaggerConfig
{
	public static IServiceCollection AdicionaConfiguracaoSwagger(this IServiceCollection services)
	{

		//Configurações do Swagger adiciona e configura o serviço do Swagger para gerar a documentação da API.
		services.AddSwaggerGen(c =>
		{
			c.SwaggerDoc("v1", new() { Title = "ContaCorrente API", Version = "v1" });
			// Cria a documentação da API com o título e a versão especificados (neste caso, "ContaCorrente API" versão "v1").

			// Configuração para JWT Bearer
			c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme  // Define o esquema de segurança do tipo "Bearer" para autenticação JWT.
			{
				Description = "JWT Authorization header usando o esquema Bearer. Exemplo: 'Bearer {token}'",
				// Descrição que aparece na interface do Swagger, explicando como enviar o token JWT no cabeçalho da requisição.

				Name = "Authorization",                         // Nome do cabeçalho HTTP que o Swagger usará (no caso, "Authorization").
				In = Microsoft.OpenApi.Models.ParameterLocation.Header,  // Indica que o token será passado no cabeçalho da requisição.
				Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http, // Especifica que o tipo de segurança é baseado em HTTP (não em API key ou OAuth, por exemplo).
				Scheme = "bearer",                               // Define o esquema de autenticação como "bearer".
				BearerFormat = "JWT"                             // Especifica o formato do token usado, que é JWT (JSON Web Token).
			});

			c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement  // Define o requisito de segurança para a API.
			{
				{
					new Microsoft.OpenApi.Models.OpenApiSecurityScheme  // Indica que a API requer o esquema "Bearer" definido acima.
					{
						Reference = new Microsoft.OpenApi.Models.OpenApiReference  // Cria uma referência ao esquema de segurança já definido.
						{
							Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,  // Especifica que a referência é de um tipo de esquema de segurança.
							Id = "Bearer"                                                  // Define o ID do esquema, que deve corresponder ao nome dado antes ("Bearer").
						}
					},
					new string[] { }  // Lista de escopos de segurança (vazia neste caso, pois JWT não usa escopos).
				}
			});
		});

		return services;
	}
}
