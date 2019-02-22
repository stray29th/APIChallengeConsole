using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APIHandler.Interfaces;
using Newtonsoft.Json;

namespace APIHandler.DataModels
{
    public class StarshipModel : VehicleModel
    {

        [JsonProperty("hyperdrive_rating")]
        public string HyperdriveRating { get; set; }

        [JsonProperty("MGLT")]
        public string MegalightsPerHour { get; set; }

        [JsonProperty("starship_class")]
        public override string Class { get; set; }


        public override List<string> GenerateFullDisplayList()
        {
            List<string> returnList = new List<string>();

            returnList.Add("");
            returnList.Add(string.Format("### {0} - {1} ###", Name, Class));
            returnList.Add(string.Format("Model: {0} | Manufacturer: {1} | Hyperdrive Rating: {2}", Model, Manufacturer, HyperdriveRating));
            returnList.Add(string.Format("Length: {0}m | Atmosphere Speed: {1}", Length, Manufacturer, Crew));
            returnList.Add(string.Format("Crew: {0:#,##0} | Passengers: {1:#,##0} | Cargo: {2:#,##0}kg", Crew, Passengers, CargoCapacity));
            returnList.Add(string.Format("Cost: {0:#,##0}cr | Consumables: {1}", Cost, Passengers));

            if (Pilots.Count > 0)
            {
                returnList.Add("");
                returnList.Add("### Pilots ###");
                foreach (var item in Pilots)
                {
                    returnList.Add(item.BriefDisplayTitle);
                } 
            }

            if (Films.Count > 0)
            {
                returnList.Add("");
                returnList.Add("### Films ###");
                foreach (var item in Films)
                {
                    returnList.Add(item.BriefDisplayTitle);
                } 
            }

            return returnList;
        }
    }
}
