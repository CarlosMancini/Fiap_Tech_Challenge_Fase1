using Core.Entities;
using Core.Gateways;
using Core.Repository;
using Core.Services;


namespace Fiap_Tech_Challenge_Fase1.Services
{
    public class ContatoService(IContatoRepository contatoRepository, IRegiaoRepository regiaoRepository, IBrasilGateway brasilGateway) : Service<Contato>(contatoRepository), IContatoService
    {
        private readonly IRegiaoRepository _regiaoRepository = regiaoRepository;
        private readonly IContatoRepository _contatoRepository = contatoRepository;
        private readonly IBrasilGateway _brasilGateway = brasilGateway;

        public async Task Cadastrar(Contato entidade)
        {
            int region = Int32.Parse(entidade.ContatoTelefone[..2]);
            var SelectedRegion = _regiaoRepository.ObterTodos().FirstOrDefault(item => item.RegiaoDdd == region);


            if (SelectedRegion != null)
            {
                var regiaoNome = await this._brasilGateway.BuscarDDDAsync(region);
                Regiao regiao = new()
                {
                    RegiaoNome = regiaoNome,
                    RegiaoDdd = region,
                };
                _regiaoRepository.Cadastrar(regiao);
            }
            base.Cadastrar(entidade);
        }
        public IList<Contato> ObterPorRegião(int RegiaoId)
        {
            return _contatoRepository.ObterPorRegião(RegiaoId);
        }

    }
}
