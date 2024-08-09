using System.ComponentModel.DataAnnotations;

namespace Core.Inputs
{
    public class ContatoInputCadastrar
    {
        [Required(ErrorMessage = "O nome do contato é obrigatório.")]
        public required string ContatoNome { get; set; }

        [Required(ErrorMessage = "O telefone do contato é obrigatório.")]
        [RegularExpression(@"^\d{2}\d{8,9}$", ErrorMessage = "Formato de telefone inválido.")]
        public required string ContatoTelefone { get; set; }

        [Required(ErrorMessage = "O e-mail do contato é obrigatório.")]
        [EmailAddress(ErrorMessage = "Formato de e-mail inválido.")]
        public required string ContatoEmail { get; set; }
    }
}
