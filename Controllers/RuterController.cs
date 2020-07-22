using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TodoApi.Controllers
{
    public class NameQty
    {

        public string name { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
        public string atStop { get; set; }
        public NameQty(string n, string q, string p, string s)
        {
            name = n;
            lat = q;
            lng = p;
            atStop = s;
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class RuterController : ControllerBase
    {
        List<RuterData> stops = new List<RuterData>();
        string Baseurl = "https://api.entur.io/realtime/v1/rest/";

        public RuterController()
        {
            // 




        }
        [Route("/")]
        [HttpGet("[action]")]
        public string GetRuterStops()
        {
            using (StreamReader reader = new StreamReader("stops.txt"))
            {
                String line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] test;

                    test = line.Split(",");
                    test = test.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                    stops.Add(new RuterData
                    {
                        stop_id = test[0],
                        stop_name = test[1],
                        stop_lat = test[2],
                        stop_lon = test[3]
                    });
                }
            }
            return "TEST";
        }
        [Route("/")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetRuterLive()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                XElement liveData;
                IEnumerable<NameQty> data;

                HttpResponseMessage Res = await client.GetAsync("vm?datasetId=RUT&maxSize=20");

                if (Res.IsSuccessStatusCode)
                {

                    var ObjResponse = Res.Content.ReadAsStringAsync().Result;
                    liveData = XElement.Parse(ObjResponse);
                    XNamespace ns = "http://www.siri.org.uk/siri";

                    data = (from el in liveData.Descendants(ns + "MonitoredVehicleJourney")
                            select new NameQty(
                                (string)(from name in el.Descendants(ns + "MonitoredCall") select name.Element(ns + "StopPointName")).First(),
                                (string)(from test in el.Descendants(ns + "Latitude") select test).First(),
                                (string)(from test in el.Descendants(ns + "Longitude") select test).First(),
                                (string)(from test in el.Descendants(ns + "MonitoredCall") select test.Element(ns + "VehicleAtStop")).First()
                            ));

                }
                else
                {
                    data = null;
                }
                try
                {
                    return Ok(data);
                }
                catch (Exception)
                {
                    return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
                }
            }
        }

          [Route("/")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetRuterTestLive()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                XElement liveData;
                IEnumerable<string> data;

                HttpResponseMessage Res = await client.GetAsync("et?datasetId=RUT&maxSize=10");

                if (Res.IsSuccessStatusCode)
                {

                    var ObjResponse = Res.Content.ReadAsStringAsync().Result;
                    liveData = XElement.Parse(ObjResponse);
                    XNamespace ns = "http://www.siri.org.uk/siri";

                    data = from el in liveData.Descendants(ns + "EstimatedCalls")
                            select (string)(from name in el.Descendants(ns + "EstimatedCall") select name.Element(ns + "StopPointRef")).First();

                }
                else
                {
                    data = null;
                }
                try
                {
                    return Ok(data);
                }
                catch (Exception)
                {
                    return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
                }
            }
        }
    }
}

    
