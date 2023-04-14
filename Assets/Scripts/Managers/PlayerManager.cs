using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SAS.Models;

namespace SAS.Managers
{
    public class PlayerManager : BaseMonoSingleton<PlayerManager>
    {
        #region Properties

        public List<PlayerModel> Players { get; set; } = new List<PlayerModel>();

        #endregion

        #region Methods

        // Start is called before the first frame update
        protected override void Awake()
        {
            base.Awake();
            //Players = new List<PlayerModel>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void AddPlayer(PlayerModel player)
        {
            Players.Add(player);
        }

        public bool RemovePlayer(PlayerModel player)
        {
            return Players.Remove(player);
        }

        public void PrintPlayers()
        {
            Debug.LogFormat("DEBUG... Number of players: {0}", Players.Count);
            foreach(var player in Players)
            {
                Debug.LogFormat("DEBUG... Player name: {0}", player.Name);
            }
        }

        #endregion
    }
}

