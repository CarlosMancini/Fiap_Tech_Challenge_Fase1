namespace Core.Entities
{
    public class Regiao
    {
        public int RegiaoId { get; set; }
        public required string RegiaoNome { get; set; }
        public int RegiaoDdd { get; set; }

        public ICollection<Contato> Contatos { get; set; }
    }
}
