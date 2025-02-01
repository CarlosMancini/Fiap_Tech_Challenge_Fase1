using Core.Entities;

namespace Core.Interfaces.Repository
{
    public interface IContatoRepository : IRepository<Contato>
    {
        IList<Contato> ObterPorRegiao(int RegiaoId);
        Task<Contato> ObterPorNomeETelefone(string nome, string telefone);
    }
}
