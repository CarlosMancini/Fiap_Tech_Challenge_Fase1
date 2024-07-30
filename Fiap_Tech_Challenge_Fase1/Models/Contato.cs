namespace Fiap_Tech_Challenge_Fase1.Models
{
    public class Contato
    {
        public int ContatoId { get; set; }
        public required string ContatoNome { get; set; }
        public required string ContatoTelefone { get; set; }
        public required string ContatoEmail { get; set; }
        public int DDDId { get; set; }
    }
}
