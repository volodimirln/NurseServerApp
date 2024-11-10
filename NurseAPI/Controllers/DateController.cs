using Microsoft.AspNetCore.Mvc;

namespace NurseAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DateController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public DateController(IWebHostEnvironment webHostEnvironment) : base()
        {
            _webHostEnvironment = webHostEnvironment;
        }

        [Route("GetServices")]
        [HttpGet]
        public ActionResult GetServices()
        {
            var filePath = Path.Combine(_webHostEnvironment.ContentRootPath + "/Data/services.json");
            using (StreamReader reader = new StreamReader(filePath))
            {
                string text = reader.ReadToEnd();
                return Ok(text);
            }
        }

        [Route("GetDescriptoin")]
        [HttpGet]
        public ActionResult GetDescriptoin()
        {
            var filePath = Path.Combine(_webHostEnvironment.ContentRootPath + "/Data/descriptoin.json");
            using (StreamReader reader = new StreamReader(filePath))
            {
                string text = reader.ReadToEnd();
                return Ok(text);
            }
        }
    }
}
