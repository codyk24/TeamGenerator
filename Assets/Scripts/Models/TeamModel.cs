using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SAS.Models
{
    public class TeamModel
    {
        #region Properties

        public string Name;

        public List<PlayerModel> Players = new List<PlayerModel>();

        public int Size => Players.Count;

        #endregion

        #region Methods

        public bool FindPlayer(string name, out PlayerModel player)
        {
            player = Players.Find(x => string.Equals(x.Name, name));
            return player != null;
        }

        public double FindAverageSkillLevel()
        {
            double totalSkillLevel = 0;
            foreach (var player in Players)
            {
                //totalSkillLevel += player.SkillLevel;
            }

            return totalSkillLevel / Players.Count;
        }

        public void AddPlayer(PlayerModel player)
        {
            Players.Add(player);
        }

        public bool RemovePlayer(PlayerModel player)
        {
            return Players.Remove(player);
        }

        public void ClearPlayers()
        {
            Players.Clear();
        }

        public void Print()
        {
            string teamString = string.Format("{0}: ", Name);
            foreach (var player in Players)
            {
                teamString += player.Name + ", \n";
            }

            Debug.LogFormat(teamString);
        }

        #endregion
    }
}
