using APIHandler.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIHandler.DataModels
{
    public class PlanetModel : IStarWarsAPIModel
    {
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

        public string BriefDisplayTitle
        {
            get
            {

                return string.Format("ID: {0} | Name: {1} | Population: {2:#,0} | Climate: {3} | Terrain: {4}", ID, Name, Population, Climate, Terrain);

            }
        }

        public List<string> GenerateFullDisplayList()
        {
            List<string> returnList = new List<string>();
            returnList.Add("");
            returnList.Add(string.Format("### {0} ###", Name));
            returnList.Add(string.Format("Rotation Period: {0}days | Orbital Period: {1}days | Diamater: {2:#,##0}km", RotationPeriod, OrbitalPeriod, Diameter));
            returnList.Add(string.Format("Climate: {0} | Terrain: {1} | Surface Water: {2}% ", Climate, Terrain, SurfaceWater));
            returnList.Add(string.Format("Population: {0:#,##0} | Gravity: {1}", Population, Gravity));

            if (Residents.Count > 0)
            {
                returnList.Add("");
                returnList.Add("### Residents ###");
                string peopleString = null;
                for (int i = 0; i < Residents.Count; i++)
                {
                    if (peopleString == null)
                    {
                        peopleString = Residents[i].BriefDisplayTitle;
                    }
                    else
                    {
                        peopleString = string.Format("{0} # {1}", peopleString, Residents[i].BriefDisplayTitle);

                        returnList.Add(peopleString);
                        peopleString = null;
                    }
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

        public int ID { get; set; }

        public string Name { get; set; }

        public string Population { get; set; }

        public string Climate { get; set; }

        public string Terrain { get; set; }

        /// <summary>
        /// Rotation in days
        /// </summary>
        [JsonProperty("rotation_period")]
        public string RotationPeriod { get; set; }

        /// <summary>
        /// Orbit in days
        /// </summary>
        [JsonProperty("orbital_period")]
        public string OrbitalPeriod { get; set; }

        public string Diameter { get; set; }

        public string Gravity { get; set; }

        /// <summary>
        /// % of planet surface 
        /// </summary>
        [JsonProperty("surface_water")]
        public string SurfaceWater { get; set; }

        /// <summary>
        /// URLs of Residents (Persons) as provided by API
        /// </summary>
        [JsonProperty("residents")]
        public List<string> ResidentsURLS { get; set; }

        public List<PersonModel> Residents { get; set; } = new List<PersonModel>();



        /// <summary>
        /// URLs of Residents (Persons) as provided by API
        /// </summary>
        [JsonProperty("films")]
        public List<string> FilmsURLs { get; set; }

        public List<FilmModel> Films { get; set; } = new List<FilmModel>();

    }
}
