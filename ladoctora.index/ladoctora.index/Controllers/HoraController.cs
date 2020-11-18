using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos;
using System.IO;
using ladoctora.index.Models;

namespace ladoctora.index.Controllers
{
    public class HoraController : Controller
    {
        // GET: HoraController
        CosmosClient cosmosClient = new CosmosClient("AccountEndpoint=https://ladoctora.documents.azure.com:443/;AccountKey=WeYBZ904lzpo9pTTytoKErcPKdsAuOzbKuAPPXaqYHa21LRTCNqXGlqrFTf0EtkxZaY8PAskDzpAaCu1Y1mi5g==;");

        public JsonResult Index()
        {
            StreamReader r = new StreamReader("config.json");
            Config config = JsonConvert.DeserializeObject<Config>(r.ReadToEnd());
            List<string> fechas = new List<string>();
            var fechaFin = DateTime.Now.AddDays(7);
            
            var container = cosmosClient.GetContainer("ladoctora", "horas");

            // calculamos máximo de horas por día.
            int maximoPorHora = 60 / (int)config.atencionesPorVez;
            
            
            for (DateTime fecha = DateTime.Now; fecha <= fechaFin; fecha = fecha.AddDays(1))
            {
                // día de la semana
                dynamic item  = this.validarDia(fecha);
                if (!item["valido"])
                {
                    continue;
                }

                // validamos que no esten tomadas todas las horas para este día.
                var horaInicio = item["dia"].horario[0].Split(":");
                var horaFin = item["dia"].horario[1].Split(":");
                DateTime _horaInicio = new DateTime((int)DateTime.Now.Year, (int)DateTime.Now.Month, (int)DateTime.Now.Day, Int16.Parse(horaInicio[0]), Int16.Parse(horaInicio[1]), 0);
                DateTime _horaFin = new DateTime((int)DateTime.Now.Year, (int)DateTime.Now.Month, (int)DateTime.Now.Day, Int16.Parse(horaFin[0]), Int16.Parse(horaFin[1]), 0);
                Double difMin = _horaFin.Subtract(_horaInicio).TotalMinutes;
                int maximasPorDia = (int)(difMin / config.minutos) * config.atencionesPorVez;

                QueryDefinition queryCount = new QueryDefinition($"SELECT COUNT(1) as total FROM c WHERE c.fecha >= '{fecha.ToString("yyyy-MM-dd")} 00:00:00' AND c.fecha <= '{fecha.ToString("yyyy-MM-dd")} 00:00:00'");
                var usadasDelDia = container.GetItemQueryIterator<Dictionary<string, int>>(queryCount, null)
                    .ReadNextAsync()
                    .GetAwaiter()
                    .GetResult()
                    .ToList();

                
                if(usadasDelDia.First()["total"] >= maximasPorDia)
                {
                    continue;
                }

                fechas.Add(fecha.ToString("yyyy-MM-dd"));
            }

            return Json(fechas);
        }

        // GET: HoraController/Details/5
        public JsonResult Details(string dia)
        {
            StreamReader r = new StreamReader("config.json");
            Config config = JsonConvert.DeserializeObject<Config>(r.ReadToEnd());
            List<string> horas = new List<string>();

            var container = cosmosClient.GetContainer("ladoctora", "horas");

            // validamos.
            DateTime DTdia = DateTime.Parse(dia);
            dynamic item = this.validarDia(DTdia);
            if(!item["valido"])
            {
                return Json(horas);
            }

            // horas
            DateTime fechaInicio = DateTime.Parse($"{dia} {item["dia"].horario[0]}:00");
            DateTime fechaFin = DateTime.Parse($"{dia} {item["dia"].horario[1]}:00");

            QueryDefinition queryCount = new QueryDefinition($"SELECT c.fecha FROM c WHERE c.fecha >= '{fechaInicio.ToString("yyyy-MM-dd HH:mm:ss")}' AND c.fecha <= '{fechaFin.ToString("yyyy-MM-dd HH:mm:ss")}'");
            var horasUsadas = container.GetItemQueryIterator<Dictionary<string, string>>(queryCount, null)
                .ReadNextAsync()
                .GetAwaiter()
                .GetResult()
                .ToList();

            for (DateTime fecha = fechaInicio; fecha <= fechaFin; fecha = fecha.AddMinutes(config.minutos))
            {
                try
                {
                    var coincidencias = from h in horasUsadas where String.Equals(h["fecha"], fecha.ToString("yyyy-MM-dd HH:mm:ss")) select h["fecha"];

                    if (coincidencias.Count() >= config.atencionesPorVez)
                    {
                        continue;
                    }
                    horas.Add(fecha.ToString("HH:mm"));

                } catch { }               
            }

            return Json(horas);
        }

        private Dictionary<string, dynamic> validarDia(DateTime dia)
        {
            Dictionary<string, dynamic> _return = new Dictionary<string, dynamic>();
            StreamReader r = new StreamReader("config.json");
            Config config = JsonConvert.DeserializeObject<Config>(r.ReadToEnd());

            switch (dia.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    _return.Add("valido", config.lunes.atencion);
                    _return.Add("dia", config.lunes);
                    break;
                case DayOfWeek.Tuesday:
                    _return.Add("valido", config.martes.atencion);
                    _return.Add("dia", config.martes);
                    break;
                case DayOfWeek.Wednesday:
                    _return.Add("valido", config.miercoles.atencion);
                    _return.Add("dia", config.miercoles);
                    break;
                case DayOfWeek.Thursday:
                    _return.Add("valido", config.jueves.atencion);
                    _return.Add("dia", config.jueves);
                    break;
                case DayOfWeek.Friday:
                    _return.Add("valido", config.viernes.atencion);
                    _return.Add("dia", config.viernes);
                    break;
                case DayOfWeek.Saturday:
                    _return.Add("valido", config.sabado.atencion);
                    _return.Add("dia", config.sabado);
                    break;
                case DayOfWeek.Sunday:
                    _return.Add("valido", config.domingo.atencion);
                    _return.Add("dia", config.domingo);
                    break;
            }
            return _return;
        }


        // POST: HoraController/Create
        //[ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult Create(IFormCollection collection)
        {
            try
            {
                var container = cosmosClient.GetContainer("ladoctora", "horas");
                var item = new Dictionary<string, dynamic>() {
                    { "id", collection["correo"] + DateTime.UtcNow.ToString("yyyyMMddHHmmss") },
                    { "nombre", collection["nombre"].ToString() },
                    { "apellido", collection["apellido"].ToString() },
                    { "correo", collection["correo"].ToString() },
                    { "telefono", collection["telefono"].ToString() },
                    { "fecha", $"{collection["dia"]} {collection["hora"]}:00" },
                    { "dia", collection["dia"].ToString() },
                    { "hora", collection["hora"].ToString() }
                };
                container.CreateItemAsync(item).GetAwaiter().GetResult();
                return Json("ok");
            }
            catch
            {
               return Json("error");
            }
        }

        // GET: HoraController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HoraController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HoraController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HoraController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
