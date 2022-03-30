using Newtonsoft.Json;
using System.Collections.Generic;

namespace api.Models
{
    public class PerfilModel
    {
        public int codigo { get; set; }
        public string descricao { get; set; }
        public List<UsuarioModel>? usuario { get; set; }
        
    }
}
