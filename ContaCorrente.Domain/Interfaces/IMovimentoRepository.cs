using ContaCorrente.Domain.Entities;

namespace ContaCorrente.Domain.Interfaces;

public interface IMovimentoRepository
{
	Task Adicionar(Movimento movimento);
	Task<decimal> BuscarPorConta(Guid contaCorrenteId);
}