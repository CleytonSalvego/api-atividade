using System.Collections.Generic;

namespace api.Models
{
    public class StatusModel
    {
        public int codigo { get; set; }
        public string descricao { get; set; }
        public bool ativo { get; set; }
        public List<AtividadeModel> atividade { get; set; }
       
    }
}
