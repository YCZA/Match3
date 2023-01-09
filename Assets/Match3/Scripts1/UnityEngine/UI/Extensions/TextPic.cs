using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000BBD RID: 3005
	[AddComponentMenu("UI/Extensions/TextPic")]
	[ExecuteInEditMode]
	public class TextPic : Text, IPointerClickHandler, IPointerExitHandler, IPointerEnterHandler, ISelectHandler, IEventSystemHandler
	{
		// Token: 0x17000A3B RID: 2619
		// (get) Token: 0x06004688 RID: 18056 RVA: 0x0016553B File Offset: 0x0016393B
		// (set) Token: 0x06004689 RID: 18057 RVA: 0x00165543 File Offset: 0x00163943
		public bool AllowClickParents
		{
			get
			{
				return this.m_ClickParents;
			}
			set
			{
				this.m_ClickParents = value;
			}
		}

		// Token: 0x0600468A RID: 18058 RVA: 0x0016554C File Offset: 0x0016394C
		public override void SetVerticesDirty()
		{
			base.SetVerticesDirty();
			this.UpdateQuadImage();
		}

		// Token: 0x0600468B RID: 18059 RVA: 0x0016555C File Offset: 0x0016395C
		private new void Start()
		{
			this.button = base.GetComponentInParent<Button>();
			if (this.button != null)
			{
				CanvasGroup canvasGroup = base.GetComponent<CanvasGroup>();
				if (canvasGroup == null)
				{
					canvasGroup = base.gameObject.AddComponent<CanvasGroup>();
				}
				canvasGroup.blocksRaycasts = false;
				this.highlightselectable = canvasGroup.GetComponent<Selectable>();
			}
			else
			{
				this.highlightselectable = base.GetComponent<Selectable>();
			}
			this.Reset_m_HrefInfos();
			base.Start();
		}

		// Token: 0x0600468C RID: 18060 RVA: 0x001655D8 File Offset: 0x001639D8
		protected void UpdateQuadImage()
		{
			this.m_OutputText = this.GetOutputText();
			this.m_ImagesVertexIndex.Clear();
			IEnumerator enumerator = TextPic.s_Regex.Matches(this.m_OutputText).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					System.Text.RegularExpressions.Match match = (System.Text.RegularExpressions.Match)obj;
					int index = match.Index;
					int item = index * 4 + 3;
					this.m_ImagesVertexIndex.Add(item);
					this.m_ImagesPool.RemoveAll((Image image) => image == null);
					if (this.m_ImagesPool.Count == 0)
					{
						base.GetComponentsInChildren<Image>(this.m_ImagesPool);
					}
					if (this.m_ImagesVertexIndex.Count > this.m_ImagesPool.Count)
					{
						GameObject gameObject = DefaultControls.CreateImage(default(DefaultControls.Resources));
						gameObject.layer = base.gameObject.layer;
						RectTransform rectTransform = gameObject.transform as RectTransform;
						if (rectTransform)
						{
							rectTransform.SetParent(base.rectTransform);
							rectTransform.localPosition = Vector3.zero;
							rectTransform.localRotation = Quaternion.identity;
							rectTransform.localScale = Vector3.one;
						}
						this.m_ImagesPool.Add(gameObject.GetComponent<Image>());
					}
					string value = match.Groups[1].Value;
					Image image2 = this.m_ImagesPool[this.m_ImagesVertexIndex.Count - 1];
					Vector2 b = Vector2.zero;
					if ((image2.sprite == null || image2.sprite.name != value) && this.inspectorIconList != null && this.inspectorIconList.Length > 0)
					{
						foreach (TextPic.IconName iconName in this.inspectorIconList)
						{
							if (iconName.name == value)
							{
								image2.sprite = iconName.sprite;
								image2.rectTransform.sizeDelta = new Vector2((float)base.fontSize * this.ImageScalingFactor * iconName.scale.x, (float)base.fontSize * this.ImageScalingFactor * iconName.scale.y);
								b = iconName.offset;
								break;
							}
						}
					}
					image2.enabled = true;
					if (this.positions.Count == this.m_ImagesPool.Count)
					{
						List<Vector2> list;
						int index2;
						image2.rectTransform.anchoredPosition = ((list = this.positions)[index2 = this.m_ImagesVertexIndex.Count - 1] = list[index2] + b);
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			for (int j = this.m_ImagesVertexIndex.Count; j < this.m_ImagesPool.Count; j++)
			{
				if (this.m_ImagesPool[j])
				{
					this.m_ImagesPool[j].gameObject.SetActive(false);
					this.m_ImagesPool[j].gameObject.hideFlags = HideFlags.HideAndDontSave;
					this.culled_ImagesPool.Add(this.m_ImagesPool[j].gameObject);
					this.m_ImagesPool.Remove(this.m_ImagesPool[j]);
				}
			}
			if (this.culled_ImagesPool.Count > 1)
			{
				this.clearImages = true;
			}
		}

		// Token: 0x0600468D RID: 18061 RVA: 0x001659A0 File Offset: 0x00163DA0
		protected override void OnPopulateMesh(VertexHelper toFill)
		{
			string text = this.m_Text;
			this.m_Text = this.GetOutputText();
			base.OnPopulateMesh(toFill);
			this.m_Text = text;
			this.positions.Clear();
			UIVertex vertex = default(UIVertex);
			for (int i = 0; i < this.m_ImagesVertexIndex.Count; i++)
			{
				int num = this.m_ImagesVertexIndex[i];
				RectTransform rectTransform = this.m_ImagesPool[i].rectTransform;
				Vector2 sizeDelta = rectTransform.sizeDelta;
				if (num < toFill.currentVertCount)
				{
					toFill.PopulateUIVertex(ref vertex, num);
					this.positions.Add(new Vector2(vertex.position.x + sizeDelta.x / 2f, vertex.position.y + sizeDelta.y / 2f) + this.imageOffset);
					toFill.PopulateUIVertex(ref vertex, num - 3);
					Vector3 position = vertex.position;
					int j = num;
					int num2 = num - 3;
					while (j > num2)
					{
						toFill.PopulateUIVertex(ref vertex, num);
						vertex.position = position;
						toFill.SetUIVertex(vertex, j);
						j--;
					}
				}
			}
			if (this.m_ImagesVertexIndex.Count != 0)
			{
				this.m_ImagesVertexIndex.Clear();
			}
			foreach (TextPic.HrefInfo hrefInfo in this.m_HrefInfos)
			{
				hrefInfo.boxes.Clear();
				if (hrefInfo.startIndex < toFill.currentVertCount)
				{
					toFill.PopulateUIVertex(ref vertex, hrefInfo.startIndex);
					Vector3 position2 = vertex.position;
					Bounds bounds = new Bounds(position2, Vector3.zero);
					int k = hrefInfo.startIndex;
					int endIndex = hrefInfo.endIndex;
					while (k < endIndex)
					{
						if (k >= toFill.currentVertCount)
						{
							break;
						}
						toFill.PopulateUIVertex(ref vertex, k);
						position2 = vertex.position;
						if (position2.x < bounds.min.x)
						{
							hrefInfo.boxes.Add(new Rect(bounds.min, bounds.size));
							bounds = new Bounds(position2, Vector3.zero);
						}
						else
						{
							bounds.Encapsulate(position2);
						}
						k++;
					}
					hrefInfo.boxes.Add(new Rect(bounds.min, bounds.size));
				}
			}
			this.UpdateQuadImage();
		}

		// Token: 0x17000A3C RID: 2620
		// (get) Token: 0x0600468E RID: 18062 RVA: 0x00165C6C File Offset: 0x0016406C
		// (set) Token: 0x0600468F RID: 18063 RVA: 0x00165C74 File Offset: 0x00164074
		public TextPic.HrefClickEvent onHrefClick
		{
			get
			{
				return this.m_OnHrefClick;
			}
			set
			{
				this.m_OnHrefClick = value;
			}
		}

		// Token: 0x06004690 RID: 18064 RVA: 0x00165C80 File Offset: 0x00164080
		protected string GetOutputText()
		{
			TextPic.s_TextBuilder.Length = 0;
			int num = 0;
			this.fixedString = this.text;
			if (this.inspectorIconList != null && this.inspectorIconList.Length > 0)
			{
				foreach (TextPic.IconName iconName in this.inspectorIconList)
				{
					if (iconName.name != null && iconName.name != string.Empty)
					{
						this.fixedString = this.fixedString.Replace(iconName.name, string.Concat(new object[]
						{
							"<quad name=",
							iconName.name,
							" size=",
							base.fontSize,
							" width=1 />"
						}));
					}
				}
			}
			int num2 = 0;
			IEnumerator enumerator = TextPic.s_HrefRegex.Matches(this.fixedString).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					System.Text.RegularExpressions.Match match = (System.Text.RegularExpressions.Match)obj;
					TextPic.s_TextBuilder.Append(this.fixedString.Substring(num, match.Index - num));
					TextPic.s_TextBuilder.Append("<color=" + this.hyperlinkColor + ">");
					Group group = match.Groups[1];
					if (this.isCreating_m_HrefInfos)
					{
						TextPic.HrefInfo item = new TextPic.HrefInfo
						{
							startIndex = TextPic.s_TextBuilder.Length * 4,
							endIndex = (TextPic.s_TextBuilder.Length + match.Groups[2].Length - 1) * 4 + 3,
							name = group.Value
						};
						this.m_HrefInfos.Add(item);
					}
					else if (this.m_HrefInfos.Count > 0)
					{
						this.m_HrefInfos[num2].startIndex = TextPic.s_TextBuilder.Length * 4;
						this.m_HrefInfos[num2].endIndex = (TextPic.s_TextBuilder.Length + match.Groups[2].Length - 1) * 4 + 3;
						num2++;
					}
					TextPic.s_TextBuilder.Append(match.Groups[2].Value);
					TextPic.s_TextBuilder.Append("</color>");
					num = match.Index + match.Length;
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			if (this.isCreating_m_HrefInfos)
			{
				this.isCreating_m_HrefInfos = false;
			}
			TextPic.s_TextBuilder.Append(this.fixedString.Substring(num, this.fixedString.Length - num));
			return TextPic.s_TextBuilder.ToString();
		}

		// Token: 0x06004691 RID: 18065 RVA: 0x00165F78 File Offset: 0x00164378
		public void OnPointerClick(PointerEventData eventData)
		{
			Vector2 point;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(base.rectTransform, eventData.position, eventData.pressEventCamera, out point);
			foreach (TextPic.HrefInfo hrefInfo in this.m_HrefInfos)
			{
				List<Rect> boxes = hrefInfo.boxes;
				for (int i = 0; i < boxes.Count; i++)
				{
					if (boxes[i].Contains(point))
					{
						this.m_OnHrefClick.Invoke(hrefInfo.name);
						return;
					}
				}
			}
		}

		// Token: 0x06004692 RID: 18066 RVA: 0x00166038 File Offset: 0x00164438
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (this.m_ImagesPool.Count >= 1)
			{
				foreach (Image image in this.m_ImagesPool)
				{
					if (this.highlightselectable != null && this.highlightselectable.isActiveAndEnabled)
					{
						image.color = this.highlightselectable.colors.highlightedColor;
					}
				}
			}
		}

		// Token: 0x06004693 RID: 18067 RVA: 0x001660D8 File Offset: 0x001644D8
		public void OnPointerExit(PointerEventData eventData)
		{
			if (this.m_ImagesPool.Count >= 1)
			{
				foreach (Image image in this.m_ImagesPool)
				{
					if (this.highlightselectable != null && this.highlightselectable.isActiveAndEnabled)
					{
						image.color = this.highlightselectable.colors.normalColor;
					}
					else
					{
						image.color = this.color;
					}
				}
			}
		}

		// Token: 0x06004694 RID: 18068 RVA: 0x0016618C File Offset: 0x0016458C
		public void OnSelect(BaseEventData eventData)
		{
			if (this.m_ImagesPool.Count >= 1)
			{
				foreach (Image image in this.m_ImagesPool)
				{
					if (this.highlightselectable != null && this.highlightselectable.isActiveAndEnabled)
					{
						image.color = this.highlightselectable.colors.highlightedColor;
					}
				}
			}
		}

		// Token: 0x06004695 RID: 18069 RVA: 0x0016622C File Offset: 0x0016462C
		private void Update()
		{
			object obj = this.thisLock;
			lock (obj)
			{
				if (this.clearImages)
				{
					for (int i = 0; i < this.culled_ImagesPool.Count; i++)
					{
						global::UnityEngine.Object.DestroyImmediate(this.culled_ImagesPool[i]);
					}
					this.culled_ImagesPool.Clear();
					this.clearImages = false;
				}
			}
			if (this.previousText != this.text)
			{
				this.Reset_m_HrefInfos();
			}
		}

		// Token: 0x06004696 RID: 18070 RVA: 0x001662C8 File Offset: 0x001646C8
		private void Reset_m_HrefInfos()
		{
			this.previousText = this.text;
			this.m_HrefInfos.Clear();
			this.isCreating_m_HrefInfos = true;
		}

		// Token: 0x04006DCB RID: 28107
		private readonly List<Image> m_ImagesPool = new List<Image>();

		// Token: 0x04006DCC RID: 28108
		private readonly List<GameObject> culled_ImagesPool = new List<GameObject>();

		// Token: 0x04006DCD RID: 28109
		private bool clearImages;

		// Token: 0x04006DCE RID: 28110
		private Object thisLock = new Object();

		// Token: 0x04006DCF RID: 28111
		private readonly List<int> m_ImagesVertexIndex = new List<int>();

		// Token: 0x04006DD0 RID: 28112
		private static readonly Regex s_Regex = new Regex("<quad name=(.+?) size=(\\d*\\.?\\d+%?) width=(\\d*\\.?\\d+%?) />", RegexOptions.Singleline);

		// Token: 0x04006DD1 RID: 28113
		private string fixedString;

		// Token: 0x04006DD2 RID: 28114
		[SerializeField]
		[Tooltip("Allow click events to be received by parents, (default) blocks")]
		private bool m_ClickParents;

		// Token: 0x04006DD3 RID: 28115
		private string m_OutputText;

		// Token: 0x04006DD4 RID: 28116
		public TextPic.IconName[] inspectorIconList;

		// Token: 0x04006DD5 RID: 28117
		[Tooltip("Global scaling factor for all images")]
		public float ImageScalingFactor = 1f;

		// Token: 0x04006DD6 RID: 28118
		public string hyperlinkColor = "blue";

		// Token: 0x04006DD7 RID: 28119
		[SerializeField]
		public Vector2 imageOffset = Vector2.zero;

		// Token: 0x04006DD8 RID: 28120
		private Button button;

		// Token: 0x04006DD9 RID: 28121
		private Selectable highlightselectable;

		// Token: 0x04006DDA RID: 28122
		private List<Vector2> positions = new List<Vector2>();

		// Token: 0x04006DDB RID: 28123
		private string previousText = string.Empty;

		// Token: 0x04006DDC RID: 28124
		public bool isCreating_m_HrefInfos = true;

		// Token: 0x04006DDD RID: 28125
		private readonly List<TextPic.HrefInfo> m_HrefInfos = new List<TextPic.HrefInfo>();

		// Token: 0x04006DDE RID: 28126
		private static readonly StringBuilder s_TextBuilder = new StringBuilder();

		// Token: 0x04006DDF RID: 28127
		private static readonly Regex s_HrefRegex = new Regex("<a href=([^>\\n\\s]+)>(.*?)(</a>)", RegexOptions.Singleline);

		// Token: 0x04006DE0 RID: 28128
		[SerializeField]
		private TextPic.HrefClickEvent m_OnHrefClick = new TextPic.HrefClickEvent();

		// Token: 0x02000BBE RID: 3006
		[Serializable]
		public struct IconName
		{
			// Token: 0x04006DE2 RID: 28130
			public string name;

			// Token: 0x04006DE3 RID: 28131
			public Sprite sprite;

			// Token: 0x04006DE4 RID: 28132
			public Vector2 offset;

			// Token: 0x04006DE5 RID: 28133
			public Vector2 scale;
		}

		// Token: 0x02000BBF RID: 3007
		[Serializable]
		public class HrefClickEvent : UnityEvent<string>
		{
		}

		// Token: 0x02000BC0 RID: 3008
		private class HrefInfo
		{
			// Token: 0x04006DE6 RID: 28134
			public int startIndex;

			// Token: 0x04006DE7 RID: 28135
			public int endIndex;

			// Token: 0x04006DE8 RID: 28136
			public string name;

			// Token: 0x04006DE9 RID: 28137
			public readonly List<Rect> boxes = new List<Rect>();
		}
	}
}
