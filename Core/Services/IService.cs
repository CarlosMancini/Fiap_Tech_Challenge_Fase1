using Core.Entities;

namespace Core.Services
{
    public interface IService<T> where T : EntityBase
    {
            IList<T> ObterTodos();
            T ObterPorId(int id);
            void Cadastrar(T entidade);
            void Alterar(T entidade);
            void Deletar(int id);
    }
}
