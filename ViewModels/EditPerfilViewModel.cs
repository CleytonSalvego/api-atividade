using api.Models;
using System.ComponentModel.DataAnnotations;

namespace api.ViewModels.Perfil
{
    public class EditPerfilViewModel
    {

        [Required(ErrorMessage = "O campo descricao é obrigatório")]
        public string descricao { get; set; }

    }
}
