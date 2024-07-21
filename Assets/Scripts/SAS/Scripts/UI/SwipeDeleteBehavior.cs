using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeDeleteBehavior : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    #region Members
    public Transform topView;

    private Vector3 mouseStartPosition;
    private Vector3 topViewStartPosition;

    private float deleteThreshold = -500f;

    #endregion

    #region Methods

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.LogFormat("DEBUG... OnBeginDrag");

        mouseStartPosition = Input.mousePosition;
        topViewStartPosition = topView.localPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 distance = Input.mousePosition - mouseStartPosition;

        float newPositionX = topViewStartPosition.x + Mathf.Min(0f, distance.x);
        topView.localPosition = new Vector3(newPositionX, topViewStartPosition.y, topViewStartPosition.z);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector3 distance = Input.mousePosition - mouseStartPosition;
        Debug.LogFormat("DEBUG... OnEndDrag: Distance.x: {0}", distance.x);

        if (distance.x < deleteThreshold)
        {
            Destroy(transform.parent.gameObject);
        }
        else
        {
            topView.localPosition = topViewStartPosition;
        }
    }

    #endregion
}
