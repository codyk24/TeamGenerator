using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SAS.Models;

namespace SAS.Managers
{
    public class PlayerManager : BaseMonoSingleton<PlayerManager>
    {
        #region Properties

        List<PlayerModel> Players { get; set; }

        #endregion

        #region Methods

        // Start is called before the first frame update
        void Start()
        {
            Players = new List<PlayerModel>();
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

        #endregion
    }
}

