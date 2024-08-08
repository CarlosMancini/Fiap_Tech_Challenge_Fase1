using Core.Entities;

namespace Core.Interfaces.Services
{
    public interface IContatoService : IService<Contato>
    {
        Task Cadastrar(Contato entidade);

        IList<Contato> ObterPorRegiao(int RegiaoId);
    }
}
