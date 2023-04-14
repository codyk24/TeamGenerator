using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SAS.Models
{
    public class CategoryModel
    {
        #region Properties

        public string Name { get; set; }
        public List<PlayerModel> Players { get; set; }
        public int CategorySize => Players.Count;

        #endregion

        #region Constructors

        public CategoryModel()
        {
            Players = new List<PlayerModel>();
        }

        #endregion

        #region Methods
        public bool FindPlayer(string name, out PlayerModel player)
        {
            player = Players.Find(x => string.Equals(x.Name, name));
            return player != null;
        }

        public void Print()
        {
            Debug.LogFormat("DEBUG... Category Name: {0}, number of players: {1}", Name, Players.Count);
            foreach(var player in Players)
            {
                Debug.LogFormat("Player: {0}", player.Name);
            }
        }

        #endregion
    }
}

