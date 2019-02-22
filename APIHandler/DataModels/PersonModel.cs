using APIHandler.DataModels;
using APIHandler.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIHandler
{
    /// <summary>
    /// Wrapper model for JSON return from API
    /// </summary>
    public class PersonModel : IStarWarsAPIModel
    {

        public PersonModel()
        {

        }

        public bool Loaded { get; set; }

        [JsonIgnore]
        public int ID { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Height in CM
        /// </summary>
        public string Height { get; set; }

        /// <summary>
        /// Weight in Kilograms
        /// </summary>
        public string Mass { get; set; }

        [JsonProperty("hair_color")]
        public string HairColour { private get; set; }

        [JsonProperty("skin_color")]
        public string SkinColour { private get; set; }

        [JsonProperty("eye_color")]
        public string EyeColour { private get; set; }

        /// <summary>
        /// Birthyear using BBY (Before the Battle of Yavin) format
        /// </summary>
        [JsonProperty("birth_year")]
        public string BirthYear { get; set; }

        public string Gender { get; set; }

        [JsonProperty("homeworld")]
        public string HomeworldUrl { get; set; }

        public PlanetModel Homeworld { get; set; }

        /// <summary>
        /// URLs of Species as provided by API
        /// </summary>
        [JsonProperty("species")]
        public List<string> SpeciesUrls { get; set; }

        public SpeciesModel Species { get; set; }

        /// <summary>
        /// URLs of Vehicles as provided by API
        /// </summary>
        [JsonProperty("vehicles")]
        public List<string> VehiclesUrls { get; set; }

        public List<VehicleModel> Vehicles { get; set; } = new List<VehicleModel>();

        /// <summary>
        /// URLs of Vehicles as provided by API
        /// </summary>
        [JsonProperty("starships")]
        public List<string> StarshipsUrls { get; set; }

        public List<StarshipModel> Starships { get; set; } = new List<StarshipModel>();

        [JsonProperty("films")]
        public List<string> FilmURLs { get; set; }


        public List<FilmModel> Films { get; set; } = new List<FilmModel>();

        private List<string> fullDisplayList;

        /// <summary>
        /// Returns a list of strings to display in console
        /// </summary>
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

        public string BriefDisplayTitle
        {
            get
            {

                return string.Format("ID: {0} | Name: {1} | Born: {2}",ID,  Name, BirthYear);

            }
        }



        public List<string> GenerateFullDisplayList()
        {
            List<string> returnList = new List<string>();
            returnList.Add("");
            returnList.Add(string.Format("### {0} ###", Name));
            returnList.Add(string.Format("Species: {4} | Gender: {1} | Born: {2} | Height {3}cm",
                Name, Gender, BirthYear, Height, Species.Name));
            returnList.Add(string.Format("Mass: {0}kg | Hair Colour: {1} | Skin Colour: {2} | Eye Colour: {3}",
                Mass, HairColour, SkinColour, EyeColour));
            returnList.Add("");
            returnList.Add("### Homeworld ###");
            returnList.Add(Homeworld.BriefDisplayTitle);

            returnList.Add("");
            returnList.Add("### Vehicles ###");
            foreach (var item in Vehicles)
            {
                returnList.Add(item.BriefDisplayTitle);
            }

            returnList.Add("");
            returnList.Add("### Starships ###");
            foreach (var item in Starships)
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


    }
}
