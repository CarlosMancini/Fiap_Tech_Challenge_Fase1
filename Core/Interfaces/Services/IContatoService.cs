using Core.Entities;

namespace Core.Interfaces.Services
{
    public interface IContatoService : IService<Contato>
    {
        Task Cadastrar(Contato entidade);
        Task Alterar(Contato entidade);

        IList<Contato> ObterPorRegiao(int RegiaoId);
    }
}
