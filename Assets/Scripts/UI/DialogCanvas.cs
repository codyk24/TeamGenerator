using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SAS.UI
{
	public enum Accent
	{
		None,
		Correct,
		Informational,
		Caution,
		Warning
	};

	public class DialogCanvas : BaseCanvasControl<DialogCanvas>
    {
		#region Fields

		[SerializeField]
		private TextMeshProUGUI m_title;

		[SerializeField]
		private TextMeshProUGUI m_body;

		[SerializeField]
		private ButtonInfo m_primary;

		[SerializeField]
		private ButtonInfo m_secondary;

		[SerializeField]
		private ButtonInfo m_tertiary;

		[SerializeField, Range(0f, 1f)]
		private float m_alpha = 0.8f;

		[Header("Accent Settings")]

		[SerializeField]
		private Image m_accent;

		[SerializeField]
		private Color m_correctColor;

		[SerializeField]
		private Color m_directionalColor;

		[SerializeField]
		private Color m_cautionColor;

		[SerializeField]
		private Color m_warningColor;

		private Queue<DialogInfo> m_queue = new Queue<DialogInfo>();

#if UNITY_LOCALIZATION
		private HashSet<LocalizeTextMeshUGUI> m_registered = new HashSet<LocalizeTextMeshUGUI>();
		private HashSet<LocalizeTextMeshUGUI> m_changing = new HashSet<LocalizeTextMeshUGUI>();
#endif
		#endregion

		#region Events

		public event System.EventHandler Changed;

		#endregion

		#region Properties

		internal string titleText => m_title.text;
		internal string bodyText => m_body.text;
		internal string primaryLabel => m_primary.label.text;
		internal string secondaryLabel => m_secondary.label.text;
		internal string tertiaryLabel => m_tertiary.label.text;

		internal bool anyTextChanging
		{
			get
			{
#if UNITY_LOCALIZATION
				return m_changing.Count > 0;
#else
				return false;
#endif
			}
		}

		#endregion

		#region Methods

		private void Start()
		{
			m_primary.button.onClick.AddListener(m_primary.Click);
			m_secondary.button.onClick.AddListener(m_secondary.Click);
			m_tertiary.button.onClick.AddListener(m_tertiary.Click);
		}

		public void Show(string title, string message, Accent accent, string primaryLabel, bool ignoreLocalization = false)
		{
			ShowInternal(title, message, accent, primaryLabel, null, ignoreLocalization);
		}

		private void ShowInternal(string title, string message, Accent accent, string primaryLabel, System.Action primaryAction, bool ignoreLocalization)
		{
			ShowInternal(title, message, accent, primaryLabel, string.Empty, string.Empty, primaryAction, null, null, ignoreLocalization);
		}

		public void ShowInternal(string title, string message, Accent accent, string primaryLabel, string secondaryLabel, System.Action primaryAction, System.Action secondaryAction, bool ignoreLocalization)
		{
			ShowInternal(title, message, accent, primaryLabel, secondaryLabel, string.Empty, primaryAction, secondaryAction, null, ignoreLocalization);
		}

		public void ShowInternal(string title, string message, Accent accent, string primaryLabel, string secondaryLabel, string tertiaryLabel, System.Action primaryAction, System.Action secondaryAction, System.Action tertiaryAction, bool ignoreLocalization)
		{
			Enqueue(new DialogInfo()
			{
				title = title,
				message = message,
				accent = accent,
				primaryLabel = primaryLabel,
				secondaryLabel = secondaryLabel,
				tertiaryLabel = tertiaryLabel,
				ignoreLocalization = ignoreLocalization,
				primaryAction = primaryAction,
				secondaryAction = secondaryAction,
				tertiaryAction = tertiaryAction,
			});
		}

		private void Enqueue(DialogInfo info)
		{
			m_queue.Enqueue(info);

			if (!showing)
			{
				NextDialog();
			}
		}

		private void NextDialog()
		{
			if (m_queue.Count == 0)
				return;

			var info = m_queue.Dequeue();

			m_secondary.button.gameObject.SetActive(!string.IsNullOrWhiteSpace(info.secondaryLabel));
			m_tertiary.button.gameObject.SetActive(!string.IsNullOrWhiteSpace(info.tertiaryLabel));


#if UNITY_LOCALIZATION
			if (!info.ignoreLocalization)
			{
				RegisterLocalizeTextMesh(m_title, info.title);
				RegisterLocalizeTextMesh(m_body, info.message);
				RegisterLocalizeTextMesh(m_primary.label, info.primaryLabel);
				RegisterLocalizeTextMesh(m_secondary.label, info.secondaryLabel);
				RegisterLocalizeTextMesh(m_tertiary.label, info.tertiaryLabel);
			}
			else
            {
				SetText(info);

				LocalizeTextMeshUGUI.ClearKey(m_title.gameObject);
				LocalizeTextMeshUGUI.ClearKey(m_body.gameObject);
				LocalizeTextMeshUGUI.ClearKey(m_primary.label.gameObject);
				LocalizeTextMeshUGUI.ClearKey(m_secondary.label.gameObject);
				LocalizeTextMeshUGUI.ClearKey(m_tertiary.label.gameObject);
			}
#else
			SetText(info);
#endif

			m_accent.color = GetAccentColor(info.accent);
			m_primary.action = info.primaryAction;
			m_secondary.action = info.secondaryAction;
			m_tertiary.action = info.tertiaryAction;

			//var color = OverlayCanvas.Instance.background.color;
			//color.a = m_alpha;
			//OverlayCanvas.Instance.background.color = color;

			ShowInternal();
		}

		private void SetText(DialogInfo info)
        {
			m_title.text = info.title;
			m_body.text = info.message;
			m_primary.label.text = info.primaryLabel;
			m_secondary.label.text = info.secondaryLabel;
			m_tertiary.label.text = info.tertiaryLabel;

			LayoutRebuilder.MarkLayoutForRebuild(m_canvas.GetComponent<RectTransform>());
			Changed?.Invoke(this, System.EventArgs.Empty);
		}

        //private void RegisterLocalizeTextMesh(TextMeshProUGUI textMesh, string key)
        //{
        //	if (LocalizeTextMeshUGUI.Initialize(textMesh.gameObject, key, out LocalizeTextMeshUGUI localizeTextMesh) && !m_registered.Contains(localizeTextMesh))
        //	{
        //		localizeTextMesh.TextChanging += LocalizeTextMesh_TextChanging;
        //		localizeTextMesh.TextChanged += LocalizeTextMesh_TextChanged;
        //		m_registered.Add(localizeTextMesh);
        //	}
        //}

        //private void LocalizeTextMesh_TextChanging(object sender, System.EventArgs e)
        //{
        //	m_changing.Add(sender as LocalizeTextMeshUGUI);
        //}

        //private void LocalizeTextMesh_TextChanged(object sender, System.EventArgs e)
        //{
        //	var localizeTextMesh = sender as LocalizeTextMeshUGUI;
        //	m_changing.Remove(localizeTextMesh);

        //	if (Exists)
        //	{
        //		this.WaitForNextFrame(() =>
        //		{
        //			LayoutRebuilder.MarkLayoutForRebuild(localizeTextMesh.GetComponent<RectTransform>());
        //			Changed?.Invoke(this, System.EventArgs.Empty);
        //		});
        //	}
        //}

        protected override void HideInternal()
		{
			if (m_queue.Count > 0)
			{
				NextDialog();
			}
			else
			{
				base.HideInternal();
			}
		}

		private Color GetAccentColor(Accent accent)
		{
			switch (accent)
			{
				case Accent.Correct:
					return m_correctColor;

				case Accent.Informational:
					return m_directionalColor;

				case Accent.Caution:
					return m_warningColor;

				case Accent.Warning:
					return m_warningColor;
			}
			return Color.clear;
		}

		internal void InvokePrimary() => m_primary.button.onClick.Invoke();
		internal void InvokeSecondary() => m_secondary.button.onClick.Invoke();
		internal void InvokeTertiary() => m_tertiary.button.onClick.Invoke();

		#endregion

		#region Static Methods

		public static void Show(string title, string message, Accent accent, string primaryLabel, System.Action primaryAction = null, bool ignoreLocalization = false)
		{
			Instance.ShowInternal(title, message, accent, primaryLabel, primaryAction, ignoreLocalization);
		}

		public static void Show(string title, string message, Accent accent, string primaryLabel, string secondaryLabel, System.Action primaryAction = null, System.Action secondaryAction = null, bool ignoreLocalization = false)
		{
			Instance.ShowInternal(title, message, accent, primaryLabel, secondaryLabel, primaryAction, secondaryAction, ignoreLocalization);
		}

		public static void Show(string title, string message, Accent accent, string primaryLabel, string secondaryLabel, string tertiaryLabel, System.Action primaryAction = null, System.Action secondaryAction = null, System.Action tertiaryAction = null, bool ignoreLocalization = false)
		{
			Instance.ShowInternal(title, message, accent, primaryLabel, secondaryLabel, tertiaryLabel, primaryAction, secondaryAction, tertiaryAction, ignoreLocalization);
		}

		public static void Clear()
		{
			Instance.m_queue.Clear();
			Instance.HideInternal();
		}

		#endregion

		#region Structures

		private class DialogInfo
		{
			public string title;
			public string message;
			public Accent accent;

			public string primaryLabel;
			public string secondaryLabel;
			public string tertiaryLabel;

			public bool ignoreLocalization;

			public System.Action primaryAction;
			public System.Action secondaryAction;
			public System.Action tertiaryAction;
		}

		#endregion
	}
}
