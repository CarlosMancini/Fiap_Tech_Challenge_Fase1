using Core.Entities;

namespace Core.Interfaces.Services
{
    public interface ICadastroService
    {
        Task Cadastrar(Contato entidade);
    }
}
