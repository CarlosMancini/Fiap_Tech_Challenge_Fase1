namespace Core.Entities
{
    public class Contato : EntityBase
    {
        public required string ContatoNome { get; set; }
        public required string ContatoTelefone { get; set; }
        public required string ContatoEmail { get; set; }
        public int RegiaoId { get; set; }

        public Regiao Regiao { get; set; }
    }
}
