using APIHandler;
using APIHandler.Enums;
using APIHandler.Exceptions;
using APIHandler.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace APIUsageChallengeConsole
{
    class Program
    {
        private static IStarWarsAPIModel loadedSearch;

        private static StarWarsSearchEnum? selectedSearch;

        private static string enteredSearch = null;

        private static bool validSearch = false;

        private static bool loading = false;

        static void Main(string[] args)
        {
            while (true)
            {
                if (!loading)
                {
                    PromptSearch();

                    MainAsync().Wait();
                    if (validSearch)
                    {
                        DisplayLoadedInformation();
                    } 
                }
            }
        }



        static async Task MainAsync()
        {
            try
            {
                //Start API
                APIHelper.StartClient();
                validSearch = true;
                loading = true;
                int searchInt = Convert.ToInt32(enteredSearch);

                if (selectedSearch == StarWarsSearchEnum.Person)
                {
                    await LoadPerson(searchInt);
                }
                else if (selectedSearch == StarWarsSearchEnum.Planet)
                {

                    await LoadPlanet(searchInt);


                }
                else if (selectedSearch == StarWarsSearchEnum.Species)
                {

                    await LoadSpecies(searchInt);

                }
                else if (selectedSearch == StarWarsSearchEnum.Films)
                {

                    await LoadFilms(searchInt);

                }
                else if (selectedSearch == StarWarsSearchEnum.Starships)
                {

                    await LoadStarships(searchInt);

                }
                else if (selectedSearch == StarWarsSearchEnum.Vehicles)
                {

                    await LoadVehicles(searchInt);

                }
                loading = false;
            }
            catch (NoRecordFoundException ex)
            {
                Console.WriteLine("No record found!");
                validSearch = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error has occured and could not be stopped.");

                validSearch = false;

            }
            loading = false;
        }

        private static async Task LoadPerson(int personID = 0)
        {
            var person = await APIProcessor.LoadPerson(personID);

            if (person != null)
            {

                loadedSearch = person;
            }
        }

        private static async Task LoadPlanet(int planetID = 0)
        {
            var planet = await APIProcessor.LoadPlanet(planetID);

            if (planet != null)
            {
                loadedSearch = planet;
            }
        }

        private static async Task LoadSpecies(int speciesID = 0)
        {
            var species = await APIProcessor.LoadSpecies(speciesID);

            if (species != null)
            {
                loadedSearch = species;
            }
        }

        private static async Task LoadVehicles(int vehicleID = 0)
        {
            var vehicles = await APIProcessor.LoadVehicle(vehicleID);

            if (vehicles != null)
            {
                loadedSearch = vehicles;
            }
        }


        private static async Task LoadStarships(int starshipID = 0)
        {
            var starships = await APIProcessor.LoadStarship(starshipID);

            if (starships != null)
            {
                loadedSearch = starships;
            }
        }

        private static async Task LoadFilms(int filmID = 0)
        {
            var films = await APIProcessor.LoadFilm(filmID);

            if (films != null)
            {
                loadedSearch = films;
            }
        }

        private static void DisplayLoadedInformation()
        {


            if (loadedSearch != null && loadedSearch.FullDisplayList != null)
            {
                foreach (string item in loadedSearch.FullDisplayList)
                {
                    Console.WriteLine(item);
                }

                Console.WriteLine();
            }




        }

        private static void PromptSearch()
        {
            string selectedSearchString = null;

            Console.WriteLine("Please select a search: 1 = Persons | 2 = Planets | 3 = Species | 4 = Vehicles  5 = Starships | 6 = Films");

            selectedSearch = null;
            while (selectedSearch == null)
            {
                if (CheckStringUnder10Chars(selectedSearchString))
                {

                    if (!string.IsNullOrEmpty(selectedSearchString))
                    {
                        Console.WriteLine("Please selected a valid search!");

                    }
                    selectedSearchString = Console.ReadLine();

                    if (selectedSearchString == "1")
                    {
                        selectedSearch = StarWarsSearchEnum.Person;
                    }
                    else if (selectedSearchString == "2")
                    {
                        selectedSearch = StarWarsSearchEnum.Planet;
                    }
                    else if (selectedSearchString == "3")
                    {
                        selectedSearch = StarWarsSearchEnum.Species;
                    }
                    else if (selectedSearchString == "4")
                    {
                        selectedSearch = StarWarsSearchEnum.Vehicles;
                    }
                    else if (selectedSearchString == "5")
                    {
                        selectedSearch = StarWarsSearchEnum.Starships;
                    }
                    else if (selectedSearchString == "6")
                    {
                        selectedSearch = StarWarsSearchEnum.Films;
                    }
                }
                else
                {
                    Console.WriteLine("Please enter a valid input!");
                }
            }




            Console.WriteLine("Please enter your search:");


            enteredSearch = null;
            while (enteredSearch == null)
            {

                enteredSearch = Console.ReadLine();
                int x = 0;

                if (!CheckStringUnder10Chars(enteredSearch))
                {
                    Console.WriteLine("Please enter a valid input!");
                    enteredSearch = null;
                }
                else if (!Int32.TryParse(enteredSearch, out x))
                {
                    Console.WriteLine("Please enter a numeric value!");
                    enteredSearch = null;
                }

            }
            Console.WriteLine("Searching...");


        }

        private static bool CheckStringUnder10Chars(string selectedSearchString)
        {
            if (selectedSearchString != null && selectedSearchString.Count() > 9)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
