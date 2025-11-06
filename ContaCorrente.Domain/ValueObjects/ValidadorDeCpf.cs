using System.Text.RegularExpressions;

namespace ContaCorrente.Domain.ValueObjects;

public static class ValidadorDeCpf
{
	public static bool EhValido(string? cpf)
	{
		if (string.IsNullOrWhiteSpace(cpf)) return false;

		var apenasDigitos = Regex.Replace(cpf, @"\D", "");
		if (apenasDigitos.Length != 11) return false;
		if (apenasDigitos.Distinct().Count() == 1) return false;

		int[] digitos = apenasDigitos.Select(c => c - '0').ToArray();

		if (!ValidarDigitoVerificador(digitos, 9, 10)) return false;
		if (!ValidarDigitoVerificador(digitos, 10, 11)) return false;

		return true;
	}

	private static bool ValidarDigitoVerificador(int[] digitos, int tamanho, int pesoInicial)
	{
		int soma = 0;
		for (int i = 0; i < tamanho; i++)
			soma += digitos[i] * (pesoInicial - i);

		int resto = (soma * 10) % 11;
		if (resto == 10) resto = 0;

		return resto == digitos[tamanho];
	}

}
