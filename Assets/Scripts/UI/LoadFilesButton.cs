using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace SAS.UI
{
    public class LoadFilesButton : MonoBehaviour
    {
        [SerializeField]
        private Button m_loadButton;

        [SerializeField]
        private string fileNamePattern;

        // Start is called before the first frame update
        void Start()
        {
            m_loadButton.interactable = CheckEnable();
        }

        public bool CheckEnable()
        {
            var filePaths = Directory.EnumerateFiles(Application.persistentDataPath);
            Debug.LogFormat("DEBUG... Filenames contain {0}: {1}", fileNamePattern, filePaths.Any(x => x.Contains(fileNamePattern)));
            return filePaths.Any(x => x.Contains(fileNamePattern));
        }
    }
}