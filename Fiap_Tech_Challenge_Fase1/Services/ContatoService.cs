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
            // Verifica se já existe um contato com o mesmo nome e telefone
            var contatoExistente = await _contatoRepository.ObterPorNomeETelefone(entidade.ContatoNome, entidade.ContatoTelefone);
            if (contatoExistente != null)
            {
                throw new Exception("Já existe um contato com o mesmo nome e telefone.");
            }

            int region = Int32.Parse(entidade.ContatoTelefone[..2]);
            var allRegions = await _regiaoRepository.ObterTodos() ?? new List<Regiao>();

            var selectedRegion = allRegions.FirstOrDefault(item => item.RegiaoDdd == region);

            if (selectedRegion is null)
            {
                try
                {
                    var regiaoNome = await _brasilGateway.BuscarDDDAsync(region);
                    Regiao regiao = new()
                    {
                        RegiaoNome = regiaoNome,
                        RegiaoDdd = region,
                    };

                    await _regiaoRepository.Cadastrar(regiao);

                    // Recarrega todas as regiões
                    allRegions = await _regiaoRepository.ObterTodos() ?? new List<Regiao>();
                    selectedRegion = allRegions.FirstOrDefault(item => item.RegiaoDdd == region);

                    if (selectedRegion is null)
                    {
                        throw new Exception("Erro ao cadastrar a nova região.");
                    }
                }
                catch (Exception)
                {
                    throw new Exception("Erro ao cadastrar a nova região.");
                }
            }

            entidade.RegiaoId = selectedRegion.Id;

            await base.Cadastrar(entidade);
        }

        public async Task Alterar(Contato entidade)
        {
            int region = Int32.Parse(entidade.ContatoTelefone[..2]);
            var allRegions = await _regiaoRepository.ObterTodos() ?? new List<Regiao>();
            var selectedRegion = allRegions.FirstOrDefault(item => item.RegiaoDdd == region);

            if (selectedRegion == null)
            {
                var regiaoNome = await _brasilGateway.BuscarDDDAsync(region);
                Regiao regiao = new()
                {
                    RegiaoNome = regiaoNome,
                    RegiaoDdd = region,
                };
                try
                {
                    await _regiaoRepository.Cadastrar(regiao);
                    selectedRegion = regiao;
                }
                catch
                {
                    throw new Exception("Erro ao cadastrar a nova região.");
                }
            }

            entidade.RegiaoId = selectedRegion?.Id ?? throw new Exception("Erro ao associar a região ao contato.");

            var contatoExistente = await _contatoRepository.ObterPorNomeETelefone(entidade.ContatoNome, entidade.ContatoTelefone);
            if (contatoExistente != null)
            {
                throw new Exception("Já existe um contato com o mesmo nome e telefone.");
            }

            await base.Alterar(entidade);
        }


        public IList<Contato> ObterPorRegiao(int regiaoId)
        {
            return _contatoRepository.ObterPorRegiao(regiaoId);
        }
    }
}
