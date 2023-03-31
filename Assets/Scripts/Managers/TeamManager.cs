using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SAS.Models;

using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        // Example usage
        List<(string, string)> players = new List<(string, string)> {("Alice", "A"), ("Bob", "B"), ("Charlie", "C"), ("David", "A"), ("Eve", "B"), ("Frank", "C"), ("Grace", "A"), ("Heidi", "B")};
        int numTeams = 2;
        List<List<(string, string)>> teams = GenerateTeamsWithCategories(players, numTeams);
        foreach (List<(string, string)> team in teams)
        {
            Console.WriteLine(String.Join(", ", team.Select(p => p.Item1)));
        }
    }

    static List<List<(string, string)>> GenerateTeamsWithCategories(List<(string, string)> players, int numTeams)
    {
        // Create a dictionary of players grouped by category
        Dictionary<string, List<string>> categories = new Dictionary<string, List<string>>();
        foreach ((string player, string category) in players)
        {
            if (!categories.ContainsKey(category))
            {
                categories[category] = new List<string>();
            }
            categories[category].Add(player);
        }

        // Shuffle the players in each category
        Random rng = new Random();
        foreach (List<string> playerList in categories.Values)
        {
            Shuffle(playerList, rng);
        }

        // Divide the players into teams, ensuring that each team has players from every category
        List<List<(string, string)>> teams = new List<List<(string, string)>>();
        for (int i = 0; i < numTeams; i++)
        {
            teams.Add(new List<(string, string)>());
        }

        int numCategories = categories.Count;
        int[] categoryIndices = Enumerable.Range(0, numCategories).ToArray();
        Shuffle(categoryIndices, rng);
        int teamIndex = 0;
        foreach (int categoryIndex in categoryIndices)
        {
            string category = categories.Keys.ElementAt(categoryIndex);
            List<string> categoryPlayers = categories[category];
            int numPlayers = categoryPlayers.Count;
            int teamSize = numPlayers / numTeams;
            int remainder = numPlayers % numTeams;
            int startIndex = 0;
            for (int i = 0; i < numTeams; i++)
            {
                int endIndex = startIndex + teamSize + (i < remainder ? 1 : 0); // add one extra player to the first 'remainder' teams
                List<(string, string)> team = teams[(teamIndex + i) % numTeams];
                for (int j = startIndex; j < endIndex; j++)
                {
                    team.Add((categoryPlayers[j], category));
                }
                startIndex = endIndex;
            }
            teamIndex = (teamIndex + remainder) % numTeams;
        }

        // Shuffle the order of the teams
        Shuffle(teams, rng);

        return teams;
    }

    static void Shuffle<T>(List<T> list, Random rng)
    {
        // Fisher-Yates shuffle algorithm
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T temp = list[k];
            list[k] = list[n];
            list[n] = temp;
        }
    }

    static void Shuffle<T>(IList<T> list, Random rng)
    {
        // Fisher-Yates shuffle algorithm
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}


namespace SAS.Managers
{
    public class TeamManager : BaseMonoSingleton<TeamManager>
    {
        #region Properties

        List<TeamModel> Teams { get; set; }

        #endregion

        #region Methods

        // Start is called before the first frame update
        void Start()
        {
            Teams = new List<TeamModel>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// Generate teams of specified size according to random numbers assigned
        /// to each player within a category
        /// </summary>
        /// <returns>Number of teams</returns>
        public int GenerateTeams()
        {
            System.Random rng = new System.Random();

            List<CategoryModel> categories = CategoryManager.Instance.Categories;

            // Give each player in each category a random number between 1 and 100 
            foreach (var category in categories)
            {
                foreach (var player in category.Players)
                {
                    player.RandomNumberSeed = rng.Next(1, 101);
                }

                // Sort the players by random number
                List<PlayerModel> sortedList = category.Players.OrderBy(x => x.RandomNumberSeed).ToList();
                category.Players = sortedList;
            }


            return 0;
        }

        #endregion
    }
}

