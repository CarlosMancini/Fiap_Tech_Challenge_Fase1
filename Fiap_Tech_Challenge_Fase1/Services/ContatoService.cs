using Core.Entities;
using Core.Gateways;
using Core.Interfaces.Repository;
using Core.Interfaces.Services;

namespace Fiap_Tech_Challenge_Fase1.Services
{
    public class ContatoService : Service<Contato>, IContatoService
    {
        private readonly IRegiaoRepository _regiaoRepository;
        private readonly IContatoRepository _contatoRepository;
        private readonly IBrasilGateway _brasilGateway;

        public ContatoService(IContatoRepository contatoRepository, IRegiaoRepository regiaoRepository, IBrasilGateway brasilGateway) : base(contatoRepository)
        {
            _regiaoRepository = regiaoRepository;
            _contatoRepository = contatoRepository;
            _brasilGateway = brasilGateway;
        }

        public async Task Cadastrar(Contato entidade)
        {
            int region = Int32.Parse(entidade.ContatoTelefone[..2]);
            var allRegions = await _regiaoRepository.ObterTodos();
            var selectedRegion = allRegions.FirstOrDefault(item => item.RegiaoDdd == region);

            if (selectedRegion is null)
            {
                var regiaoNome = await this._brasilGateway.BuscarDDDAsync(region);
                Regiao regiao = new()
                {
                    RegiaoNome = regiaoNome,
                    RegiaoDdd = region,
                };
                await _regiaoRepository.Cadastrar(regiao);
            }

            //entidade.RegiaoId = selectedRegion.Id;

            base.Cadastrar(entidade);
        }

        public IList<Contato> ObterPorRegiao(int RegiaoId)
        {
            return _contatoRepository.ObterPorRegiao(RegiaoId);
        }
    }
}
