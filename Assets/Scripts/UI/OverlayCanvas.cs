using UnityEngine;
using UnityEngine.UI;

namespace SAS.UI
{
	public class OverlayCanvas : BaseCanvasControl<OverlayCanvas>
	{
		#region Fields

		[SerializeField]
		private Image m_background;

		private int m_count;

		#endregion

		#region Properties

		protected override bool useOverlay => false;

		public int count
		{
			get => m_count;
			set
			{
				if (m_count == value)
					return;

				var prevCount = m_count;
				m_count = value;

				if (m_count > 0 && prevCount == 0)
				{
					ShowInternal();
				}
				else if (m_count == 0 && prevCount > 0)
				{
					HideInternal();
				}
			}
		}

		public int sortingOrder => m_canvas.sortingOrder;
		public Image background => m_background;

		#endregion

		#region Methods

		protected override void ShowInternal()
		{
			//++CameraManager.CastInstance.IgnoreDrawCount;
			base.ShowInternal();
		}

		protected override void HideInternal()
		{
			base.HideInternal();
			//--CameraManager.CastInstance.IgnoreDrawCount;
		}

		#endregion
	}
}
