using System.Web.Mvc;

namespace ChessSystem.Controllers
{
    public class ErrorsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AccessDenied()
        {
            Response.StatusCode = 403;
            return View();
        }

        public ActionResult NotFound()
        {
            Response.StatusCode = 404;
            return View();
        }
    }
}