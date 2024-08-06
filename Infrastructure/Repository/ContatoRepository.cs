using Core.Entities;
using Core.Repository;

namespace Infrastructure.Repository
{
    public class ContatoRepository : EFRepository<Contato>, IContatoRepository
    {
        public ContatoRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IList<Contato> ObterPorRegião(int RegiaoId)
        {
            return _dbSet.Where(item => item.RegiaoId == RegiaoId).ToList();
        }
    }
}
