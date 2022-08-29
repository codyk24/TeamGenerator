using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SAS.Models;

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

