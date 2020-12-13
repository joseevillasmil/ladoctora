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
using SendGrid;
using SendGrid.Helpers.Mail;
using Google.Apis.Auth.OAuth2;
using System.Threading;
using Google.Apis.Util.Store;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;

namespace ladoctora.index.Controllers
{
    public class HoraController : Controller
    {
        // GET: HoraController
        const string COSMOS_CONSTR = "AccountEndpoint=https://ladoctora.documents.azure.com:443/;AccountKey=WeYBZ904lzpo9pTTytoKErcPKdsAuOzbKuAPPXaqYHa21LRTCNqXGlqrFTf0EtkxZaY8PAskDzpAaCu1Y1mi5g==;";

        const string SENDGRID_API_KEY = "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";
        public JsonResult Index()
        {
            StreamReader r = new StreamReader("config.json");
            Config config = JsonConvert.DeserializeObject<Config>(r.ReadToEnd());
            List<string> fechas = new List<string>();
            var fechaFin = DateTime.Now.AddDays(7);
            using (CosmosClient cosmosClient = new CosmosClient(COSMOS_CONSTR))
            {
                var container = cosmosClient.GetContainer("ladoctora", "horas");

                // calculamos máximo de horas por día.
                int maximoPorHora = 60 / (int)config.atencionesPorVez;


                for (DateTime fecha = DateTime.Now; fecha <= fechaFin; fecha = fecha.AddDays(1))
                {
                    // día de la semana
                    dynamic item = this.validarDia(fecha);
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


                    if (usadasDelDia.First()["total"] >= maximasPorDia)
                    {
                        continue;
                    }

                    fechas.Add(fecha.ToString("yyyy-MM-dd"));
                }
            }

            return Json(fechas);
        }

        // GET: HoraController/Details/5
        public JsonResult Details(string dia)
        {
            StreamReader r = new StreamReader("config.json");
            Config config = JsonConvert.DeserializeObject<Config>(r.ReadToEnd());
            List<string> horas = new List<string>();

            using (CosmosClient cosmosClient = new CosmosClient(COSMOS_CONSTR))
            {
                var container = cosmosClient.GetContainer("ladoctora", "horas");

                // validamos.
                DateTime DTdia = DateTime.Parse(dia);
                dynamic item = this.validarDia(DTdia);
                if (!item["valido"])
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

                    }
                    catch { }
                }
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
                using (CosmosClient cosmosClient = new CosmosClient(COSMOS_CONSTR))
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
                    { "hora", collection["hora"].ToString() },
                    { "code", RandomString(8)},
                    { "confirmada", false },
                };
                    container.CreateItemAsync(item).GetAwaiter().GetResult();
                    //
                    DateTime fecha = DateTime.Parse($"{collection["dia"]} {collection["hora"]}:00");
                    // Enviamos el email.
                    var sendGridClient = new SendGridClient(SENDGRID_API_KEY);

                    var sendGridMessage = new SendGridMessage();
                    sendGridMessage.SetFrom("horas@ladoctora.cl");
                    sendGridMessage.AddTo(collection["correo"].ToString());
                    sendGridMessage.SetTemplateId("d-45a82549a834461d9919e4db20432e6e");
                    var data = new Models.Emails.ReservaEmail
                    {
                        code = item["code"],
                        date = fecha.ToString("dd/MM/yyyy HH:mm")
                    };
                    sendGridMessage.SetTemplateData(data);

                    var response = sendGridClient.SendEmailAsync(sendGridMessage).GetAwaiter().GetResult();
                }
                return Json("ok");
            }
            catch
            {
               return Json("error");
            }
        }

        [HttpPost]
        public JsonResult Confirmar(IFormCollection collection)
        {
            try
            {
                using (CosmosClient cosmosClient = new CosmosClient(COSMOS_CONSTR))
                {
                    var container = cosmosClient.GetContainer("ladoctora", "horas");
                    QueryDefinition queryCount = new QueryDefinition($"SELECT *  FROM c WHERE c.id = '{collection["id"]}' AND c.confirmada != true AND c.fecha >= '{DateTime.Now.ToString("yyyy-MM-dd")}'");
                    var hora = container.GetItemQueryIterator<Dictionary<string, dynamic>>(queryCount, null)
                        .ReadNextAsync()
                        .GetAwaiter()
                        .GetResult()
                        .ToList()
                        .First();
                    hora["confirmada"] = true;
                    container.ReplaceItemAsync(hora, collection["id"]).GetAwaiter().GetResult();
                    // Enviamos el email.
                    var sendGridClient = new SendGridClient(SENDGRID_API_KEY);

                    var sendGridMessage = new SendGridMessage();
                    sendGridMessage.SetFrom("horas@ladoctora.cl");
                    sendGridMessage.AddTo(hora["correo"].ToString());
                    sendGridMessage.SetTemplateId("d-b043538b104f4397aebcd1cd7ccca717");
                    DateTime datetime = DateTime.Parse(hora["fecha"]);
                    var data = new Models.Emails.ReservaEmail
                    {
                        code = hora["code"],
                        date = datetime.ToString("dd/MM/yyyy HH:mm")
                    };
                    // configuración
                    Dictionary<string, dynamic> _return = new Dictionary<string, dynamic>();
                    StreamReader r = new StreamReader("config.json");
                    Config config = JsonConvert.DeserializeObject<Config>(r.ReadToEnd());

                    string title = $"{hora["nombre"]} {hora["apellido"]}";
                    string description = $"Nombre: {hora["nombre"]} {hora["apellido"]} \n Correo: {hora["correo"]} \n Teléfono: {hora["telefono"]}";
                    this.createEvent(datetime, datetime.AddMinutes(config.minutos), title, description);
                    sendGridMessage.SetTemplateData(data);

                    var response = sendGridClient.SendEmailAsync(sendGridMessage).GetAwaiter().GetResult();
                }
                return Json("ok");
            }
            catch(Exception e)
            {
                return Json("error");
            }
        }

        
        public static string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
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
        // funciones privadas.
        private void createEvent(DateTime from, DateTime to, string title, string description)
        {
            // Todo esto es de la documentación 

            GoogleCredential credential;
            string[] scopes = { CalendarService.Scope.Calendar };
            using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                                 .CreateScoped(scopes);
            }

            // Create Google Calendar API service.
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Agendor",
            });

            // Generamos los eventos.
            var ev = new Event();
            EventDateTime start = new EventDateTime();
            start.DateTime = new DateTime(from.Year, from.Month, from.Day, from.Hour, from.Minute, 0);

            EventDateTime end = new EventDateTime();
            end.DateTime = new DateTime(to.Year, to.Month, to.Day, to.Hour, to.Minute, 0);

            ev.Start = start;
            ev.End = end;
            ev.Summary = title;
            ev.Description = description;

            var calendarId = "1ptprrf3j4ip64sa0r7i1f1i6g@group.calendar.google.com";
            // guardar
            Event recurringEvent = service.Events.Insert(ev, calendarId).Execute();


        }
    }
}
