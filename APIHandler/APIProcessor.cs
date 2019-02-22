using APIHandler.DataModels;
using APIHandler.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace APIHandler
{
    public class APIProcessor
    {
        /// <summary>
        /// List of persons previously searched for in session
        /// </summary>
        private static List<PersonModel> LoadedPersons = new List<PersonModel>();

        /// <summary>
        /// List of planets previously searched for in session
        /// </summary>
        private static List<PlanetModel> LoadedPlanets { get; set; } = new List<PlanetModel>();

        /// <summary>
        /// List of species previously searched for in session
        /// </summary>
        private static List<SpeciesModel> LoadedSpecies = new List<SpeciesModel>();

        /// <summary>
        /// List of films previously searched for in session
        /// </summary>
        private static List<FilmModel> LoadedFilms = new List<FilmModel>();

        /// <summary>
        /// List of vehicles previously searched for in session
        /// </summary>
        private static List<VehicleModel> LoadedVehicles { get; set; } = new List<VehicleModel>();

        /// <summary>
        /// List of starships previously searched for in session
        /// </summary>
        private static List<StarshipModel> LoadedStarships { get; set; } = new List<StarshipModel>();


        /// <summary>
        /// Returns an ID from a URL
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static int GetIDFromURL(string url)
        {
            List<string> urlSplit = url.Split('/').ToList();

            if (urlSplit.Count > 4)
            {
                int personID = int.Parse(urlSplit[5]);
                return personID;
            }
            else
            {
                return 0;
            }
        }


        /// <summary>
        /// Load a Person by providing their Person ID
        /// </summary>
        /// <param name="personID"></param>
        /// <returns>Returns a Person Model populated automatically by APIClient</returns>
        public static async Task<PersonModel> LoadPerson(int personID)
        {
            string url = string.Format(@"https://swapi.co/api/people/{0}/", personID.ToString());

            return await LoadPerson(url);
        }

        /// <summary>
        /// Load persons by providing the URLs
        /// </summary>
        /// <param name="urls">Person URL</param>
        /// <returns>Returns a Person Model populated automatically by APIClient</returns>
        public static async Task<List<PersonModel>> LoadPerson(List<string> urls)
        {
            List<PersonModel> persons = new List<PersonModel>();

            foreach (var url in urls)
            {
                persons.Add(await LoadPerson(url));
            }
            return persons;
        }


        /// <summary>
        /// Load a Person by providing the URL
        /// </summary>
        /// <param name="url">Person URL</param>
        /// <returns>Returns a Person Model populated automatically by APIClient</returns>
        private static async Task<PersonModel> LoadPerson(string url)
        {
            int personID = GetIDFromURL(url);

            PersonModel person = await CallAPIForPerson(url, personID);

            if (!person.Loaded)
            {

                string speciesURL = person.SpeciesUrls.First();
                person.Species = await CallAPIForSpecies(speciesURL, GetIDFromURL(speciesURL));

                person.Homeworld = await CallAPIForPlanet(person.HomeworldUrl, GetIDFromURL(person.HomeworldUrl));

                foreach (var item in person.VehiclesUrls)
                {
                    VehicleModel vehicle = await CallAPIForVehicle(item, GetIDFromURL(item));
                    person.Vehicles.Add(vehicle);
                }

                foreach (var item in person.StarshipsUrls)
                {
                    StarshipModel starship = await CallAPIForStarship(item, GetIDFromURL(item));
                    person.Starships.Add(starship);
                }

                foreach (var item in person.FilmURLs)
                {
                    FilmModel film = await CallAPIForFilms(item, GetIDFromURL(item));
                    person.Films.Add(film);
                }

                person.Loaded = true;

                PersonModel tempPerson = LoadedPersons.Single(x => x.ID == person.ID);

                LoadedPersons.Remove(tempPerson);
                LoadedPersons.Add(person);

            }


            return person;
        }


        private static async Task<PersonModel> CallAPIForPerson(string url, int personID)
        {


            //Check to see if previously loaded
            PersonModel personCheck = LoadedPersons.Where(x => x.ID == personID).FirstOrDefault();

            if (personCheck != null)
            {
                return personCheck;
            }
            else if (personID > 0)
            {
                using (HttpResponseMessage response = await APIHelper.APIClient.GetAsync(url))
                {

                    if (response.IsSuccessStatusCode)
                    {
                        PersonModel person = await response.Content.ReadAsAsync<PersonModel>();
                        person.ID = personID;
                        LoadedPersons.Add(person);
                        return person;
                    }
                    else
                    {
                        //If unsuccessful throw an exception with reason
                        throw new NoRecordFoundException(response.ReasonPhrase);
                    }
                }
            }
            else
            {
                throw new Exception("No PlayerID Provided!");
            }


        }





        /// <summary>
        /// Loading a Planet by providing their Planet ID
        /// </summary>
        /// <param name="planetID"></param>
        /// <returns>Returns a Planet Model populated automatically by APIClient</returns>
        public static async Task<PlanetModel> LoadPlanet(int planetID)
        {
            string url = string.Format(@"https://swapi.co/api/planets/{0}/", planetID.ToString());

            return await LoadPlanet(url);
        }

        /// <summary>
        /// Load Planets by providing the URLs
        /// </summary>
        /// <param name="urls">Planet URL</param>
        /// <returns>Returns a Planet Model populated automatically by APIClient</returns>
        private static List<PlanetModel> LoadPlanet(List<string> urls)
        {
            List<PlanetModel> planets = new List<PlanetModel>();

            foreach (var url in urls)
            {
                planets.Add(LoadPlanet(url).Result);
            }
            return planets;
        }


        /// <summary>
        /// Loading a Planet by providing the URL
        /// </summary>
        /// <param name="url">Planet URL</param>
        /// <returns>Returns a Planet Model populated automatically by APIClient</returns>
        private static async Task<PlanetModel> LoadPlanet(string url)
        {
            int planetID = GetIDFromURL(url);

            PlanetModel planet = await CallAPIForPlanet(url, planetID);

            if (!planet.Loaded)
            {
                foreach (var item in planet.ResidentsURLS)
                {
                    planet.Residents.Add(await CallAPIForPerson(item, GetIDFromURL(item)));
                }

                foreach (var item in planet.FilmsURLs)
                {
                    planet.Films.Add(await CallAPIForFilms(item, GetIDFromURL(item)));
                }

                PlanetModel tempPlanet = LoadedPlanets.Single(x => x.ID == planetID);
                planet.Loaded = true;

                LoadedPlanets.Remove(tempPlanet);
                LoadedPlanets.Add(planet);

            }
            return planet;
        }


        private static async Task<PlanetModel> CallAPIForPlanet(string url, int planetID)
        {

            //Check to see if previously loaded
            PlanetModel planetCheck = LoadedPlanets.Where(x => x.ID == planetID).FirstOrDefault();

            if (planetCheck != null)
            {
                return planetCheck;
            }
            else if (planetID > 0)
            {
                using (HttpResponseMessage response = await APIHelper.APIClient.GetAsync(url))
                {

                    if (response.IsSuccessStatusCode)
                    {
                        PlanetModel planet = await response.Content.ReadAsAsync<PlanetModel>();
                        planet.ID = planetID;
                        LoadedPlanets.Add(planet);

                        return planet;
                    }
                    else
                    {
                        //If unsuccessful throw an exception with reason
                        throw new NoRecordFoundException(response.ReasonPhrase);
                    }
                }
            }
            else
            {
                throw new Exception("No PlanetID Provided!");
            }

        }



        /// <summary>
        /// Loading a Species by providing their Species ID
        /// </summary>
        /// <param name="speciesID"></param>
        /// <returns>Returns a Species Model populated automatically by APIClient</returns>
        public static async Task<SpeciesModel> LoadSpecies(int speciesID)
        {
            string url = string.Format(@"https://swapi.co/api/species/{0}/", speciesID.ToString());

            return await LoadSpecies(url);
        }

        /// <summary>
        /// Load species by providing the URLs
        /// </summary>
        /// <param name="urls">species URL</param>
        /// <returns>Returns a species Model populated automatically by APIClient</returns>
        private static async Task<List<SpeciesModel>> LoadSpecies(List<string> urls)
        {
            List<SpeciesModel> species = new List<SpeciesModel>();

            foreach (var url in urls)
            {
                species.Add(await LoadSpecies(url));
            }
            return species;
        }

        /// <summary>
        /// Loading a Species by providing their Species ID
        /// </summary>
        /// <param name="speciesID"></param>
        /// <returns>Returns a Species Model populated automatically by APIClient</returns>
        private static async Task<SpeciesModel> LoadSpecies(string url)
        {
            int speciesID = GetIDFromURL(url);

            SpeciesModel species = await CallAPIForSpecies(url, speciesID);

            if (!species.Loaded)
            {
                species.Homeworld = await CallAPIForPlanet(species.HomeworldURL, GetIDFromURL(species.HomeworldURL));




                foreach (var item in species.PeopleURLs)
                {
                    PersonModel person = await CallAPIForPerson(item, GetIDFromURL(item));
                    species.People.Add(person);
                }



                foreach (var item in species.FilmURLs)
                {
                    FilmModel film = await CallAPIForFilms(item, GetIDFromURL(item));
                    species.Films.Add(film);
                }


                species.Loaded = true;
                SpeciesModel tempSpecies = LoadedSpecies.Single(x => x.ID == speciesID);

                LoadedSpecies.Remove(tempSpecies);
                LoadedSpecies.Add(tempSpecies);
            }

            return species;

        }



        private static async Task<SpeciesModel> CallAPIForSpecies(string url, int speciesID)
        {

            //Check to see if previously loaded
            SpeciesModel speciesCheck = LoadedSpecies.Where(x => x.ID == speciesID).FirstOrDefault();

            if (speciesCheck != null)
            {
                return speciesCheck;
            }
            else if (speciesID > 0)
            {
                using (HttpResponseMessage response = await APIHelper.APIClient.GetAsync(url))
                {

                    if (response.IsSuccessStatusCode)
                    {
                        SpeciesModel species = await response.Content.ReadAsAsync<SpeciesModel>();
                        species.ID = speciesID;

                        LoadedSpecies.Add(species);

                        return species;
                    }
                    else
                    {
                        //If unsuccessful throw an exception with reason
                        throw new NoRecordFoundException(response.ReasonPhrase);
                    }
                }
            }
            else
            {
                throw new Exception("No SpeciesID Provided!");
            }

        }

        /// <summary>
        /// Load a Film by providing their Film ID
        /// </summary>
        /// <param name="filmID"></param>
        /// <returns>Returns a Film Model populated automatically by APIClient</returns>
        public static async Task<FilmModel> LoadFilm(int filmID)
        {
            string url = string.Format(@"https://swapi.co/api/films/{0}/", filmID.ToString());

            return await LoadFilm(url);
        }

        /// <summary>
        /// Load films by providing the URLs
        /// </summary>
        /// <param name="urls">Film URL</param>
        /// <returns>Returns a Films Model populated automatically by APIClient</returns>
        public static async Task<List<FilmModel>> LoadFilm(List<string> urls)
        {
            List<FilmModel> films = new List<FilmModel>();

            foreach (var url in urls)
            {
                films.Add(await LoadFilm(url));
            }
            return films;
        }

        /// <summary>
        /// Load a Film by providing the URL
        /// </summary>
        /// <param name="url">Film URL</param>
        /// <returns>Returns a Film Model populated automatically by APIClient</returns>
        private static async Task<FilmModel> LoadFilm(string url)
        {
            int filmID = GetIDFromURL(url);

            FilmModel film = await CallAPIForFilms(url, filmID);

            if (!film.Loaded)
            {
                foreach (var item in film.PlanetURLs)
                {
                    PlanetModel planet = await CallAPIForPlanet(item, GetIDFromURL(item));
                    film.Planets.Add(planet);
                }

                foreach (var item in film.PersonURLs)
                {
                    PersonModel person = await CallAPIForPerson(item, GetIDFromURL(item));
                    film.Persons.Add(person);
                }

                foreach (var item in film.SpeciesURLs)
                {
                    SpeciesModel species = await CallAPIForSpecies(item, GetIDFromURL(item));
                    film.Species.Add(species);
                }

                foreach (var item in film.VehicleURLs)
                {
                    VehicleModel vehicle = await CallAPIForVehicle(item, GetIDFromURL(item));
                    film.Vehicles.Add(vehicle);
                }

                foreach (var item in film.StarshipURLs)
                {
                    StarshipModel starships = await CallAPIForStarship(item, GetIDFromURL(item));
                    film.Starships.Add(starships);
                }



                film.Loaded = true;

                FilmModel tempFilm = LoadedFilms.Single(x => x.ID == filmID);

                LoadedFilms.Remove(tempFilm);
                LoadedFilms.Add(tempFilm);

            }

            return film;
        }



        private static async Task<FilmModel> CallAPIForFilms(string url, int filmID)
        {

            //Check to see if previously loaded
            FilmModel filmCheck = LoadedFilms.Where(x => x.ID == filmID).FirstOrDefault();

            if (filmCheck != null)
            {
                return filmCheck;
            }
            else if (filmID > 0)
            {
                using (HttpResponseMessage response = await APIHelper.APIClient.GetAsync(url))
                {

                    if (response.IsSuccessStatusCode)
                    {
                        FilmModel film = await response.Content.ReadAsAsync<FilmModel>();
                        film.ID = filmID;
                        LoadedFilms.Add(film);

                        return film;
                    }
                    else
                    {
                        //If unsuccessful throw an exception with reason
                        throw new NoRecordFoundException(response.ReasonPhrase);
                    }
                }
            }
            else
            {
                throw new Exception("No Film ID Provided!");
            }

        }

        /// <summary>
        /// Load a Vehicle by providing their Vehicle ID
        /// </summary>
        /// <param name="vehicleID"></param>
        /// <returns>Returns a Vehicle Model populated automatically by APIClient</returns>
        public static async Task<VehicleModel> LoadVehicle(int vehicleID)
        {
            string url = string.Format(@"https://swapi.co/api/vehicles/{0}/", vehicleID.ToString());

            return await LoadVehicle(url);
        }

        /// <summary>
        /// Load persons by providing the URLs
        /// </summary>
        /// <param name="urls">Person URL</param>
        /// <returns>Returns a Person Model populated automatically by APIClient</returns>
        public static async Task<List<VehicleModel>> LoadVehicle(List<string> urls)
        {
            List<VehicleModel> vehicles = new List<VehicleModel>();

            foreach (var url in urls)
            {
                vehicles.Add(await LoadVehicle(url));
            }
            return vehicles;
        }

        /// <summary>
        /// Load a Vehicle by providing the URL
        /// </summary>
        /// <param name="url">Vehicle URL</param>
        /// <returns>Returns a Vehicle Model populated automatically by APIClient</returns>
        private static async Task<VehicleModel> LoadVehicle(string url)
        {
            int vehicleID = GetIDFromURL(url);

            VehicleModel vehicle = await CallAPIForVehicle(url, vehicleID);

            if (!vehicle.Loaded)
            {
                foreach (var item in vehicle.FilmURLs)
                {
                    vehicle.Films.Add(await CallAPIForFilms(item, GetIDFromURL(item)));
                }

                foreach (var item in vehicle.PilotURLs)
                {
                    vehicle.Pilots.Add(await CallAPIForPerson(item, GetIDFromURL(item)));
                }

                vehicle.Loaded = true;

                VehicleModel tempVehicle = LoadedVehicles.Single(x => x.ID == vehicleID);

                LoadedVehicles.Remove(tempVehicle);
                LoadedVehicles.Add(vehicle);
            }

            return vehicle;
        }

        private static async Task<VehicleModel> CallAPIForVehicle(string url, int vehicleID)
        {

            //Check to see if previously loaded
            VehicleModel vehicleCheck = LoadedVehicles.Where(x => x.ID == vehicleID).FirstOrDefault();

            if (vehicleCheck != null)
            {
                return vehicleCheck;
            }
            else if (vehicleID > 0)
            {
                using (HttpResponseMessage response = await APIHelper.APIClient.GetAsync(url))
                {

                    if (response.IsSuccessStatusCode)
                    {
                        VehicleModel vehicle = await response.Content.ReadAsAsync<VehicleModel>();
                        vehicle.ID = vehicleID;

                        LoadedVehicles.Add(vehicle);
                        return vehicle;
                    }
                    else
                    {

                        throw new NoRecordFoundException(response.ReasonPhrase);
                    }
                }
            }
            else
            {
                throw new Exception("No Vehicle ID Provided!");
            }

        }


        /// <summary>
        /// Load a Starship by providing their Starship ID
        /// </summary>
        /// <param name="starshipID"></param>
        /// <returns>Returns a Starship Model populated automatically by APIClient</returns>
        public static async Task<StarshipModel> LoadStarship(int starshipID)
        {
            string url = string.Format(@"https://swapi.co/api/starships/{0}/", starshipID.ToString());

            return await LoadStarship(url);
        }

        /// <summary>
        /// Load starships by providing the URLs
        /// </summary>
        /// <param name="urls">starships URL</param>
        /// <returns>Returns a starship Model populated automatically by APIClient</returns>
        public static async Task<List<StarshipModel>> LoadStarship(List<string> urls)
        {
            List<StarshipModel> starships = new List<StarshipModel>();

            foreach (var url in urls)
            {
                starships.Add(LoadStarship(url).Result);
            }
            return starships;
        }

        /// <summary>
        /// Load a Starship by providing the URL
        /// </summary>
        /// <param name="url">Starship URL</param>
        /// <returns>Returns a Starship Model populated automatically by APIClient</returns>
        private static async Task<StarshipModel> LoadStarship(string url)
        {
            int starshipID = GetIDFromURL(url);

            StarshipModel starship = await CallAPIForStarship(url, starshipID);

            if (!starship.Loaded)
            {
                foreach (var item in starship.FilmURLs)
                {
                    starship.Films.Add(await CallAPIForFilms(item, GetIDFromURL(item)));
                }

                foreach (var item in starship.PilotURLs)
                {
                    starship.Pilots.Add(await CallAPIForPerson(item, GetIDFromURL(item)));
                }

                starship.Loaded = true;

                VehicleModel tempStarship = LoadedStarships.Single(x => x.ID == starshipID);

                LoadedVehicles.Remove(tempStarship);
                LoadedVehicles.Add(starship);
            }

            return starship;
        }

        private static async Task<StarshipModel> CallAPIForStarship(string url, int starshipID)
        {

            //Check to see if previously loaded
            StarshipModel starshipCheck = LoadedStarships.Where(x => x.ID == starshipID).FirstOrDefault();

            if (starshipCheck != null)
            {
                return starshipCheck;
            }
            else if (starshipID > 0)
            {
                using (HttpResponseMessage response = await APIHelper.APIClient.GetAsync(url))
                {

                    if (response.IsSuccessStatusCode)
                    {
                        StarshipModel starship = await response.Content.ReadAsAsync<StarshipModel>();
                        starship.ID = starshipID;

                        LoadedStarships.Add(starship);
                        return starship;
                    }
                    else
                    {

                        throw new NoRecordFoundException(response.ReasonPhrase);
                    }
                }
            }
            else
            {
                throw new Exception("No Starship ID Provided!");
            }

        }
    }
}
