using SAS.Managers;
using SAS.UI;
using System.Linq;
using UnityEngine;

public class TeamOutputList : MonoBehaviour
{
    [SerializeField]
    private GameObject m_teamPanel;
    
    [SerializeField]
    private GameObject m_teamNameListTemplate;

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
        TeamManager.Instance.ResetTeams();

        ClearTeamList();

        // Call the GenerateTeams function again
        var teams = TeamManager.Instance.GenerateTeamsWithCategories(
            CategoryManager.Instance.Categories,
            TeamManager.Instance.Teams.Count);

        var orderedTeams = teams.OrderBy(team => team.Name);

        // Do stuff with the teams...
        foreach (var team in orderedTeams)
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

    public void ShareTeams()
    {
        string output = TeamManager.Instance.PrintTeams();

        // Do stuff with the output --> share
    }
}
