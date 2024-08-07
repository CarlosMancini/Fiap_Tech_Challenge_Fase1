using Core.Entities;
using Core.Repository;

namespace Infrastructure.Database.Repository
{
    public class RegiaoRepository(ApplicationDbContext context) : EFRepository<Regiao>(context), IRegiaoRepository
    {
    }
}
