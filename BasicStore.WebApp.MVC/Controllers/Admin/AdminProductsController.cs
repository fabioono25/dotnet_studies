using BasicStore.Catalog.Application.DTOs;
using BasicStore.Catalog.Application.Services;
using Microsoft.AspNetCore.Mvc;


namespace BasicStore.WebApp.MVC.Controllers.Admin
{
    public class AdminProductsController: Controller
    {
        private readonly IProductApplicationService _productAppService;

        public AdminProductsController(IProductApplicationService productAppService)
        {
            _productAppService = productAppService;
        }


        [HttpGet]
        [Route("admin-produtos")]
        public async Task<IActionResult> Index()
        {
            return View(await _productAppService.GetAll());
        }

        [Route("novo-produto")]
        public async Task<IActionResult> NovoProduto()
        {
            return View(await PopularCategorias(new ProductDto()));
        }

        [Route("novo-produto")]
        [HttpPost]
        public async Task<IActionResult> NovoProduto(ProductDto ProductDto)
        {
            if (!ModelState.IsValid) return View(await PopularCategorias(ProductDto));

            await _productAppService.AddProduct(ProductDto);

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("editar-produto")]
        public async Task<IActionResult> AtualizarProduto(Guid id)
        {
            return View(await PopularCategorias(await _productAppService.GetById(id)));
        }

        [HttpPost]
        [Route("editar-produto")]
        public async Task<IActionResult> AtualizarProduto(Guid id, ProductDto ProductDto)
        {
            var produto = await _productAppService.GetById(id);
            ProductDto.QuantityStock = produto.QuantityStock\;

            ModelState.Remove("QuantidadeEstoque");
            if (!ModelState.IsValid) return View(await PopularCategorias(ProductDto));

            await _productAppService.UpdateProduct(ProductDto);

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("produtos-atualizar-estoque")]
        public async Task<IActionResult> AtualizarEstoque(Guid id)
        {
            return View("Stock", await _productAppService.GetById(id));
        }

        [HttpPost]
        [Route("produtos-atualizar-estoque")]
        public async Task<IActionResult> AtualizarEstoque(Guid id, int quantidade)
        {
            if (quantidade > 0)
            {
                await _productAppService.AddStock(id, quantidade);
            }
            else
            {
                await _productAppService.RemoveStock(id, quantidade);
            }

            return View("Index", await _productAppService.GetAll());
        }

        private async Task<ProductDto> PopularCategorias(ProductDto produto)
        {
            produto.Categories = await _productAppService.GetAllCategories();
            return produto;
        }
    }
}
}
