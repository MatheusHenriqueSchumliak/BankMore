namespace ContaCorrente.Infrastructure.Security;

public interface IGeradorTokenJwt
{
	string GerarToken(Guid contaId, int numeroConta, string cpf);
}
