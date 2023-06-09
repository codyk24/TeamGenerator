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
    public class CategoryScroll : MonoBehaviour
    {
        #region Fields

        private RectTransform m_rectTransform;

        private CategoryModel m_model;
        
        [SerializeField]
        private GameObject nameInputTemplate;

        [SerializeField]
        private TMP_InputField categoryNameInput;
        
        /// <summary>
        /// The bottom name input in the category, used to track
        /// population of the next player name input in the list
        /// </summary>
        [SerializeField]
        private TMP_InputField currentNameInput;

        private GameObject nextNameInput;

        #endregion

        #region Properties

        public CategoryModel Model => m_model;

        #endregion

        #region Methods

        // Start is called before the first frame update
        void Awake()
        {
            m_rectTransform = GetComponent<RectTransform>();

            if (categoryNameInput != null)
            {
                categoryNameInput.onEndEdit.AddListener(UpdateCategoryName);
            }

            // Create a category model to track
            m_model = new CategoryModel();
            CategoryManager.Instance.Add(m_model);

            // Initialize the category name based on number of categories
            string categoryName = string.Format("Category {0}", CategoryManager.Instance.Categories.Count);
            m_model.Name = categoryNameInput.text = categoryName;

            if (currentNameInput != null)
            {
                currentNameInput.onValueChanged.AddListener(NameInputStart);
                currentNameInput.onEndEdit.AddListener(NameInputFinish);
            }
        }

        private void OnDestroy()
        {
            if (currentNameInput != null)
            {
                currentNameInput.onValueChanged.RemoveListener(NameInputStart);
                currentNameInput.onEndEdit.RemoveListener(NameInputFinish);
            }

            // Remove the players from the category model
            m_model.Players.Clear();

            // Remove category from the category manager
            CategoryManager.Instance.Remove(m_model);
        }

        private void TransferListeners(TMP_InputField current, TMP_InputField next)
        {
            current.onValueChanged.RemoveListener(NameInputStart);
            current.onEndEdit.RemoveListener(NameInputFinish);

            next.onValueChanged.AddListener(NameInputStart);
            next.onEndEdit.AddListener(NameInputFinish);
        }

        /// <summary>
        /// Listener for when the current name input is finished
        /// </summary>
        /// <param name="input"></param>
        private void UpdateCategoryName(string input)
        {
            // Transfer the listeners to the next game input
            if (!string.IsNullOrEmpty(input))
            {
                // Update the name of the category
                m_model.Name = input;
            }
            // Reset the category name to its previous state
            else
            {
                categoryNameInput.text = m_model.Name;
            }
        }
        
        /// <summary>
        /// Listener for when you start to edit the current name
        /// Instantiate the next name input
        /// </summary>
        /// <param name="input"></param>
        private void NameInputStart(string input)
        {
            if (!string.IsNullOrEmpty(input) && nextNameInput == null)
            {
                // Track the player model in the category
                PlayerView view = currentNameInput.GetComponentInParent<PlayerView>();
                m_model.Players.Add(view.Model);

                // Set the category model of the player model
                view.Model.Category = m_model;

                // Instantiate next player input when you populate the current
                nextNameInput = Instantiate(nameInputTemplate, transform);
                StartCoroutine(Redraw());
            }
        }

        /// <summary>
        /// Listener for when the current name input is finished
        /// </summary>
        /// <param name="input"></param>
        private void NameInputFinish(string input)
        {
            // Transfer the listeners to the next game input
            if (!string.IsNullOrEmpty(input))
            {
                TransferListeners(currentNameInput, nextNameInput.GetComponentInChildren<TMP_InputField>());

                currentNameInput = nextNameInput.GetComponentInChildren<TMP_InputField>();
                nextNameInput = null;
            }
            // Player name was empty when edit finished, need to remove next player input if created
            else
            {
                if (nextNameInput != null)
                {
                    //PlayerView nextPlayerView = nextNameInput.GetComponentInParent<PlayerView>();
                    //m_model.Players.Remove(nextPlayerView?.Model);
                    Destroy(nextNameInput);
                }
            }
        }

        public IEnumerator Redraw()
        {
            yield return new WaitForEndOfFrame();
            LayoutRebuilder.MarkLayoutForRebuild(m_rectTransform);
        }

        [ContextMenu("Print Category")]
        public void PrintCategory()
        {
            m_model.Print();
        }
        
        [ContextMenu("Print Players")]
        public void PrintPlayers()
        {
            PlayerManager.Instance.PrintPlayers();
        }

        #endregion
    }
}
