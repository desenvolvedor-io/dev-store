using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace NSE.WebApp.MVC.Extensions
{
    public class SummaryViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}