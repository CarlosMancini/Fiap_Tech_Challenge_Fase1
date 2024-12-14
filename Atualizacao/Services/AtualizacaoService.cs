using Core.Entities;
using Core.Gateways;
using Core.Interfaces.Repository;
using Core.Interfaces.Services;

namespace Atualizacao.Services
{
    public class AtualizacaoService : IAtualizacaoService
    {
        private readonly IContatoRepository _contatoRepository;
        private readonly IRegiaoRepository _regiaoRepository;
        private readonly IBrasilGateway _brasilGateway;

        public AtualizacaoService(IContatoRepository contatoRepository, IRegiaoRepository regiaoRepository, IBrasilGateway brasilGateway)
        {
            _contatoRepository = contatoRepository;
            _regiaoRepository = regiaoRepository;
            _brasilGateway = brasilGateway;
        }


        public async Task Atualizar(Contato entidade)
        {
            // Teste da pipeline de atualização

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
                throw new Exception("Ja existe um contato com o mesmo nome e telefone.");
            }

            await _contatoRepository.Atualizar(entidade);
        }
    }
}
