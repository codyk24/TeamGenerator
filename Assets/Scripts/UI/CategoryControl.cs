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
        }
        else
        {
            List<CategoryModel> categoriesWithViews = new List<CategoryModel>();

            // Compile list of categories with scrolls
            for (int i = 0; i < m_categoryPanel.transform.childCount; i++)
            {
                var categoryScroll = m_categoryPanel.transform.GetChild(i).GetComponent<CategoryScroll>();
                if (categoryScroll.Model != null)
                {
                    categoriesWithViews.Add(categoryScroll.Model);
                    break;
                }
            }

            var categoriesToAdd = CategoryManager.Instance.Categories.Except(categoriesWithViews);
            // If we didn't find a category with this name
            foreach (var category in categoriesToAdd)
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

    public void ClearCategories()
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

        var filePaths = Directory.EnumerateFiles(Application.persistentDataPath);
        DialogCanvas.Instance.Show("Save Successful!", string.Format("Path: {0}, save successful: {1}", Application.persistentDataPath, success), Accent.Correct, "OK", true);
    }

    private IEnumerator Redraw()
    {
        yield return new WaitForEndOfFrame();
        RectTransform rectTransform = m_categoryPanel.GetComponent<RectTransform>();
        LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
    }
}
