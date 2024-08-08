using Core.Entities;

namespace Core.Interfaces.Repository
{
    public interface IRepository<T> where T : EntityBase
    {
        Task Cadastrar(T entidade);
        Task Alterar(T entidade);
        Task Deletar(int id);
        Task<T> ObterPorId(int id);
        Task<IList<T>> ObterTodos();
    }
}
