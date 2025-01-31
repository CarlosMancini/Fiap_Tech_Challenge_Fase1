namespace Core.Entities
{
    public class Regiao : EntityBase
    {
        public required string RegiaoNome { get; set; }
        public int RegiaoDdd { get; set; }

        public ICollection<Contato> Contatos { get; set; }
    }
}
