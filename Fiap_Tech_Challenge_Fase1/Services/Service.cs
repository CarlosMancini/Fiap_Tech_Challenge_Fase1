using Core.Entities;
using Core.Interfaces.Repository;
using Core.Interfaces.Services;

namespace ConsultaContato.Services
{
    public class Service<T> : IService<T> where T : EntityBase
    {
        private readonly IRepository<T> _repository;

        public Service(IRepository<T> repository)
        {
            _repository = repository;
        }

        public async Task<T> ObterPorId(int id)
        {
            return await _repository.ObterPorId(id);
        }

        public async Task<IList<T>> ObterTodos()
        {
            return await _repository.ObterTodos();
        }
    }
}
