using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SAS.Models
{
    public class EventModel
    {
        #region Properties

        public string Name;
        public List<TeamModel> Teams;
        public List<CategoryModel> Categories;

        #endregion

        #region Constructors

        public EventModel()
        {
            Name = string.Empty;
            Teams = new List<TeamModel>();
            Categories = new List<CategoryModel>();
        }

        #endregion

        public void Print()
        {
            Debug.LogFormat("DEBUG... Event Name: {0}, number of teams: {1}", Name, Teams.Count);
        }
    }
}
