using System;
using System.Linq;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using SAS.Managers;
using SAS.Models;
using Newtonsoft.Json;

namespace SAS.UI
{
    public class SavedEventItem : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI m_eventNameText;

        [SerializeField]
        private TextMeshProUGUI m_categorySizeText;

        [SerializeField]
        private TextMeshProUGUI m_teamSizeText;

        [SerializeField]
        private TextMeshProUGUI m_eventDateText;

        [SerializeField]
        private Button m_button;

        [SerializeField]
        private CanvasQueue m_canvasQueue;

        private EventModel m_eventModel;

        private string m_filePath;

        public string FilePath => m_filePath;

        // Start is called before the first frame update
        void Start()
        {
            m_button.onClick.AddListener(LoadEventWarning);
        }

        // Initialize the text fields within the saved category item
        public void Initialize(string filePath)
        {
            m_filePath = filePath;

            // category_Boys_06_21_23_09_10.json
            var fileName = Path.GetFileName(filePath);
            Debug.LogFormat("DEBUG... SavedEventItem:InitializeText: {0}", fileName);

            var split = fileName.Split('_');
            Debug.LogFormat("DEBUG... Split string array length: {0}", split.Length);

            // [0] = category
            // [1] = Name
            // [2]-[6] = Date
            string name = split[1].Trim('_');
            string month = split[2].Trim('_');
            string day = split[3].Trim('_');
            string year = split[4].Trim('_').Replace(".json", string.Empty);

            Debug.LogFormat("DEBUG... Name: {0}, month: {1}, day: {2}, year: {3}", name, month, day, year);

            DateTime dateTime = new(Convert.ToInt32(year), Convert.ToInt32(month), Convert.ToInt32(day));
            m_eventDateText.text = dateTime.ToString("MM/dd/yy");
            try
            {
                string fileText = File.ReadAllText(filePath);
                m_eventModel = JsonConvert.DeserializeObject<EventModel>(fileText);
                m_eventNameText.text = string.Format("Name: {0}", m_eventModel.Name);
                m_teamSizeText.text = string.Format("Teams: {0}", m_eventModel.Teams.Count.ToString());
                m_categorySizeText.text = string.Format("Categories: {0}", m_eventModel.Categories.Count.ToString());
            }
            catch (Exception e)
            {
                Debug.LogFormat("DEBUG... exception reading saved event item");
            }
        }

        private void LoadEventWarning()
        {
            if (CategoryManager.Instance.Categories.Any() || TeamManager.Instance.Teams.Any())
            {
                DialogCanvas.Show("Load Event?",
                    "Loading an event will erase any current teams or categories. Would you like to load this event?",
                    Accent.Caution, 
                    "Load",
                    "Cancel",
                    () => LoadEvent(),
                    () => { return; },
                    true);
            }
            else
            {
                LoadEvent();
            }
        }

        private void LoadEvent()
        {
            Debug.LogFormat("DEBUG... SavedEventItem:LoadEvent reached");
            if (CategoryManager.Instance.Categories.Any(category => category.Name.Equals(m_eventModel.Name)))
            {
                // Show an error message that a category with this name already exists
                DialogCanvas.Show("Load Failed", "This event has not been loaded because an event with this name already exists.", Accent.Warning, "Close", null, true);
            }
            else
            {
                // Show pop-up that category has been loaded
                DialogCanvas.Show("Load Successful", "Your event has been successfully loaded.", Accent.Correct, "Close", null, true);
                TeamEventManager.Instance.LoadEventModel(m_eventModel);

                // Navigate to the last canvas (Teams list)
                CanvasQueue.Instance.SetIndex(CanvasQueue.Instance.CanvasListSize - 1);

                // Find the list ancestor and disable that panel
                var eventList = FindObjectOfType<SavedEventList>();
                eventList.gameObject.SetActive(false);
            }
        }
    }
}

