using Core.Entities;

namespace Core.Interfaces.Services
{
    public interface IContatoService : IService<Contato>
    {
        Task Cadastrar(Contato entidade);
        Task Atualizar(Contato entidade);

        IList<Contato> ObterPorRegiao(int RegiaoId);
    }
}
