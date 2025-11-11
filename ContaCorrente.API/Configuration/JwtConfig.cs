using Microsoft.AspNetCore.Authentication.JwtBearer;
using ContaCorrente.Infrastructure.Security;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ContaCorrente.API.Configuration;

public static class JwtConfig
{
	public static IServiceCollection AdiconaAutenticacaoJwt(this IServiceCollection services, IConfiguration configuration)
	{
		// Adiciona o serviço de geração de JWT
		var jwtSecret = configuration.GetSection("JwtSecret");
		var key = Encoding.UTF8.GetBytes(jwtSecret["key"]!);

		services.AddAuthentication(options =>              // Adiciona o serviço de autenticação ao container de injeção de dependência da aplicação.
		{
			options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;  // Define o esquema padrão de autenticação como JWT Bearer.
			options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;      // Define o esquema padrão de desafio (challenge) também como JWT Bearer.
		})
		.AddJwtBearer(options =>                                    // Configura a autenticação usando tokens JWT (JSON Web Token).
		{
			options.RequireHttpsMetadata = false;                    // Define se é obrigatório usar HTTPS para validar o token (false geralmente usado em desenvolvimento).
			options.SaveToken = true;                                // Indica que o token validado deve ser armazenado no contexto de autenticação.

			options.TokenValidationParameters = new TokenValidationParameters  // Define os parâmetros de validação do token JWT.
			{
				ValidateIssuer = true,                                // Indica se o emissor (issuer) do token deve ser validado.
				ValidIssuer = jwtSecret["Issuer"],                 // Define o emissor válido esperado (geralmente o nome ou chave da aplicação).

				ValidateIssuerSigningKey = true,                      // Indica se a chave de assinatura do emissor deve ser validada.
				IssuerSigningKey = new SymmetricSecurityKey(key),      // Define a chave simétrica usada para validar a assinatura do token (deve ser a mesma usada na geração do token).

				ValidateAudience = true,                              // Indica se o público (audience) do token deve ser validado.
				ValidAudience = jwtSecret["Audience"],               // Define o público válido esperado (geralmente o nome ou identificador da aplicação).

				ValidateLifetime = true,                              // Indica se o tempo de vida (expiração) do token deve ser verificado.
			};
		});

		// Registra o GeradorTokenJwt como um serviço singleton. Buscando a chave secreta do JWT nas configurações da aplicação.
		services.AddSingleton<IGeradorTokenJwt, GeradorTokenJwt>();

		return services;
	}

}
