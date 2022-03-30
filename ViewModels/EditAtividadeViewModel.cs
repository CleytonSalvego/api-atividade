using System;
using System.ComponentModel.DataAnnotations;

namespace api.ViewModels
{
    public class EditAtividadeViewModel
    {
               
        [Required(ErrorMessage = "O campo Número Documento é obrigatório")]
        public string titulo { get; set; }
        
        [Required(ErrorMessage = "O campo Descrição é obrigatório")]
        public string descricao { get; set; }

        [Required(ErrorMessage = "O campo Código Responsável é obrigatório")]
        public int codigo_responsavel { get; set; }

        public string? solicitante { get; set; }

        [Required(ErrorMessage = "O campo Código Status é obrigatório")]
        public int codigo_status { get; set; }

        public DateTime? data_planejamento { get; set; }

        [Required(ErrorMessage = "O campo Código Criado é obrigatório")]
        public int codigo_criador { get; set; }
        

    }
}
