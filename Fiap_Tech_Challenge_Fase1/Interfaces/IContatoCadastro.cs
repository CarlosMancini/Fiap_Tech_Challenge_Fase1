using Fiap_Tech_Challenge_Fase1.Models;

namespace Fiap_Tech_Challenge_Fase1.Interfaces
{
    public interface IContatoCadastro
    {
        public List<Contato> GetContatos();
        public void CriarContato(Contato contato);
        public void AtualizarContato(Contato contato);
        public void DeletarContato(int id);
    }
}
