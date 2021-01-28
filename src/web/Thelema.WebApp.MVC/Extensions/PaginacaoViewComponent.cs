using Microsoft.AspNetCore.Mvc;
using Thelema.WebApp.MVC.Models;

namespace Thelema.WebApp.MVC.Extensions
{
    public class PaginacaoViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(IPagedList modeloPaginado)
        {
            return View(modeloPaginado);
        }
    }
}