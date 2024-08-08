using Core.Entities;

namespace Core.Interfaces.Repository
{
    public interface IRegiaoRepository : IRepository<Regiao>
    {
        Task<IList<Regiao>> ObterTodos();
    }
}
