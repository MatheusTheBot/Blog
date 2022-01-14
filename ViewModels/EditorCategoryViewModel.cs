using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels
{
    public class EditorCategoryViewModel
    {
        [Required(ErrorMessage = "O campo Nome é obrigatório")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Esse campo teve conter entre 3 a 20 caracteres")]
        public string Name { get; set; }


        [Required(ErrorMessage = "O campo Slug é obrigatório")]
        public string Slug { get; set; }
    }
}
