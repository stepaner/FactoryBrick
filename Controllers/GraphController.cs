using FactoryBrick.Data;
using FactoryBrick.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Diagnostics;
using System.Text.Json.Nodes;

namespace FactoryBrick.Controllers
{
    public class GraphController : Controller
    {
        ApplicationContext _db;
        private readonly ILogger<GraphController> _logger;
        JsonSerializerSettings _serializerSettings = new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver(), DateFormatString = "dd-MM-yyyy" };

        public GraphController(ILogger<GraphController> logger, ApplicationContext db)
        {
            _logger = logger;
            _db = db;
        }
        public IActionResult Сharts()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetLayerCake()
        {
            try
            {
                var res = _db.GetDataLayerCake();
                return Ok(JsonConvert.SerializeObject(res, _serializerSettings));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
                throw;
            }
        }        

        [HttpPost]
        public IActionResult GetLinearRegression(int type, DateTime? dtFrom, DateTime? dtTo)
        {
            try
            {                
                return Ok(JsonConvert.SerializeObject(CalculationGraphing.GetGraph(_db.GetConsumption(type, dtFrom, dtTo), _db.Consumers.Where(x => x.ConsumerTypeId == type).Count()), _serializerSettings));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
                throw;
            }
           
        }     

        [HttpPost]
        public IActionResult MainChart(int type, DateTime? dtFrom, DateTime? dtTo)
        {
            try
            {
                var dependencyGrpaph = DependencyGrpaphLabels.GetDependencyGrpaphLabels(_db.GetConsumerWithData(type, dtFrom, dtTo));
                return Ok(JsonConvert.SerializeObject(dependencyGrpaph, _serializerSettings));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
                throw;
            }            
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}