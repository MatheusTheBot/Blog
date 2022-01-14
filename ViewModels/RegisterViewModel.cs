using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "O campo Nome deve estar preenchido")]
        public string Name { get; set; }


        [Required(ErrorMessage = "O campo Email deve estar preenchido")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; }
    }
}
