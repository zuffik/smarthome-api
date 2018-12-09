using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmarthomeAPI.App.Components;

namespace SmarthomeAPI.Controllers
{
    [Route("component")]
    [ApiController]
    public class ComponentController : Controller
    {
        private ComponentControllers _ctrls = ComponentControllers.Instance;

        [HttpGet("types")]
        public JsonResult GetComponentTypes()
        {
            return Json(new {types = _ctrls.Controllers.Select(c => c.Identify())});
        }

        [HttpGet("{component}")]
        public JsonResult ListDevices(string component)
        {
            return Json(new
            {
                devices = _ctrls.GetController(component).GetContext().Components.ToList()
            });
        }

        [HttpGet("{component}/commands")]
        public JsonResult ListCommands(string component)
        {
            return Json(new
            {
                commands = _ctrls.GetController(component).GetCommands().Select(c => c.Identify())
            });
        }

        [HttpGet("{component}/{id}")]
        public JsonResult DeviceInfo(string component, int id)
        {
            return Json(new
            {
                device = _ctrls.GetController(component).GetContext().Components.First(c => c.Id == id)
            });
        }
    }
}