using Core.Interfaces.Repository;
using Core.Interfaces.Services;

namespace Exclusao.Services
{
    public class ExclusaoService : IExclusaoService
    {
        private readonly IContatoRepository _contatoRepository;

        public ExclusaoService(IContatoRepository contatoRepository)
        {
            _contatoRepository = contatoRepository;
        }

        public async Task Excluir(int id)
        {
            await _contatoRepository.Excluir(id);
        }
    }
}
