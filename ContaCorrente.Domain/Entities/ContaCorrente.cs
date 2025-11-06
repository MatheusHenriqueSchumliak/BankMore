namespace ContaCorrente.Domain.Entities;

public class ContaCorrente
{
	public Guid Id { get; set; } = Guid.NewGuid();
	public int Numero { get; set; }
	public string Nome { get; set; } = string.Empty;
	public string Cpf { get; set; } = string.Empty;
	public bool Ativo { get; set; } = false;
	public string Senha { get; set; } = string.Empty;
	public string Salt { get; set; } = string.Empty;

	protected ContaCorrente() { }

	public ContaCorrente(Guid id, int numero, string nome, string cpf, bool ativo, string senha, string salt)
	{
		this.Id = id;
		this.Numero = numero;
		this.Nome = nome;
		this.Cpf = cpf;
		this.Ativo = ativo;
		this.Senha = senha;
		this.Salt = salt;
	}

	public void Inativar(string senha)
	{
		if (Senha != senha) throw new UnauthorizedAccessException("Senha inválida");
		Ativo = false;
	}
}
