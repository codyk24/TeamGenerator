using System.IO;
using System.Linq;
using SAS.UI;
using UnityEngine;
using UnityEngine.UI;

public class WelcomeCanvas : MonoBehaviour
{
    [SerializeField]
    private Button m_loadCategoriesButton;

    [SerializeField]
    private Button m_loadTeamsButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        // Check if each of the load buttons should be interactable
        var filePaths = Directory.EnumerateFiles(Application.persistentDataPath);
        m_loadCategoriesButton.interactable = filePaths.Any(x => x.Contains("category"));
        Debug.LogFormat("DEBUG... Filenames contain {0}: {1}", "category", filePaths.Any(x => x.Contains("category")));

        //m_loadTeamsButton.interactable = m_loadTeamsButton.gameObject.GetComponent<LoadFilesButton>().CheckEnable();
    }
}
