namespace Core.Inptus
{
    public  class ContatoInput
    {
        public int Id { get; set; }
        public required string ContatoNome { get; set; }
        public required string ContatoTelefone { get; set; }
        public required string ContatoEmail { get; set; }
    }
}
