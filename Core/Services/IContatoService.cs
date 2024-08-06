using Core.Entities;

namespace Core.Services
{
    public interface IContatoService : IService<Contato>
    {
        IList<Contato> ObterPorRegião(int RegiaoId);
    }
}
