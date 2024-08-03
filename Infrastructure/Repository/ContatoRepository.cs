using Core.Entities;
using Core.Repository;

namespace Infrastructure.Repository
{
    public class ContatoRepository : EFRepository<Contato>, IContatoRepository
    {
        public ContatoRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
