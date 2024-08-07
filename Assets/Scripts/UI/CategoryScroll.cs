using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SAS.Managers;
using SAS.Models;
using TMPro;

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

        /// <summary>
        /// The bottom name input in the category, used to track
        /// population of the next player name input in the list
        /// </summary>
        [SerializeField]
        private TMP_Text playerCount;

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
            //CategoryManager.Instance.Remove(m_model);
        }

        public void Initialize()
        {
            m_model = new CategoryModel();
            CategoryManager.Instance.Add(m_model);

            // Initialize the category name based on number of categories
            string categoryName = string.Format("Category {0}", CategoryManager.Instance.Categories.Count);
            m_model.Name = categoryNameInput.text = categoryName;

            playerCount.text = string.Format("Player Count: {0}", m_model.Players.Count);
        }

        public void InitializeFromModel(CategoryModel model)
        {
            // Override the original model properties
            m_model = model;

            // Populate the category name
            UpdateCategoryName(model.Name);
            categoryNameInput.text = m_model.Name;
            UpdatePlayerCountText();

            // Populate the player views in the category scroll
            foreach (var player in model.Players)
            {
                // Instantiate next player input at the top of list
                var nameInput = Instantiate(nameInputTemplate, transform);
                nameInput.transform.SetSiblingIndex(transform.childCount - 1);

                var view = nameInput.GetComponent<PlayerView>();
                nameInput.GetComponentInChildren<TMP_InputField>().text = player.Name;
                view.InitializePlayer(player);
            }

            // Redraw after initializing all players
            if (gameObject.activeInHierarchy)
            {
                StartCoroutine(Redraw());
            }

            // Transfer listeners to the empty if necessary

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
                nextNameInput.transform.SetSiblingIndex(1);
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

            UpdatePlayerCountText();
        }

        [ContextMenu("Clear Category")]
        public void ClearCategory()
        {
            Debug.LogFormat("DEBUG... PlayerManager player count before delete: {0}, category player count: {1}", PlayerManager.Instance.Players.Count, m_model.Players.Count);

            // Clear players from the category model
            m_model.Players.Clear();

            // Transfer the listeners back to initial player view before deleting players
            TMP_InputField firstNameInput = transform.GetChild(1).GetComponentInChildren<TMP_InputField>();

            TransferListeners(currentNameInput, firstNameInput);
            currentNameInput = firstNameInput;

            // Iterate over children, starting at extent of list
            for (int i = transform.childCount - 1; i > 0; i--)
            {
                var playerView = transform.GetChild(i).GetComponent<PlayerView>();
                // Destroy all players except first one in category list
                if (i > 1)
                {
                    if (playerView != null)
                    {
                        Destroy(playerView.gameObject);
                    }
                }
                // Maintain this initial player, but delete the name
                else
                {
                    firstNameInput.text = string.Empty;
                    playerView.UpdatePlayerName(string.Empty);
                }
            }

            // Redraw after you're done deleting
            StartCoroutine(Redraw());
            Debug.LogFormat("DEBUG... PlayerManager player count after delete: {0}, category player count: {1}", PlayerManager.Instance.Players.Count, m_model.Players.Count);
        }

        public void UpdatePlayerCountText()
        {
            playerCount.text = string.Format("Player Count: {0}", m_model.Players.Count);
        }

        public void DecrementPlayerCountText()
        {
            playerCount.text = string.Format("Player Count: {0}", m_model.Players.Count - 1);
        }


        [ContextMenu("Save Category")]
        public bool SaveCategory()
        {
            return FileDataHandler.SaveCategory(m_model);
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
