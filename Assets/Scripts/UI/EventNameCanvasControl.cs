using SAS.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SAS.UI
{
    public class EventNameCanvasControl : MonoBehaviour
    {
        [SerializeField]
        GameObject m_dialog;

        private void OnDisable()
        {
            m_dialog.SetActive(false);
        }

        public void CheckShow()
        {
            // We've already got an event loaded... skip show
            if (!string.IsNullOrEmpty(TeamEventManager.Instance.EventModel.Name))
            {
                // Navigate to the next canvas (Category list)
                CanvasQueue.Instance.Next();

                // Hide this game object
                gameObject.SetActive(false);
            }
            else
            {
                // Show the text input dialog
                m_dialog.SetActive(true);
            }
        }
    }
}