using Core.Entities;
using Core.Gateways;
using Core.Interfaces.Repository;
using Core.Interfaces.Services;

namespace Cadastro.Services
{
    public class CadastroService : ICadastroService
    {
        private readonly IContatoRepository _contatoRepository;
        private readonly IRegiaoRepository _regiaoRepository;
        private readonly IBrasilGateway _brasilGateway;

        public CadastroService(IContatoRepository contatoRepository, IRegiaoRepository regiaoRepository, IBrasilGateway brasilGateway)
        {
            _contatoRepository = contatoRepository;
            _regiaoRepository = regiaoRepository;
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

            int region = Int32.Parse(entidade.ContatoTelefone[..2]); // Considerando que os dois primeiros números são o DDD
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

            await _contatoRepository.Cadastrar(entidade);
        }
    }
}
