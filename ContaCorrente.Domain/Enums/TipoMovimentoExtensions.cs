namespace ContaCorrente.Domain.Enums;

public static class TipoMovimentoExtensions
{
	public static char ToChar(this TipoMovimento tipo) => (char)(int)tipo;

	public static TipoMovimento FromChar(char c) => c switch
	{
		'C' => TipoMovimento.Credito,
		'D' => TipoMovimento.Debito,
		_ => throw new ArgumentException($"Tipo de movimento inválido: {c}", nameof(c))
	};

	public static bool TryFromChar(char c, out TipoMovimento tipo)
	{
		if (c == 'C') { tipo = TipoMovimento.Credito; return true; }
		if (c == 'D') { tipo = TipoMovimento.Debito; return true; }
		tipo = default;
		return false;
	}
}
