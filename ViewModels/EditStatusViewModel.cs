using System.ComponentModel.DataAnnotations;

namespace api.ViewModels
{
    public class EditStatusViewModel
    {
        [Required(ErrorMessage = "O campo descricao é obrigatório")]
        public string descricao { get; set; }
    }
}
