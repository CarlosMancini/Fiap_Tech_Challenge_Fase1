using Core.Entities;
using Core.Interfaces.Repository;
using Core.Interfaces.Services;

namespace ConsultaContato.Services
{
    public class ContatoService : Service<Contato>, IContatoService
    {
        private readonly IContatoRepository _contatoRepository;

        public ContatoService(IContatoRepository contatoRepository) : base(contatoRepository)
        {
            _contatoRepository = contatoRepository;
        }

        public IList<Contato> ObterPorRegiao(int regiaoId)
        {
            return _contatoRepository.ObterPorRegiao(regiaoId);
        }
    }
}
