using CasaDoCodigo.Areas.Identity.Data;
using CasaDoCodigo.Models;
using CasaDoCodigo.Models.ViewModels;
using CasaDoCodigo.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CasaDoCodigo.Controllers
{
    public class PedidoController : Controller
    {
        private readonly IProdutoRepository produtoRepository;
        private readonly IPedidoRepository pedidoRepository;
        private readonly UserManager<AppIdentityUser> userManager;

        public PedidoController(IProdutoRepository produtoRepository,
            IPedidoRepository pedidoRepository, UserManager<AppIdentityUser> userManager)
        {
            this.produtoRepository = produtoRepository;
            this.pedidoRepository = pedidoRepository;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Carrossel()
        {
            return View(await produtoRepository.GetProdutosAsync());
        }

        public async Task<IActionResult> BuscaProdutos(string pesquisa)
        {
            return View(await produtoRepository.GetProdutosAsync(pesquisa));
        }

        [Authorize]
        public async Task<IActionResult> Carrinho(string codigo)
        {
            if (!string.IsNullOrEmpty(codigo))
            {
                await pedidoRepository.AddItemAsync(codigo);
            }

            var pedido = await pedidoRepository.GetPedidoAsync();
            List<ItemPedido> itens = pedido.Itens;
            CarrinhoViewModel carrinhoViewModel = new CarrinhoViewModel(itens);
            return base.View(carrinhoViewModel);
        }

        [Authorize]
        public async Task<IActionResult> Cadastro()
        {
            var pedido = await pedidoRepository.GetPedidoAsync();

            if (pedido == null)
            {
                return RedirectToAction("Carrossel");
            }

            //Obtendo informações do usuário logado do Identity para preenchimento do cadastro
            var usuario = await userManager.GetUserAsync(this.User);

            pedido.Cadastro.Email = usuario.Email;
            pedido.Cadastro.Telefone = usuario.Telefone;
            pedido.Cadastro.Nome = usuario.Nome;
            pedido.Cadastro.Endereco = usuario.Endereço;
            pedido.Cadastro.Complemento = usuario.Complemento;
            pedido.Cadastro.Bairro = usuario.Bairro;
            pedido.Cadastro.Municipio = usuario.Municipio;
            pedido.Cadastro.UF = usuario.UF;
            pedido.Cadastro.CEP = usuario.CEP;

            return View(pedido.Cadastro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Resumo(Cadastro cadastro)
        {
            if (ModelState.IsValid)
            {
                //Obtendo informações do usuário logado do Identity para preenchimento do cadastro
                var usuario = await userManager.GetUserAsync(this.User);

                usuario.Email = cadastro.Email;
                usuario.Telefone = cadastro.Telefone;
                usuario.Nome = cadastro.Nome;
                usuario.Endereço = cadastro.Endereco;
                usuario.Complemento = cadastro.Complemento;
                usuario.Bairro = cadastro.Bairro;
                usuario.Municipio = cadastro.Municipio;
                usuario.UF = cadastro.UF;
                usuario.CEP = cadastro.CEP;

                await userManager.UpdateAsync(usuario);

                return View(await pedidoRepository.UpdateCadastroAsync(cadastro));
            }

            return RedirectToAction("Cadastro");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<UpdateQuantidadeResponse> UpdateQuantidade([FromBody] ItemPedido itemPedido)
        {
            return await pedidoRepository.UpdateQuantidadeAsync(itemPedido);
        }
    }
}
