using FactoryBrick.Data;
using FactoryBrick.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Diagnostics;

namespace FactoryBrick.Controllers
{
    public class GraphController : Controller
    {
        ApplicationContext _db;
        private readonly ILogger<GraphController> _logger;

        public GraphController(ILogger<GraphController> logger, ApplicationContext db)
        {
            _logger = logger;
            _db = db;
        }
        public IActionResult Сharts()
        {
            return View();
        }
        public IActionResult GetLayerCake(int type)
        {
            var res = _db.GetDataLayerCake(type);            
            return Ok(JsonConvert.SerializeObject(res, new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() }));
        }

        public IActionResult GetLinearRegression(int type)
        {
            var res = CalculationGraphing.GetGraph(_db.ConsumptionDatas.Where(x => x.Consumer.ConsumerTypeId == type).ToList());
            return Ok(JsonConvert.SerializeObject(res, new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() }));
        }

        [HttpPost]
        public IActionResult MainChart(int type, DateTime? dtFrom, DateTime? dtTo)
        {
            var dependencyGrpaph = DependencyGrpaphLabels.GetDependencyGrpaphLabels(_db.GetConsumerWithData(type, dtFrom, dtTo));
            return Ok(JsonConvert.SerializeObject(dependencyGrpaph, new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() }));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}