using Core.Entities;
using Core.Repository;
using Core.Services;

namespace Fiap_Tech_Challenge_Fase1.Services
{
    public class Service<T> : IService<T> where T : EntityBase
    {
        private readonly IRepository<T> _repository;
        public Service(IRepository<T> repository) => _repository = repository;
        public void Alterar(T entidade)
        {
            if (entidade.Id == null)
            {
                throw new Exception("Sem id enviado");
            }
            _repository.Alterar(entidade);
        }

        public void Cadastrar(T entidade)
        {
            _repository.Cadastrar(entidade);
        }

        public void Deletar(int id)
        {
            _repository.Deletar(id);
        }

        public T ObterPorId(int id)
        {
            return _repository.ObterPorId(id);
        }

        public IList<T> ObterTodos()
        {
            return _repository.ObterTodos();
        }
    }
}
