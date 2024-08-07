using SAS.Managers;
using SAS.Models;
using SAS.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CategoryControl : BaseMonoSingleton<CategoryControl>
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
    protected override void Awake()
    {
        base.Awake();

        Debug.LogFormat("DEBUG... CategoryControl:Awake reached");
        m_addButton.onClick.AddListener(AddCategory);
        m_minusButton.onClick.AddListener(RemoveCategory);

        m_minusButton.interactable = false;

        CategoryManager.Instance.CategoryAdded += CategoriesChanged;
        CategoryManager.Instance.CategoryRemoved += CategoriesChanged;
    }

    /// <summary>
    /// Check the CategoryManager for any categories not present in the list
    /// and instantiate a CategoryScroll if not there
    /// </summary>
    private void OnEnable()
    {
        Debug.LogFormat("DEBUG... CategoryControl:OnEnable reached. Categories: {0}, number of scrolls: {1}", CategoryManager.Instance.Categories.Count, m_categoryPanel.transform.childCount);
        if (m_categoryPanel.transform.childCount < 1 && CategoryManager.Instance.Categories.Count < 1)
        {
            AddCategory();
            CategoriesChanged(this, EventArgs.Empty);
        }
        else if (CategoryManager.Instance.Categories.Count > 1)
        {
            // Grab the original categories on the panel
            List<GameObject> originalCategories = new List<GameObject>();
            for (int i = 0; i < m_categoryPanel.transform.childCount; i++)
            {
                originalCategories.Add(m_categoryPanel.transform.GetChild(i).gameObject);
            }

            // The categories that were there to start, remove them from the bottom of the list
            for (int i = 0; i < originalCategories.Count; i++)
            {
                Destroy(originalCategories[i]);
            }

            // Add all the categories in the manager
            foreach (var category in CategoryManager.Instance.Categories)
            {
                AddCategoryFromModel(category);
            }
        }

        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(Redraw());
        }

        // Refresh category count at top in case listener wasn't set up yet
        CategoriesChanged(this, EventArgs.Empty);
    }

    private void CategoriesChanged(object sender, EventArgs e)
    {
        Debug.LogFormat("DEBUG... CategoryControl:CategoriesChanged listener reached. Number of categories: {0}", CategoryManager.Instance.Categories.Count);
        // Update the number categories text
        m_numberCategoriesLabel.text = CategoryManager.Instance.Categories.Count.ToString();

        // Enable the minus button if there's more than one category
        m_minusButton.interactable = CategoryManager.Instance.Categories.Count > 1;
    }

    private void AddCategory()
    {
        Debug.LogFormat("DEBUG... Add category listener reached...");
        // Instantiate a new category scroll
        m_lastCategory = Instantiate(m_categoryScrollTemplate, m_categoryPanel.transform);
        m_lastCategory.transform.SetAsFirstSibling();

        var scroll = m_lastCategory.GetComponent<CategoryScroll>();
        scroll.Initialize();
    }

    public void AddCategoryFromModel(CategoryModel model)
    {
        // Instantiate a new category scroll
        var categoryScrollObject = Instantiate(m_categoryScrollTemplate, m_categoryPanel.transform);
        categoryScrollObject.transform.SetAsFirstSibling();
        var scroll = categoryScrollObject.GetComponent<CategoryScroll>();
        scroll.InitializeFromModel(model);
        m_lastCategory = categoryScrollObject;
    }

    private void RemoveCategory()
    {
        // Enable the minus button if there's more than one category
        m_minusButton.interactable = CategoryManager.Instance.Categories.Count > 1;

        // Remove the model from the category manager
        var categoryScroll = m_lastCategory.GetComponent<CategoryScroll>();
        if (categoryScroll.Model != null)
        {
            CategoryManager.Instance.Remove(categoryScroll.Model);
        }

        // Remove the last category scroll in the list
        Destroy(m_lastCategory);
        
        // After removing, update the lastCategory reference
        m_lastCategory = m_categoryPanel.transform.GetChild(CategoryManager.Instance.Categories.Count - 1).gameObject;
        m_lastCategory = m_categoryPanel.transform.GetChild(0).gameObject;
    }

    private void ClearCategoryList()
    {
        for (int i = 0; i < m_categoryPanel.transform.childCount; i++)
        {
            Destroy(m_categoryPanel.transform.GetChild(i));
        }
    }

    public void ClearCategoryNames()
    {
        // Clear the players from each category scroll in children
        for (int i = 0; i < m_categoryPanel.transform.childCount; i++)
        {
            m_categoryPanel.transform.GetChild(i).GetComponent<CategoryScroll>().ClearCategory();
        }
    }

    public void SaveCategories()
    {
        // Clear the players from each category scroll in children
        bool success = true;
        for (int i = 0; i < m_categoryPanel.transform.childCount; i++)
        {
            success &= m_categoryPanel.transform.GetChild(i).GetComponent<CategoryScroll>().SaveCategory();
        }

        DialogCanvas.Instance.Show("Save Successful!", "These categories can be loaded in subsequent sessions from the Load Category page", Accent.Correct, "OK", true);
    }

    private IEnumerator Redraw()
    {
        yield return new WaitForEndOfFrame();
        RectTransform rectTransform = m_categoryPanel.GetComponent<RectTransform>();
        LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
    }
}
