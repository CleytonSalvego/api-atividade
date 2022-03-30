using System.ComponentModel.DataAnnotations;

namespace api.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Informe o usuário")]
        public string usuario { get; set; }

        [Required(ErrorMessage = "Informe a senha")]
        public string senha { get; set; }
    }
}
