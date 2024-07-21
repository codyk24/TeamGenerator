using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SAS.UI
{
	public abstract class BaseCanvasControl<T> : BaseMonoSingleton<T>
		where T : MonoBehaviour
	{
		#region Fields

		[SerializeField]
		protected Canvas m_canvas;

		[SerializeField]
		protected RectTransform m_content;

		#endregion

		#region Events

		public event System.EventHandler Shown;
		public event System.EventHandler Hidden;

		#endregion

		#region Properties

		public virtual bool showing => m_canvas.gameObject.activeInHierarchy;

		protected virtual bool useOverlay => true;

		public static BaseCanvasControl<T> CastInstance => Instance as BaseCanvasControl<T>;

		#endregion

		#region Methods

		/// <summary>
		/// Method used to connect UnityEvents in editor
		/// </summary>
		public void Open()
        {
			Show();
        }

		/// <summary>
		/// Method used to connect UnityEvents in editor
		/// </summary>
		public void Close()
		{
			Hide();
		}

		protected virtual void ShowInternal()
		{
			if (useOverlay && !showing)
			{
				++OverlayCanvas.Instance.count;
			}

			m_canvas.gameObject.SetActive(true);
			LayoutRebuilder.ForceRebuildLayoutImmediate(m_content ?? m_canvas.GetComponent<RectTransform>());
			Shown?.Invoke(this, System.EventArgs.Empty);
		}

		protected virtual void HideInternal()
		{
			if (useOverlay && showing)
			{
				--OverlayCanvas.Instance.count;
			}

			m_canvas.gameObject.SetActive(false);
			Hidden?.Invoke(this, System.EventArgs.Empty);
		}

		#endregion

		#region Static Methods
		
		public static void Show()
		{
			CastInstance.ShowInternal();
		}
		
		public static void Hide()
		{
			CastInstance.HideInternal();
		}

		#endregion

		#region Structures

		[System.Serializable]
		internal class ButtonInfo
		{
			#region Fields

			public Button button;
			public TextMeshProUGUI label;
			public System.Action action;

			#endregion

			#region Methods

			public void Click()
			{
				action?.Invoke();
				CastInstance.HideInternal();
			}

			#endregion
		}

		#endregion
	}
}
