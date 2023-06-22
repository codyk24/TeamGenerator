using System.Collections;
using System;
using System.IO;
using TMPro;
using UnityEngine;
using SAS.Models;
using Newtonsoft.Json;

namespace SAS.UI
{
    public class SavedCategoryItem : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI m_categoryNameText;

        [SerializeField]
        private TextMeshProUGUI m_categorySizeText;

        [SerializeField]
        private TextMeshProUGUI m_categoryDateText;

        private CategoryModel m_categoryModel;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Initialize the text fields within the saved category item
        public void Initialize(string filePath)
        {
            // category_Boys_06_21_23_09_10.json
            var fileName = Path.GetFileName(filePath);
            Debug.LogFormat("DEBUG... SavedCategoryItem:InitializeText: {0}", fileName);

            var split = fileName.Split('_');
            Debug.LogFormat("DEBUG... Split string array length: {0}", split.Length);

            // [0] = category
            // [1] = Name
            // [2]-[6] = Date
            string name = split[1].Trim('_');
            string month = split[2].Trim('_');
            string day = split[3].Trim('_');
            string year = split[4].Trim('_');
            string hour = split[5].Trim('_');
            string minute = split[6].Trim('_').Substring(0, 2);

            Debug.LogFormat("DEBUG... Name: {0}, month: {1}, day: {2}, year: {3}", name, month, day, year);

            m_categoryNameText.text = name;
            m_categoryDateText.text = string.Format("{0}-{1}-20{2} {3}:{4}", month, day, year, hour, minute);
            try
            {
                string fileText = File.ReadAllText(filePath);
                m_categoryModel = JsonConvert.DeserializeObject<CategoryModel>(fileText);
                m_categorySizeText.text = string.Format("Players: {0}", m_categoryModel.CategorySize.ToString());
            }
            catch (Exception e)
            {
                Debug.LogFormat("DEBUG... exception reading saved category item");
            }
        }
    }
}

