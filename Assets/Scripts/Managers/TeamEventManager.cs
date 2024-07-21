using System.Collections;
using System.Collections.Generic;
using SAS.Models;
using SAS.Managers;
using UnityEngine;

namespace SAS.Managers
{
    public class TeamEventManager : BaseMonoSingleton<TeamEventManager>
    {
        #region Members

        private EventModel m_eventModel;

        private string m_eventName;

        #endregion

        #region Properties

        public EventModel EventModel => m_eventModel;

        public string EventName => m_eventName;

        #endregion

        #region Methods

        // Start is called before the first frame update
        void Start()
        {
            m_eventModel = new();
        }

        public void SetEventName(string eventName)
        {
            Debug.LogFormat("DEBUG... Setting event name to: {0}", eventName);
            m_eventModel.Name = eventName;
        }

        public void SaveEventJson()
        {
            m_eventModel.Teams = TeamManager.Instance.Teams;
            m_eventModel.Categories = CategoryManager.Instance.Categories;
            FileDataHandler.SaveEventJson();
        }

        public void LoadEventModel(EventModel eventModel)
        {
            // Clear any teams and categories currently created
            TeamManager.Instance.ResetTeams();
            CategoryManager.Instance.ClearCategories();

            Debug.LogFormat("DEBUG... TeamEventManager.LoadEvent... eventModel categories: {0}, teams: {1}", eventModel.Categories.Count, eventModel.Teams.Count);
            m_eventModel = eventModel;
            m_eventName = eventModel.Name;

            // Populate the categories
            CategoryManager.Instance.SetCategoryList(eventModel.Categories);

            // Populate the teams
            TeamManager.Instance.SetTeamList(eventModel.Teams);
        }

        #endregion
    }
}
