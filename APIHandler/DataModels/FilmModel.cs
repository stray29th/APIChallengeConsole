using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APIHandler.Interfaces;
using Newtonsoft.Json;

namespace APIHandler.DataModels
{
    public class FilmModel : IStarWarsAPIModel
    {
        public string BriefDisplayTitle
        {
            get
            {

                return string.Format("ID: {0} | Name: {1} | Episode: {2} | Release Date: {3:d}",ID,  Name, Episode, ReleaseDate);

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

        public bool Loaded { get; set; }

        public int ID { get; set; }

        public List<string> GenerateFullDisplayList()
        {
            List<string> returnList = new List<string>();

            returnList.Add("");
            returnList.Add(string.Format("### {0} - {1:d} ###", Name, ReleaseDate));
            returnList.Add(string.Format("Episode: {0} | Director: {1} | Producer: {2}", Episode, Director, Producer));


            if (Persons.Count > 0)
            {
                returnList.Add("");
                returnList.Add("### Characters ###");
                string peopleString = null;
                for (int i = 0; i < Persons.Count; i++)
                {
                    if (peopleString == null)
                    {
                        peopleString = Persons[i].BriefDisplayTitle;
                    }
                    else
                    {
                        peopleString = string.Format("{0} # {1}", peopleString, Persons[i].BriefDisplayTitle);

                        returnList.Add(peopleString);
                        peopleString = null;
                    }
                }
            }

            if (Planets.Count > 0)
            {
                returnList.Add("");
                returnList.Add("### Planets ###");
                foreach (var item in Planets)
                {
                    returnList.Add(item.BriefDisplayTitle);
                }
            }

            if (Vehicles.Count > 0)
            {
                returnList.Add("");
                returnList.Add("### Vehicles ###");
                foreach (var item in Vehicles)
                {
                    returnList.Add(item.BriefDisplayTitle);
                }
            }

            if (Starships.Count > 0)
            {
                returnList.Add("");
                returnList.Add("### Starships ###");
                foreach (var item in Starships)
                {
                    returnList.Add(item.BriefDisplayTitle);
                }
            }

            if (Species.Count > 0)
            {
                returnList.Add("");
                returnList.Add("### Species ###");
                foreach (var item in Species)
                {
                    returnList.Add(item.BriefDisplayTitle);
                }
            }

            return returnList;
        }

        [JsonProperty("title")]
        public string Name { get; set; }

        [JsonProperty("episode_id")]
        public string Episode { get; set; }

        public string Director { get; set; }

        public string Producer { get; set; }

        [JsonProperty("release_date")]
        public DateTime ReleaseDate { get; set; }

        [JsonProperty("characters")]
        public List<string> PersonURLs { get; set; }

        public List<PersonModel> Persons { get; set; } = new List<PersonModel>();

        [JsonProperty("planets")]
        public List<string> PlanetURLs { get; set; }

        [JsonIgnore]
        public List<PlanetModel> Planets { get; set; } = new List<PlanetModel>();

        [JsonProperty("starships")]
        public List<string> StarshipURLs { get; set; }

        public List<StarshipModel> Starships { get; set; } = new List<StarshipModel>();

        [JsonProperty("vehicles")]
        public List<string> VehicleURLs { get; set; }

        public List<VehicleModel> Vehicles { get; set; } = new List<VehicleModel>();

        [JsonProperty("species")]
        public List<string> SpeciesURLs { get; set; }

        public List<SpeciesModel> Species { get; set; } = new List<SpeciesModel>();
    }
}
