using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using SAS.Models;

namespace SAS.Managers
{
    public class CategoryEventArgs : System.EventArgs
    {
        public CategoryModel model;

        public CategoryEventArgs(CategoryModel model)
        {
            this.model = model;
        }
    }

    public class CategoryListEventArgs : System.EventArgs
    {
        public List<CategoryModel> categories;

        public CategoryListEventArgs(List<CategoryModel> models)
        {
            this.categories = models;
        }
    }

    public class CategoryManager : BaseMonoSingleton<CategoryManager>
    {
        #region Properties
        public List<CategoryModel> Categories { get; set; } = new List<CategoryModel>();

        #endregion

        #region Events

        public event System.EventHandler<CategoryEventArgs> CategoryAdded;
        public event System.EventHandler<CategoryEventArgs> CategoryRemoved;
        public event System.EventHandler<CategoryListEventArgs> CategoryListChanged;

        #endregion

        #region Methods

        // Start is called before the first frame update
        protected override void Awake()
        {
            base.Awake();
        }

        public void Add(CategoryModel model)
        {
            Debug.LogFormat("DEBUG... CategoryManager:Add model: {0}", model.Name);
            if (!Categories.Contains(model))
            {
                Categories.Add(model);
                CategoryAdded?.Invoke(this, new CategoryEventArgs(model));
            }
        }

        public bool Remove(CategoryModel model)
        {
            if (Categories.Remove(model))
            {
                CategoryRemoved?.Invoke(this, new CategoryEventArgs(model));
                Debug.LogFormat("DEBUG... Category successfully removed");
                return true;
            }
            else
            {
                Debug.LogFormat("DEBUG... Category failed to be removed");
                return false;
            }
        }

        public void ClearCategories()
        {
            foreach (var category in Categories)
            {
                CategoryRemoved?.Invoke(this, new CategoryEventArgs(category));
            }

            Categories.Clear();
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

        public void SetCategoryList(IEnumerable<CategoryModel> categories)
        {
            Categories = categories.ToList();
            CategoryListChanged?.Invoke(this, new CategoryListEventArgs(categories.ToList()));
        }

        #endregion
    }
}

