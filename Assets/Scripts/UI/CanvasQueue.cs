using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasQueue : BaseMonoSingleton<CanvasQueue>
{
    #region Members

    [SerializeField]
    private List<GameObject> m_canvasList = new List<GameObject>();

    [SerializeField]
    private GameObject m_backButton;

    [SerializeField]
    private GameObject m_nextButton;

    [SerializeField]
    private GameObject m_currentCanvas;

    #endregion

    #region Properties

    public GameObject CurrentCanvas
    {
        get { return m_currentCanvas; }
        set
        {
            m_currentCanvas = value;
            Debug.LogFormat("DEBUG... Setting m_current canvas to: {0}", value.name);
        }
    }

    public int CanvasListSize => m_canvasList.Count;

    #endregion

    #region Methods

    // Start is called before the first frame update
    void Awake()
    {
        Debug.LogFormat("DEBUG... m_canvasList.Count: {0}", m_canvasList.Count);
        CurrentCanvas = m_canvasList.First();

        m_backButton.GetComponent<Button>().onClick.AddListener(Previous);
        m_nextButton.GetComponent<Button>().onClick.AddListener(Next);
        m_backButton.gameObject.SetActive(false);
        m_nextButton.gameObject.SetActive(false);

        // Listen to the visibility events of the canvas items
        for (int i = 0; i < m_canvasList.Count; i++)
        {
            var item = m_canvasList[i].GetComponent<CanvasQueueItem>();
            item.CanvasQueueIndex = i;
            //item.VisibilityChanged += Canvas_VisibilityChanged;
        }

        ToggleAllCanvasesOff();
        CurrentCanvas.SetActive(true);
    }

    private void Canvas_VisibilityChanged(object sender, QueueItemVisibilityEventArgs e)
    {
        if (e.Visible)
        {
            SetIndex(e.Index);
        }
    }

    public void SetIndex(int index)
    {
        if (index < m_canvasList.Count)
        {
            CurrentCanvas.SetActive(false);

            CurrentCanvas = m_canvasList[index].gameObject;
            CurrentCanvas.SetActive(true);

            // Update the back button visibility
            m_backButton.SetActive(HasPrevious());
        }
    }

    public void ToggleAllCanvasesOff()
    {
        foreach (var canvas in m_canvasList)
        {
            canvas.SetActive(false);
        }
    }

    public void Next()
    {
        if (!HasNext())
        {
            m_nextButton.SetActive(false);
            return;
        }

        m_backButton.SetActive(true);

        int currentIndex = m_canvasList.IndexOf(CurrentCanvas);
        CurrentCanvas.SetActive(false);

        CurrentCanvas = m_canvasList[currentIndex + 1];
        CurrentCanvas.SetActive(true);
        Debug.LogFormat("DEBUG...NEXT count: {0} current index: {1}, setting current index to: {2}, currentCanvas name: {3}", m_canvasList.Count, currentIndex, currentIndex + 1, CurrentCanvas.name);

        // Update the next button's visibility after hitting next
        m_nextButton.gameObject.SetActive(HasNext());
    }

    public void Previous()
    {
        if (!HasPrevious())
        {
            m_backButton.SetActive(false);
            return;
        }

        int currentIndex = m_canvasList.IndexOf(CurrentCanvas);
        Debug.LogFormat("DEBUG...PREV count: {0} current index: {1}, setting current index to: {2}, currentCanvas name: {3}", m_canvasList.Count, currentIndex, currentIndex - 1, CurrentCanvas.name);
        CurrentCanvas.SetActive(false);

        CurrentCanvas = m_canvasList[currentIndex - 1];
        CurrentCanvas.SetActive(true);

        // Update the back button's visibility after hitting previous
        m_backButton.gameObject.SetActive(HasPrevious());
    }

    public bool HasNext()
    {
        int currentIndex = m_canvasList.IndexOf(CurrentCanvas);
        return currentIndex + 1 <= m_canvasList.Count - 1;
    }

    public bool HasPrevious()
    {
        int currentIndex = m_canvasList.IndexOf(CurrentCanvas);
        return currentIndex - 1 >= 0;
    }

    #endregion
}
