using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TodoApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class BikesController : ControllerBase
    {

        public BikesController()
        {
            var myJsonString = System.IO.File.ReadAllText("station_information.json");
            stations = JsonConvert.DeserializeObject<BikeObject>(myJsonString).data.stations;

        }
        public BikeData[] stations;

        [Authorize]
        [HttpGet]
        public BikeData[] GetBikeStations()
        {
            return stations;
        }

        [HttpGet("{id}")]
        public IEnumerable<BikeData> getStation(int id)
        {
            return stations.Where(x => x.station_id == id);
        }

         [HttpGet("navn/{name}")]
        public IEnumerable<BikeData> getStationByName(string name)
        {
            return stations.Where(x => x.name == name);
        }
    }
}