using SAS.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CategoryControl : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private TMP_Text m_numberCategoriesLabel;

    [SerializeField]
    private GameObject m_categoryScrollTemplate;

    [SerializeField]
    private GameObject m_categoryPanel;

    [SerializeField]
    private Button m_addButton;
    
    [SerializeField]
    private Button m_minusButton;

    private GameObject m_lastCategory;

    #endregion
    // Start is called before the first frame update
    void Awake()
    {
        m_addButton.onClick.AddListener(AddCategory);
        m_minusButton.onClick.AddListener(RemoveCategory);

        m_minusButton.interactable = false;

        CategoryManager.Instance.CategoryAdded += CategoriesChanged;
        CategoryManager.Instance.CategoryRemoved += CategoriesChanged;
    }

    private void CategoriesChanged(object sender, EventArgs e)
    {
        Debug.LogFormat("DEBUG... CategoryControl:CategoriesChanged listener reached.");
        // Update the number categories text
        m_numberCategoriesLabel.text = CategoryManager.Instance.Categories.Count.ToString();

        // Enable the minus button if there's more than one category
        m_minusButton.interactable = CategoryManager.Instance.Categories.Count > 1;

        StartCoroutine(Redraw());
    }

    private void AddCategory()
    {
        Debug.LogFormat("DEBUG... Add category listener reached...");
        // Instantiate a new category scroll
        m_lastCategory = Instantiate(m_categoryScrollTemplate, m_categoryPanel.transform);
        m_lastCategory.transform.SetAsFirstSibling();
    }

    private void RemoveCategory()
    {
        // Enable the minus button if there's more than one category
        m_minusButton.interactable = CategoryManager.Instance.Categories.Count > 1;

        // Remove the last category scroll in the list
        Destroy(m_lastCategory);
        
        // After removing, update the lastCategory reference
        m_lastCategory = m_categoryPanel.transform.GetChild(CategoryManager.Instance.Categories.Count - 1).gameObject;
        m_lastCategory = m_categoryPanel.transform.GetChild(0).gameObject;
    }

    private IEnumerator Redraw()
    {
        yield return new WaitForEndOfFrame();
        RectTransform rectTransform = m_categoryPanel.GetComponent<RectTransform>();
        LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
    }
}
