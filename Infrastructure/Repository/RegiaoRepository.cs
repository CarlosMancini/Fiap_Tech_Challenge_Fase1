using Core.Entities;
using Core.Repository;

namespace Infrastructure.Repository
{
    public class RegiaoRepository(ApplicationDbContext context) : EFRepository<Regiao>(context), IRegiaoRepository
    {
    }
}
