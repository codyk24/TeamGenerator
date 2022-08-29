using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SAS.Models;

namespace SAS.Managers
{
    public class CategoryManager : BaseMonoSingleton<CategoryManager>
    {
        #region Properties

        public List<CategoryModel> Categories { get; set; }

        #endregion

        #region Methods

        // Start is called before the first frame update
        void Start()
        {
            Categories = new List<CategoryModel>();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        #endregion
    }
}

