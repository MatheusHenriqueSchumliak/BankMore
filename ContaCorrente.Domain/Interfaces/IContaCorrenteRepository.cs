namespace ContaCorrente.Domain.Interfaces;

public interface IContaCorrenteRepository
{
	Task Criar(Entities.ContaCorrente contaCorrente);
	Task Atualizar(Entities.ContaCorrente contaCorrente);

	Task<Entities.ContaCorrente?> BuscarPorId(string id);
	Task<Entities.ContaCorrente?> BuscarPorCpf(string cpf);
	Task<Entities.ContaCorrente?> BuscarPorNumero(int numero);

	Task<bool> VerificaSeNumeroExiste(int numero);
}