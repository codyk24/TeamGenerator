using System.IO;
using UnityEngine;

namespace SAS.UI
{
    public class SavedEventList : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_savedEventsPanel;

        [SerializeField]
        private GameObject m_savedEventItemTemplate;

        private string m_dataPath;

        private void Awake()
        {
            m_dataPath = Application.persistentDataPath;
        }

        public void ClearTeamList()
        {
            // Clear the content from the scroll view
            for (int i = 0; i < m_savedEventsPanel.transform.childCount; i++)
            {
                GameObject child = m_savedEventsPanel.transform.GetChild(i).gameObject;
                Destroy(child);
            }
        }

        public void PopulateListFromFiles()
        {
            // Enumerate all JSON files in the data directory that contain "category"
            var files = Directory.EnumerateFiles(m_dataPath, "event*.json", SearchOption.TopDirectoryOnly);
            Debug.LogFormat("DEBUG... Application.PersistentDataPath: {0}", m_dataPath);

            int childCount = m_savedEventsPanel.transform.childCount;
            // Destroy all children and start fresh
            for (int i = 0; i < childCount; i++)
            {
                Destroy(m_savedEventsPanel.transform.GetChild(i).gameObject);
            }

            // Ready to initialize now
            foreach (var filePath in files)
            {
                Debug.LogFormat("DEBUG... PopulateListFromFiles, category file: {0}", filePath);
                // Create a new TeamNameList for each team
                var savedEventObject = Instantiate(m_savedEventItemTemplate, m_savedEventsPanel.transform);

                // Add the players to the team name list
                var savedEventClone = savedEventObject.GetComponent<SavedEventItem>();
                savedEventClone.Initialize(filePath);
            }
        }
    }
}

