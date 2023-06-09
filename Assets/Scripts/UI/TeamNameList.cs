using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SAS.Managers;
using SAS.Models;
using TMPro;
using System;

namespace SAS.UI
{
    public class TeamNameList : MonoBehaviour
    {
        #region Fields

        private RectTransform m_rectTransform;

        private TeamModel m_model;
        
        [SerializeField]
        private GameObject nameOutputTemplate;

        [SerializeField]
        private TMP_InputField teamNameInput;
        
        #endregion

        #region Properties

        public TeamModel Model
        {
            get { return m_model; }
            set
            {
                if (m_model == value)
                    return;

                m_model = value;
            }
        }

        #endregion

        #region Methods

        // Start is called before the first frame update
        void Awake()
        {
            m_rectTransform = GetComponent<RectTransform>();

            if (teamNameInput != null)
            {
                teamNameInput.onEndEdit.AddListener(UpdateTeamName);
            }
        }

        /// <summary>
        /// Listener for when the current name input is finished
        /// </summary>
        /// <param name="input"></param>
        public void UpdateTeamName(string input)
        {
            // Transfer the listeners to the next game input
            if (!string.IsNullOrEmpty(input))
            {
                // Update the name of the category
                m_model.Name = teamNameInput.text = input;
            }
            // Reset the category name to its previous state
            else
            {
                teamNameInput.text = m_model.Name;
            }
        }

        public void AddPlayers(List<PlayerModel> players)
        {
            foreach (var player in players)
            {
                // Instantiate a player name output
                var nameOutput = Instantiate(nameOutputTemplate, transform);
                nameOutput.GetComponent<PlayerView>().Model = player;
                nameOutput.GetComponentInChildren<TextMeshProUGUI>().text = player.Name;
            }
        }
        
        public IEnumerator Redraw()
        {
            yield return new WaitForEndOfFrame();
            LayoutRebuilder.MarkLayoutForRebuild(m_rectTransform);
        }
   
        [ContextMenu("Print Players")]
        public void PrintPlayers()
        {
            Debug.LogFormat("DEBUG... Team name: {0}, number players: {1}", m_model.Name, m_model.Players.Count);
            foreach (var player in m_model.Players)
            {
                Debug.LogFormat("DEBUG... Player name: {0}", player.Name);
            }

            Debug.LogFormat("DEBUG... TeamManager number of teams: {0}, team manager contain model: {1}", TeamManager.Instance.Teams.Count, TeamManager.Instance.Teams.Contains(m_model));
        }

        #endregion
    }
}
