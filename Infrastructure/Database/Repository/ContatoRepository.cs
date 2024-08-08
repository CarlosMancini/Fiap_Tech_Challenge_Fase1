using Core.Entities;
using Core.Interfaces.Repository;

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
    }
}
