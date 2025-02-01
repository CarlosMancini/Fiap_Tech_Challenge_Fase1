using Core.Entities;
using Core.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Repository
{
    public class RegiaoRepository : EFRepository<Regiao>, IRegiaoRepository
    {
        public RegiaoRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IList<Regiao>> ObterTodos()
        {
            return await _dbSet.ToListAsync();
        }
    }
}
