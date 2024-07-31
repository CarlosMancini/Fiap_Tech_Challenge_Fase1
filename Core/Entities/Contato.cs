namespace Core.Entities
{
    public class Contato
    {
        public int ContatoId { get; set; }
        public required string ContatoNome { get; set; }
        public required string ContatoTelefone { get; set; }
        public required string ContatoEmail { get; set; }
        public int DddId { get; set; }

        public Regiao Regiao { get; set; }
    }
}
