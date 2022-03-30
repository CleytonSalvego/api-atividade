using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class UsuarioModel
    {
        public long codigo { get; set; }
        public string usuario { get; set; }
        public string senha { get; set; }
        public string nome { get; set; }
        public int codigo_perfil { get; set; }
        public PerfilModel perfil { get; set; }
        public bool ativo { get; set; }
      

        public List<AtividadeModel>? atividade { get; set; }
        public List<AtividadeModel>? criador { get; set; }
      

    }
}
