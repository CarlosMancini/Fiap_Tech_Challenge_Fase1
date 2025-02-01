using Core.Entities;

namespace Core.Interfaces.Services
{
    public interface IContatoService : IService<Contato>
    {
        IList<Contato> ObterPorRegiao(int RegiaoId);
    }
}
