using System;
using System.Collections.Generic;
using UnityEngine;

public class QueueItemVisibilityEventArgs : System.EventArgs
{
    public bool Visible;
    public int Index;

    public QueueItemVisibilityEventArgs(bool visible, int index)
    {
        Visible = visible;
        Index = index;
    }
}

public class CanvasQueueItem : MonoBehaviour
{
    #region Properties

    public int CanvasQueueIndex;

    #endregion

    #region Events

    public event EventHandler<QueueItemVisibilityEventArgs> VisibilityChanged;

    #endregion

    #region Methods

    void OnEnable()
    {
        VisibilityChanged?.Invoke(this, new QueueItemVisibilityEventArgs(true, CanvasQueueIndex));
    }

    void OnDisable()
    {
        VisibilityChanged?.Invoke(this, new QueueItemVisibilityEventArgs(false, CanvasQueueIndex));
    }

    #endregion
}
