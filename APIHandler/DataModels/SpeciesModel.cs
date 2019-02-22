using APIHandler.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace APIHandler.DataModels
{
    public class SpeciesModel : IStarWarsAPIModel
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

                return string.Format("ID: {0} | Name: {1} | Classification: {2} | Designation: {3}", ID,  Name, Classification, Designation);

            }
        }


        public string Name { get; set; }

        public int ID { get; set; }

        [JsonProperty("average_height")]
        public string AverageHeight { get; set; }

        [JsonProperty("average_lifespan")]
        public string AverageLifespan { get; set; }

        public string Classification { get; set; }

        public string Designation { get; set; }

        public string Language { get; set; }

        [JsonProperty("eye_colors")]
        public string EyeColours { get; set; }

        [JsonProperty("hair_colors")]
        public string HairColours { get; set; }

        [JsonProperty("skin_colors")]
        public string SkinColours { get; set; }

        [JsonProperty("people")]
        public List<string> PeopleURLs { get; set; }

        public List<PersonModel> People { get; set; } = new List<PersonModel>();

        [JsonProperty("homeworld")]
        public string HomeworldURL { get; set; }

        public PlanetModel Homeworld { get; set; }

        [JsonProperty("films")]
        public List<string> FilmURLs { get; set; }

        public List<FilmModel> Films { get; set; } = new List<FilmModel>();

        public List<string> GenerateFullDisplayList()
        {
            List<string> returnList = new List<string>();
            returnList.Add("");
            returnList.Add(string.Format("### {0} ###", Name));
            returnList.Add(string.Format("Classification: {0} | Designation: {1} | Language: {2}", Classification, Designation, Language));
            returnList.Add(string.Format("Hair Colours: {0} | Average Height: {1}cm ", HairColours, AverageHeight));
            returnList.Add(string.Format("Skin Colours: {0} | Average Lifespan: {1}years", SkinColours, AverageLifespan, Language));
            returnList.Add(string.Format("Eye Colours: {0}| Language: {1}", EyeColours, Language));
            //
            // 
            returnList.Add("");
            returnList.Add("### Homeworld ###");
            returnList.Add(Homeworld.BriefDisplayTitle);


            if (People.Count > 0)
            {
                returnList.Add("");
                returnList.Add("### Notable People ###");
                string peopleString = null;
                for (int i = 0; i < People.Count; i++)
                {
                    if (peopleString == null)
                    {
                        peopleString = People[i].BriefDisplayTitle;
                    }
                    else
                    {
                        peopleString = string.Format("{0} # {1}", peopleString, People[i].BriefDisplayTitle);

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


    }
}
