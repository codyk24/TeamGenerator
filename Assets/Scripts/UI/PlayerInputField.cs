using System;
using System.Collections;
using System.Collections.Generic;
using SAS.Models;
using TMPro;
using UnityEngine;

public class PlayerInputField : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField nameInput;

    private PlayerModel m_model;

    // Start is called before the first frame update
    void Start()
    {
        if (nameInput != null)
        {
            //currentNameInput.onValueChanged.AddListener(NameInputStart);
            nameInput.onEndEdit.AddListener(NameInputFinish);
        }

        // Initialize the model
        m_model = new PlayerModel();
    }

    private void NameInputFinish(string input)
    {
        if (!string.IsNullOrEmpty(input))
        {
            m_model.Name = input;
        }
    }
}
