using FactoryBrick.Data;
using FactoryBrick.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Text.Json;

namespace FactoryBrick.Controllers
{
    public class ImportController : Controller
    {
        ApplicationContext _db;
        private readonly IWebHostEnvironment _env;

        public ImportController(IWebHostEnvironment env, ApplicationContext db)
        {
            _env = env;
            _db = db;
        }

        public IActionResult UploadPage()
        {
            return View();
        }       

        [HttpPost]
        public IActionResult UploadText([FromBody] object inputJson)
        {            
            _db.SaveJsonToBase(JsonConvert.DeserializeObject<JsonRoot>(inputJson.ToString()));
            return Ok("All good");
        }

       
    }
}
