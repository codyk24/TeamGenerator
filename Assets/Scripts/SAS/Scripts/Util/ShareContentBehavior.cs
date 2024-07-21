using SAS.Managers;
using UnityEngine;
using NativeShareNamespace;

public class ShareContentBehavior : MonoBehaviour
{
    #region Members

    private NativeShare m_nativeShare;

    #endregion

    #region Methods

    // Start is called before the first frame update
    void Start()
    {
        m_nativeShare = new NativeShare();
        m_nativeShare.Clear();
    }

    public void ShareContent()
    {
        string teamString = TeamManager.Instance.PrintTeams();
        m_nativeShare.SetText(teamString);
        m_nativeShare.Share();
    }

    #endregion
}
