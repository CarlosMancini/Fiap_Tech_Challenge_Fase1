using Core.Entities;

namespace Core.Services
{
    public interface IContatoService : IService<Contato>
    {
        Task Cadastrar(Contato entidade);

        IList<Contato> ObterPorRegião(int RegiaoId);
    }
}
