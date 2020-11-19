using CasaDoCodigo.Areas.Identity.Data;
using CasaDoCodigo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CasaDoCodigo
{
    //MELHORIA: 8) dados do cadastro gravados na sessão
    public class HttpHelper : IHttpHelper
    {
        private readonly IHttpContextAccessor contextAccessor;
        private readonly UserManager<AppIdentityUser> userManager;

        public IConfiguration Configuration { get; }

        public HttpHelper(IHttpContextAccessor contextAccessor
            , IConfiguration configuration
            , UserManager<AppIdentityUser> userManager)
        {
            this.contextAccessor = contextAccessor;
            Configuration = configuration;
            this.userManager = userManager;
        }

        public int? GetPedidoId() => contextAccessor.HttpContext.Session.GetInt32($"pedidoId{ GetClienteId()}");

        private string GetClienteId()
        {
            //Esta classe que armazena informaçoes sobre o usuário
            var claimsPrincipal = contextAccessor.HttpContext.User;
            var clienteId = userManager.GetUserId(claimsPrincipal);
            return clienteId;
        }

        public void SetPedidoId(int pedidoId)
        {
            contextAccessor.HttpContext.Session.SetInt32($"pedidoId{GetClienteId()}", pedidoId);
        }

        public void ResetPedidoId()
        {
            contextAccessor.HttpContext.Session.Remove($"pedidoId{GetClienteId()}");
        }

        public void SetCadastro(Cadastro cadastro)
        {
            string json = JsonConvert.SerializeObject(cadastro.GetClone());
            contextAccessor.HttpContext.Session.SetString("cadastro", json);
        }

        public Cadastro GetCadastro()
        {
            string json = contextAccessor.HttpContext.Session.GetString("cadastro");
            if (string.IsNullOrWhiteSpace(json))
                return new Cadastro();

            return JsonConvert.DeserializeObject<Cadastro>(json);
        }
    }

}
