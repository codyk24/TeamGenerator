using SAS.Managers;
using SAS.Models;
using SAS.UI;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class TeamOutputList : MonoBehaviour
{
    #region Members

    [SerializeField]
    private GameObject m_teamPanel;
    
    [SerializeField]
    private GameObject m_teamNameListTemplate;

    #endregion

    #region Methods

    private void Awake()
    {
        TeamManager.Instance.TeamListChanged += TeamsChanged;
    }

    public void ClearTeamList()
    {
        // Clear the content from the scroll view
        for (int i = 0; i < m_teamPanel.transform.childCount; i++)
        {
            GameObject child = m_teamPanel.transform.GetChild(i).gameObject;
            Destroy(child);
        }
    }

    public void RegenerateTeams()
    {
        // Clear the players from each team in the TeamManager
        TeamManager.Instance.ClearTeamPlayerLists();

        ClearTeamList();

        // Call the GenerateTeams function again
        var teams = TeamManager.Instance.GenerateTeamsWithCategories(
            CategoryManager.Instance.Categories,
            TeamManager.Instance.Teams.Count);

        var orderedTeams = teams.OrderBy(team => team.Name);
        UpdateTeamsList(orderedTeams);

        TeamEventManager.Instance.SaveEventJson();
    }

    // Handler to populate the teams list from external changes to the team manager
    // e.g. Loading an event from disk
    private void TeamsChanged(object sender, TeamListEventArgs e)
    {
        // Clear the list of existing teams when loading
        ClearTeamList();

        var orderedTeams = e.teams.OrderBy(team => team.Name);
        UpdateTeamsList(orderedTeams);
    }

    private void UpdateTeamsList(IEnumerable<TeamModel> teams)
    {
        // Do stuff with the teams...
        foreach (var team in teams)
        {
            // Create a new TeamNameList for each team
            var teamNameList = Instantiate(m_teamNameListTemplate, m_teamPanel.transform);

            // Add the players to the team name list
            var listClone = teamNameList.GetComponent<TeamNameList>();
            listClone.Model = team;
            listClone.UpdateTeamName(team.Name);
            listClone.AddPlayers(team.Players);
        }
    }

    #endregion
}
