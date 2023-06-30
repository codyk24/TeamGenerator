using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SAS.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class PlayerModel
    {
        #region Properties

        public int SkillLevel { get; set; }

        [JsonProperty]
        public string Name;

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

