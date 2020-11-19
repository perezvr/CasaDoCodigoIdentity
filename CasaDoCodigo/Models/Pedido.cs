using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CasaDoCodigo.Models
{
    //MELHORIA: 5) arquivo modelo.cs foi quebrado em vários arquivos
    //veja o diagrama
    public class Pedido : BaseModel
    {
        public Pedido(string clientId)
        {
            ClientId = clientId;
            Cadastro = new Cadastro();
        }

        public Pedido(string clientId, Cadastro cadastro)
        {
            ClientId = clientId;
            Cadastro = cadastro;
        }

        public List<ItemPedido> Itens { get; private set; } = new List<ItemPedido>();

        [ForeignKey("CadastroId")]
        public int CadastroId { get; set; }
        [Required]
        public virtual Cadastro Cadastro { get; private set; }
        [Required]
        public string ClientId { get; set; }
    }
}
