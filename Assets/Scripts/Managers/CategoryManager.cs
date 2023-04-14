using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SAS.Models;

namespace SAS.Managers
{
    public class CategoryManager : BaseMonoSingleton<CategoryManager>
    {
        #region Properties
        public List<CategoryModel> Categories { get; set; } = new List<CategoryModel>();

        #endregion

        #region Events

        public event System.EventHandler CategoryAdded;
        public event System.EventHandler CategoryRemoved;

        #endregion

        #region Methods

        // Start is called before the first frame update
        protected override void Awake()
        {
            base.Awake();
        }

        public void Add(CategoryModel model)
        {
            if (!Categories.Contains(model))
            {
                Categories.Add(model);
                CategoryAdded?.Invoke(this, new System.EventArgs());
            }
        }

        public bool Remove(CategoryModel model)
        {
            if (Categories.Remove(model))
            {
                CategoryRemoved?.Invoke(this, new System.EventArgs());
                Debug.LogFormat("DEBUG... Category successfully removed");
                return true;
            }
            else
            {
                Debug.LogFormat("DEBUG... Category failed to be removed");
                return false;
            }
        }

        public int SmallestCategory()
        {
            int minCategorySize = int.MaxValue;
            foreach (var category in Categories)
            {
                if (category.CategorySize < minCategorySize)
                {
                    minCategorySize = category.CategorySize;
                }
            }

            return minCategorySize;
        }

        #endregion
    }
}

