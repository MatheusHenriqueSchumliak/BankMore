using System.Security.Cryptography;

namespace ContaCorrente.Infrastructure.Security;

public static class HashDeSenha
{
	public static (string Hash, string Sal) CriarHash(string senha, int iteracoes = 10000)
	{
		if (senha is null) throw new ArgumentNullException(nameof(senha));

		var bytesSal = new byte[16];
		using var rng = RandomNumberGenerator.Create();
		rng.GetBytes(bytesSal);
		var sal = Convert.ToBase64String(bytesSal);

		var hash = GerarHashDaSenha(senha, sal, iteracoes);
		return (hash, sal);
	}

	public static bool Verificar(string senha, string hash, string sal, int iteracoes = 10000)
	{
		if (senha is null) throw new ArgumentNullException(nameof(senha));
		if (hash is null) throw new ArgumentNullException(nameof(hash));
		if (sal is null) throw new ArgumentNullException(nameof(sal));

		var candidato = GerarHashDaSenha(senha, sal, iteracoes);

		return CryptographicOperations.FixedTimeEquals(Convert.FromBase64String(candidato), Convert.FromBase64String(hash));
	}

	private static string GerarHashDaSenha(string senha, string sal, int iteracoes)
	{
		var bytesSal = Convert.FromBase64String(sal);
		
		using var pbkdf2 = new Rfc2898DeriveBytes(senha, bytesSal, iteracoes, HashAlgorithmName.SHA256);
		var bytesHash = pbkdf2.GetBytes(32);
		
		return Convert.ToBase64String(bytesHash);
	}
}
