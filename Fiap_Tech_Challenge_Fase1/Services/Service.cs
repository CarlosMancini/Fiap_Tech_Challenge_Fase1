using Core.Entities;
using Core.Interfaces.Repository;
using Core.Interfaces.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fiap_Tech_Challenge_Fase1.Services
{
    public class Service<T> : IService<T> where T : EntityBase
    {
        private readonly IRepository<T> _repository;

        public Service(IRepository<T> repository)
        {
            _repository = repository;
        }

        public async Task Alterar(T entidade)
        {
            if (entidade.Id == 0)
            {
                throw new Exception("Sem id enviado");
            }
            await _repository.Alterar(entidade);
        }

        public async Task Cadastrar(T entidade)
        {
            await _repository.Cadastrar(entidade);
        }

        public async Task Deletar(int id)
        {
            await _repository.Deletar(id);
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
