using ContaCorrente.Infrastructure.Context;
using ContaCorrente.Domain.Interfaces;
using ContaCorrente.Domain.Entities;
using ContaCorrente.Domain.Enums;
using Dapper;

namespace ContaCorrente.Infrastructure.Repositories;

public class MovimentoRepository : IMovimentoRepository
{
	#region Construtor
	private readonly DbConnectionFactory _connectionString;
	public MovimentoRepository(DbConnectionFactory connectionString)
	{
		_connectionString = connectionString;
	}
	#endregion

	public async Task Adicionar(Movimento movimento)
	{
		const string query = @"
			INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor)
			VALUES (@Id, @ContaId, @DataMovimento, @TipoMovimento, @Valor);
		";

		using var conexao = _connectionString.CreateConnection();
		await conexao.ExecuteAsync(query, new
		{
			Id = movimento.Id.ToString(),
			ContaId = movimento.ContaCorrenteId.ToString(),
			DataMovimento = movimento.DataMovimento.ToString("o"),
			TipoMovimento = movimento.TipoMovimento.ToChar().ToString(),
			Valor = movimento.Valor
		});
	}

	public async Task<decimal> BuscarPorConta(Guid contaCorrenteId)
	{
		const string query = @"
			SELECT idmovimento		AS Id,
				   idcontacorrente	AS ContaCorrenteId,
				   datamovimento    AS DataMovimento,
				   tipomovimento    AS TipoMovimento,
				   valor            AS Valor
			FROM movimento
			WHERE idcontacorrente = @ContaCorrenteId;
		";

		using var conexao = _connectionString.CreateConnection();
		var saldo = await conexao.QuerySingleOrDefaultAsync<decimal>(query, new { ContaCorrenteId = contaCorrenteId.ToString() });
		return saldo;
	}

}