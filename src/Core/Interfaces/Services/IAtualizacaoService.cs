using Core.Entities;

namespace Core.Interfaces.Services
{
    public interface IAtualizacaoService
    {
        Task Atualizar(Contato entidade);
    }
}
