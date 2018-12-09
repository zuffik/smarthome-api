using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
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
        public ActionResult ListDevices(string component)
        {
            try
            {
                return Json(new
                {
                    devices = _ctrls.GetController(component).GetContext().Components.ToList()
                });
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }

        [HttpGet("{component}/commands")]
        public ActionResult ListCommands(string component)
        {
            try
            {
                return Json(new
                {
                    commands = _ctrls.GetController(component).GetCommands().Select(c => c.Identify())
                });
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }

        [HttpGet("{component}/{id}")]
        public ActionResult DeviceInfo(string component, int id)
        {
            try
            {
                return Json(new
                {
                    device = _ctrls.GetController(component).GetContext().Components.First(c => c.Id == id)
                });
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }

        [HttpPost("{component}/detect")]
        public async Task<ActionResult> DetectNewDevices(string component)
        {
            IComponentController controller;
            try
            {
                controller = _ctrls.GetController(component);
            }
            catch (InvalidOperationException)
            {
                return NotFound(new JObject
                {
                    {"err", "ControllerNotFound"}
                });
            }

            try
            {
                return Json((await controller.GetCommand<IGroupCommand>("detect").Execute()).Data);
            }
            catch (InvalidOperationException)
            {
                return BadRequest(new JObject
                {
                    {"err", "Components are not detectable or have no detect command"}
                });
            }
        }

        [HttpPost("{component}/{id}/{command}")]
        [HttpGet("{component}/{id}/{command}")]
        [HttpDelete("{component}/{id}/{command}")]
        [HttpPut("{component}/{id}/{command}")]
        public async Task<ActionResult> CommandDevice(string component, int id, string command,
            [FromBody] JObject body)
        {
            IComponentController controller;
            try
            {
                controller = _ctrls.GetController(component);
            }
            catch (InvalidOperationException)
            {
                return NotFound(new JObject
                {
                    {"err", "ControllerNotFound"}
                });
            }

            IComponentCommand comm;
            try
            {
                comm = controller.GetCommand<IComponentCommand>(command);
            }
            catch (InvalidOperationException)
            {
                return NotFound(new JObject
                {
                    {"err", "CommandNotFound"}
                });
            }

            Component comp;
            try
            {
                comp = controller.GetContext().Components.First(c => c.Id == id);
            }
            catch (InvalidOperationException)
            {
                return NotFound(new JObject
                {
                    {"err", "ComponentNotFound"}
                });
            }

            var task = controller.GetCommander()
                .ExecuteCommand(comp, comm, body["args"].Select(a => a.Value<object>()).ToArray());
            if (body["shouldWait"] != null && body["shouldWait"].Value<bool>()) return null;
            var result = await task;
            return Json(result.Data);
        }

        [HttpPost("{component}")]
        public ActionResult SaveDevice(string component, [FromBody] Component dto)

        {
            IComponentController controller;
            try
            {
                controller = _ctrls.GetController(component);
            }
            catch (InvalidOperationException)
            {
                return NotFound(new JObject
                {
                    {"err", "ControllerNotFound"}
                });
            }

            controller.GetContext().Add(dto);
            controller.GetContext().SaveChanges();
            return Json(dto);
        }
    }
}