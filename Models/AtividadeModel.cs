using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace api.Models
{
    public class AtividadeModel
    {
        public long codigo { get; set; }
       
        public long numero_documento { get; set; }
        public string titulo { get; set; }
        public string descricao { get; set; }
        public DateTime data_criacao { get; set; }
        public DateTime? data_alteracao { get; set; } = null;
       
        
        public string? solicitante { get; set; }
        public int codigo_status { get; set; }
        public StatusModel status { get; set; }
       
        public DateTime? data_planejamento { get; set; }
     
        public long codigo_criador { get; set; }
        public UsuarioModel criador { get; set; }
     

        

    }
}
