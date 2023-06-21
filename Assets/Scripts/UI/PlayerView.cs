using System;
using System.Collections;
using System.Collections.Generic;
using SAS.Managers;
using SAS.Models;
using SAS.UI;
using TMPro;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField nameInput;

    private PlayerModel m_model;


    #region Properties

    public PlayerModel Model
    {
        get { return m_model; }
        set { 
            if (m_model == value)
                return;

            m_model = value;
        }
    }

    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        if (nameInput != null)
        {
            //currentNameInput.onValueChanged.AddListener(NameInputStart);
            nameInput.onEndEdit.AddListener(UpdatePlayerName);
        }

        // Initialize the model
        m_model = new PlayerModel();
    }

    private void OnDestroy()
    {
        Debug.LogFormat("DEBUG... OnDestroy called in PlayerView");

        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.RemovePlayer(m_model);
        }

        if (m_model != null && m_model.Category != null)
        {
            // Remove the player from its category
            Debug.LogFormat("DEBUG... OnDestroy m_model.Category wasn't null, removing player model from category");
            m_model?.Category?.Players?.Remove(m_model);
        }
    }

    public void DeletePlayer()
    {
        //if (PlayerManager.Instance.Players.Contains(m_model))
        //{
        //    PlayerManager.Instance.RemovePlayer(m_model);
        //}

        //// Remove the player from its category
        //m_model.Category.Players.Remove(m_model);

        // Delete the player input game object
        Destroy(gameObject);
    }

    public void UpdatePlayerName(string input)
    {
        Debug.LogFormat("DEBUG... UpdatePlayerName hit.");
        if (!string.IsNullOrEmpty(input))
        {
            Debug.LogFormat("DEBUG... Updating player name to: {0}", input);
            m_model.Name = input;

            if (!PlayerManager.Instance.Players.Contains(m_model))
            {
                PlayerManager.Instance.AddPlayer(m_model);
            }
        }

        if (transform.gameObject != null)
        {
            CategoryScroll scroll = transform.parent.gameObject.GetComponent<CategoryScroll>();
            StartCoroutine(scroll.Redraw());
        }
    }
}
