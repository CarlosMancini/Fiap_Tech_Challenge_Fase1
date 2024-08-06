using Core.Entities;
using Core.Repository;
using Core.Services;


namespace Fiap_Tech_Challenge_Fase1.Services
{
    public class ContatoService(IContatoRepository contatoRepository, IRegiaoRepository regiaoRepository) : Service<Contato>(contatoRepository), IContatoService
    {
        private readonly IRegiaoRepository _regiaoRepository = regiaoRepository;
        private readonly IContatoRepository _contatoRepository = contatoRepository;

        public void Cadastrar (Contato entidade)
        {
            int region = Int32.Parse(entidade.ContatoTelefone[..2]);
            var SelectedRegion = _regiaoRepository.ObterTodos().FirstOrDefault(item => item.RegiaoDdd == region);
            if (SelectedRegion != null)
            {
                Regiao regiao = new()
                {
                    RegiaoNome = "nao entendi pq do nome",
                    RegiaoDdd = region,
                };
                _regiaoRepository.Cadastrar(regiao);
            }
            base.Cadastrar (entidade);
        }
        public IList<Contato> ObterPorRegião(int RegiaoId)
        {
            return _contatoRepository.ObterPorRegião(RegiaoId);
        }

    }
}
