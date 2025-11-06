using ContaCorrente.Domain.Enums;

namespace ContaCorrente.Domain.Entities;

public class Movimento
{
	public Guid Id { get; set; } = Guid.NewGuid();
	public Guid ContaCorrenteId { get; set; }
	public DateTime DataMovimento { get; set; } = DateTime.UtcNow;
	public TipoMovimento TipoMovimento { get; set; } 
	public decimal Valor { get; set; }

	protected Movimento() { }

	public Movimento(Guid id, Guid contaCorrenteId, DateTime dataMovimento, TipoMovimento tipoMovimento, decimal valor)
	{
		this.Id = id;
		this.ContaCorrenteId = contaCorrenteId;
		this.DataMovimento = dataMovimento;
		this.TipoMovimento = tipoMovimento;
		this.Valor = valor;
	}
}