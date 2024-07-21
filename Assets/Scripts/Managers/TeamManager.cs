using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SAS.Models;
using NativeShareNamespace;

namespace SAS.Managers
{
	public class TeamListEventArgs : EventArgs
	{
		public List<TeamModel> teams;

		public TeamListEventArgs(List<TeamModel> teams)
		{
			this.teams = teams;
		}
	}

	public class TeamManager : BaseMonoSingleton<TeamManager>
    {
        #region Properties

        int TeamCount = 0;
		public List<TeamModel> Teams { get; set; } = new List<TeamModel>();

		#endregion

		#region Events

		public event EventHandler<TeamListEventArgs> TeamListChanged;

		#endregion

		#region Methods

		// Start is called before the first frame update
		protected override void Awake()
        {
			base.Awake();

			// Add default team
			TeamModel model = new TeamModel();
			Teams.Add(model);
			model.Name = string.Format("Team {0}", Teams.Count);
		}

		public void AddTeam()
        {
			TeamModel model = new TeamModel();
			Teams.Add(model);
			model.Name = string.Format("Team {0}", Teams.Count);
		}

		public void AddTeam(TeamModel model)
		{
			Teams.Add(model);
		}

		public void RemoveTeam()
        {
			// Order teams by name (ascending numerical order) before deleting
			Teams = Teams.OrderBy(team => team.Name).ToList();

			// Remove team at the end of the list
			Teams.RemoveAt(Teams.Count - 1);
        }

		/// <summary>
        /// Method to clear the list of teams
        /// </summary>
		public void ResetTeams()
        {
			Teams.Clear();
        }

		/// <summary>
        /// Method to clear the player list of all teams
        /// </summary>
		public void ClearTeamPlayerLists()
        {
			// Remove the players from the team model
			foreach (var team in Teams)
            {
				team.ClearPlayers();
            }
		}

		/// <summary>
        /// Method to set the list of teams as a whole from disk
        /// </summary>
        /// <param name="categories"></param>
        /// <param name="numTeams"></param>
        /// <returns></returns>
        public void SetTeamList(IEnumerable<TeamModel> teams)
        {
			Teams = teams.ToList();
			TeamListChanged?.Invoke(this, new TeamListEventArgs(teams.ToList()));
		}

		public List<TeamModel> GenerateTeamsWithCategories(List<CategoryModel> categories, int numTeams)
		{
			// Shuffle the players in each category
			System.Random rng = new System.Random();
			foreach (var c in categories)
			{
				Shuffle(c.Players, rng);
			}

			int numCategories = categories.Count;
			int[] categoryIndices = Enumerable.Range(0, numCategories).ToArray();
			Shuffle(categoryIndices, rng);
			int teamIndex = 0;
			foreach (int categoryIndex in categoryIndices)
			{
				CategoryModel category = categories.ElementAt(categoryIndex);
				List<PlayerModel> categoryPlayers = category.Players;
				int numPlayers = categoryPlayers.Count;
				int teamSize = numPlayers / numTeams;
				int remainder = numPlayers % numTeams;
				int startIndex = 0;
				for (int i = 0; i < numTeams; i++)
				{
					int endIndex = startIndex + teamSize + (i < remainder ? 1 : 0); // add one extra player to the first 'remainder' teams
					TeamModel team = Teams[(teamIndex + i) % numTeams];
					for (int j = startIndex; j < endIndex; j++)
					{
						//team.Add((categoryPlayers[j], category));
						team.AddPlayer(categoryPlayers[j]);
					}
					startIndex = endIndex;
				}
				teamIndex = (teamIndex + remainder) % numTeams;
			}

			// Shuffle the order of the teams
			Shuffle(Teams, rng);

			return Teams;
		}

		public void Shuffle<T>(List<T> list, System.Random rng)
		{
			// Fisher-Yates shuffle algorithm
			int n = list.Count;
			while (n > 1)
			{
				n--;
				int k = rng.Next(n + 1);
				T temp = list[k];
				list[k] = list[n];
				list[n] = temp;
			}
		}

		public void Shuffle(int[] list, System.Random rng)
		{
			// Fisher-Yates shuffle algorithm
			int n = list.Count();
			while (n > 1)
			{
				n--;
				int k = rng.Next(n + 1);
				int value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}

		public string PrintTeams()
        {
			var orderedTeams = Teams.OrderBy(team => team.Name);
			string teamString = "====================\n";
			foreach (var team in orderedTeams)
            {
				team.Print();

				teamString += team.Name + "\n\n";
				foreach (var player in team.Players)
                {
					teamString += player.Name + "\n";
                }

				teamString += "====================\n";
			}

			Debug.Log(teamString);

			NativeShare sharer = new NativeShare();
			sharer.Clear();
			sharer.SetText(teamString);
			sharer.Share();
			return teamString;
		}

		#endregion
	}
}

