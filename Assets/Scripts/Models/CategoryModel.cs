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

        #region Methods
        public bool FindPlayer(string name, out PlayerModel player)
        {
            player = Players.Find(x => string.Equals(x.Name, name));
            return player != null;
        }

        #endregion
    }
}

