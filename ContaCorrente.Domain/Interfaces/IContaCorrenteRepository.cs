namespace ContaCorrente.Domain.Interfaces;

public interface IContaCorrenteRepository
{
	Task Criar(Entities.ContaCorrente contaCorrente);
	Task Atualizar(Entities.ContaCorrente contaCorrente);

	// Retornos anuláveis para representar "não encontrado"
	Task<Entities.ContaCorrente?> BuscarPorId(Guid id);
	Task<Entities.ContaCorrente?> BuscarPorCpf(string cpf);
	Task<Entities.ContaCorrente?> BuscarPorNumero(int numero);

	Task<bool> VerificaSeNumeroExiste(int numero);
}