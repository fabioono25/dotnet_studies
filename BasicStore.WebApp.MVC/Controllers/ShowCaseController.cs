using BasicStore.Catalog.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BasicStore.WebApp.MVC.Controllers
{
    public class ShowCaseController : Controller
    {
        private readonly IProductApplicationService _productAppService;

        public ShowCaseController(IProductApplicationService productAppService)
        {
            _productAppService = productAppService;
        }

        [HttpGet]
        [Route("")]
        [Route("vitrine")]
        public async Task<IActionResult> Index()
        {
            return View(await _productAppService.GetAll());
        }

        [HttpGet]
        [Route("produto-detalhe/{id}")]
        public async Task<IActionResult> ProdutoDetalhe(Guid id)
        {
            return View(await _productAppService.GetById(id));
        }
    }
}
