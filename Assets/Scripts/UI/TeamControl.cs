using SAS.Managers;
using SAS.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TeamControl : BaseMonoSingleton<TeamControl>
{
    #region Fields

    [SerializeField]
    private TMP_Text m_numberTeamsLabel;

    [SerializeField]
    private GameObject m_teamNameListTemplate;

    [SerializeField]
    private GameObject m_teamPanel;

    [SerializeField]
    private Button m_addButton;
    
    [SerializeField]
    private Button m_minusButton;

    private GameObject m_lastTeam;

    private int numberTeams = 1;

    #endregion
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();

        Debug.LogFormat("DEBUG... TeamControl.Awake reached");
        m_addButton.onClick.AddListener(AddTeam);
        m_minusButton.onClick.AddListener(RemoveTeam);

        m_minusButton.interactable = false;
    }

    private void OnEnable()
    {
        StartCoroutine(UpdateTeamCountText());
    }

    private void AddTeam()
    {
        TeamManager.Instance.AddTeam();

        StartCoroutine(UpdateTeamCountText());
    }

    private void RemoveTeam()
    {
        // Remove the last team in the list
        TeamManager.Instance.RemoveTeam();

        StartCoroutine(UpdateTeamCountText());
    }

    private IEnumerator UpdateTeamCountText()
    {
        yield return new WaitForEndOfFrame();

        // Update the number categories text
        m_numberTeamsLabel.text = TeamManager.Instance.Teams.Count.ToString();

        // Enable the minus button if there's more than one category
        m_minusButton.interactable = TeamManager.Instance.Teams.Count > 1;
    }

    public void GenerateTeams()
    {
        // Clear the team list before starting
        TeamManager.Instance.ClearTeamPlayerLists();
        for (int i = 0; i < m_teamPanel.transform.childCount; i++)
        {
            GameObject child = m_teamPanel.transform.GetChild(i).gameObject;
            Destroy(child);
        }

        var teams = TeamManager.Instance.GenerateTeamsWithCategories(
            CategoryManager.Instance.Categories, 
            TeamManager.Instance.Teams.Count);

        var orderedTeams = teams.OrderBy(team => team.Name);

        // Do stuff with the teams...
        foreach(var team in orderedTeams)
        {
            team.Print();

            // Create a new TeamNameList for each team
            var teamNameList = Instantiate(m_teamNameListTemplate, m_teamPanel.transform);

            // Add the players to the team name list
            var listClone = teamNameList.GetComponent<TeamNameList>();
            listClone.Model = team;
            listClone.UpdateTeamName(team.Name);
            listClone.AddPlayers(team.Players);
        }

        // Save the teams to a file
        TeamEventManager.Instance.SaveEventJson();
    }
}
