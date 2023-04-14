using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SAS.Models
{
    public class PlayerModel
    {
        #region Properties

        public int SkillLevel { get; set; }

        public string Name { get; set; }

        public CategoryModel Category { get; set; }

        public int RandomNumberSeed { get; set; }

        #endregion

        #region Constructors

        public PlayerModel() { }

        public PlayerModel(string name)
        {
            Name = name;
        }

        #endregion
    }
}

