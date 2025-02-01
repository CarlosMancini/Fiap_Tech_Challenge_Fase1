using Core.Entities;
using Core.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Repository
{
    public class ContatoRepository : EFRepository<Contato>, IContatoRepository
    {
        public ContatoRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IList<Contato> ObterPorRegiao(int RegiaoId)
        {
            return _dbSet.Where(item => item.RegiaoId == RegiaoId).ToList();
        }

        public async Task<Contato> ObterPorNomeETelefone(string nome, string telefone)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.ContatoNome == nome && c.ContatoTelefone == telefone);
        }
    }
}
