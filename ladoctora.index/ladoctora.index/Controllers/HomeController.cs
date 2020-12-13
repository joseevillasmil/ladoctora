using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ladoctora.index.Models;
using Microsoft.Azure.Cosmos;

namespace ladoctora.index.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        const string COSMOS_CONSTR = "AccountEndpoint=https://ladoctora.documents.azure.com:443/;AccountKey=WeYBZ904lzpo9pTTytoKErcPKdsAuOzbKuAPPXaqYHa21LRTCNqXGlqrFTf0EtkxZaY8PAskDzpAaCu1Y1mi5g==;";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Confirm(string id)
        {
            using (CosmosClient cosmosClient = new CosmosClient(COSMOS_CONSTR))
            {
                try
                {
                    var container = cosmosClient.GetContainer("ladoctora", "horas");
                    QueryDefinition queryCount = new QueryDefinition($"SELECT *  FROM c WHERE c.id = '{id}' AND c.confirmada != true  AND c.fecha >= '{DateTime.Now.ToString("yyyy-MM-dd")}'");
                    var hora = container.GetItemQueryIterator<Dictionary<string, dynamic>>(queryCount, null)
                        .ReadNextAsync()
                        .GetAwaiter()
                        .GetResult()
                        .ToList()
                        .First();
                    DateTime datetime = DateTime.Parse(hora["fecha"]);
                    hora["dia"] = datetime.ToString("dd/MM/yyyy");
                    ViewBag.hora = hora;
                }
                catch(Exception e) 
                { 
                }
                
            }
                return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
