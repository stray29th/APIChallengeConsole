using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using APIHandler.Interfaces;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace APIHandler.DataModels
{
    public class VehicleModel : IStarWarsAPIModel
    {
        public virtual string BriefDisplayTitle
        {
            get
            {
                return string.Format("ID: {0} | Name: {1} | Class: {2}", ID, Name, Class);
            }
        }

        private List<string> fullDisplayList;

        public List<string> FullDisplayList
        {
            get
            {
                if (Loaded)
                {
                    if (fullDisplayList == null)
                    {
                        fullDisplayList = GenerateFullDisplayList();

                    }
                    return fullDisplayList;
                }
                else
                {
                    return null;
                }
            }
        }

        public int ID { get; set; }

        public bool Loaded { get; set; }

        public virtual List<string> GenerateFullDisplayList()
        {
            List<string> returnList = new List<string>();

            returnList.Add("");
            returnList.Add(string.Format("### {0} - {1} ###", Name, Class));
            returnList.Add(string.Format("Model: {0} | Manufacturer: {1}", Model, Manufacturer));
            returnList.Add(string.Format("Length: {0}m | Atmosphere Speed: {1}", Length, MaxSpeed));
            returnList.Add(string.Format("Crew: {0:#,##0} | Passengers: {1:#,##0} | Cargo: {2:#,##0}kg", Crew, Passengers, CargoCapacity));
            returnList.Add(string.Format("Cost: {0:#,##0}cr | Consumables: {1}", Cost, Passengers));

            returnList.Add("");
            returnList.Add("### Pilots ###");
            foreach (var item in Pilots)
            {
                returnList.Add(item.BriefDisplayTitle);
            }

            returnList.Add("");
            returnList.Add("### Films ###");
            foreach (var item in Films)
            {
                returnList.Add(item.BriefDisplayTitle);
            }

            return returnList;
        } 

        public string Name { get; set; }

        public string Model { get; set; }

        public string Manufacturer { get; set; }

        /// <summary>
        /// Cost in Credits
        /// </summary>
        [JsonProperty("cost_in_credits")]
        public string Cost { get; set; }


        /// <summary>
        /// Length in metres
        /// </summary>
        public string Length { get; set; }

        [JsonProperty("max_atmosphering_speed")]
        public string MaxSpeed { get; set; }


        public string Crew { get; set; }

        public string Passengers { get; set; }

        /// <summary>
        /// Capacity in Kilograms
        /// </summary>
        [JsonProperty("cargo_capacity")]
        public string CargoCapacity { get; set; }

        /// <summary>
        /// Supplies essential to life
        /// </summary>
        public string Consumables { get; set; }

        [JsonProperty("vehicle_class")]
        public virtual string Class { get; set; }

        [JsonProperty("pilots")]
        public List<string> PilotURLs { get; set; }

        public List<PersonModel> Pilots { get; set; } = new List<PersonModel>();

        [JsonProperty("films")]
        public List<string> FilmURLs { get; set; }

        [JsonIgnore]
        public List<FilmModel> Films { get; set; } = new List<FilmModel>();
    }
}
