using SAS.Managers;
using SAS.UI;
using System.Linq;
using System.Collections;
using System.IO;
using UnityEngine;

namespace SAS.UI
{
    public class SavedCategoriesList : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_savedCategoriesPanel;

        [SerializeField]
        private GameObject m_savedCategoryTemplate;

        private string m_dataPath;

        private void Awake()
        {
            m_dataPath = Application.persistentDataPath;
        }

        public void ClearTeamList()
        {
            // Clear the content from the scroll view
            for (int i = 0; i < m_savedCategoriesPanel.transform.childCount; i++)
            {
                GameObject child = m_savedCategoriesPanel.transform.GetChild(i).gameObject;
                Destroy(child);
            }
        }

        public void PopulateListFromFiles()
        {
            // Enumerate all JSON files in the data directory that contain "category"
            var files = Directory.EnumerateFiles(m_dataPath, "category*.json", SearchOption.TopDirectoryOnly);
            Debug.LogFormat("DEBUG... Application.PersistentDataPath: {0}", m_dataPath);
            foreach (var filePath in files)
            {
                Debug.LogFormat("DEBUG... PopulateListFromFiles, category file: {0}", filePath);
                // Create a new TeamNameList for each team
                var teamNameList = Instantiate(m_savedCategoryTemplate, m_savedCategoriesPanel.transform);

                // Add the players to the team name list
                var savedCategoryClone = teamNameList.GetComponent<SavedCategoryItem>();
                savedCategoryClone.Initialize(filePath);
            }
        }
    }
}