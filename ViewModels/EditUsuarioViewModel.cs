using api.Models;
using System.ComponentModel.DataAnnotations;

namespace api.ViewModels.Users
{
    public class EditUsuarioViewModel
    {
        [Required(ErrorMessage = "O campo usuário é obrigatório")]
        public string usuario { get; set; }

        [Required(ErrorMessage = "O campo nome é obrigatório")]
        public string nome { get; set; }
        [Required(ErrorMessage = "O campo codigo_perfil é obrigatório")]
        public int codigo_perfil { get; set; }
        
        [Required(ErrorMessage = "O campo ativo é obrigatório")]
        public bool ativo { get; set; }
       
       
    }
}
