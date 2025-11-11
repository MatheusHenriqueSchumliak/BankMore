using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace ContaCorrente.Infrastructure.Security;

public class GeradorTokenJwt : IGeradorTokenJwt
{
	private readonly int _jwtExpirationMinutes;
	private readonly string _jwtSecret;
	private readonly string _audience;
	private readonly string _issuer;

	public GeradorTokenJwt(IConfiguration configuration)
	{
		_jwtSecret = configuration["JwtSecret:Key"]!;
		_issuer = configuration["JwtSecret:Issuer"]!;
		_audience = configuration["JwtSecret:Audience"]!;
		_jwtExpirationMinutes = int.TryParse(configuration["JwtSecret:ExpirationMinutes"], out var exp) ? exp : 60;
	}

	public string GerarToken(Guid contaId, int numeroConta, string cpf)
	{
		var tokenHandler = new JwtSecurityTokenHandler();
		var key = Encoding.UTF8.GetBytes(_jwtSecret);

		var claims = new List<Claim>
		{
			new Claim(JwtRegisteredClaimNames.Sub, contaId.ToString()),
			new Claim("account_number", numeroConta.ToString()),
			new Claim("cpf", cpf)
		};

		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(claims),
			Expires = DateTime.UtcNow.AddMinutes(_jwtExpirationMinutes),
			SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
			Issuer = _issuer,
			Audience = _audience
		};

		var token = tokenHandler.CreateToken(tokenDescriptor);
		return tokenHandler.WriteToken(token);
	}
}