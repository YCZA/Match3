using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000BDD RID: 3037
	[ExecuteInEditMode]
	[RequireComponent(typeof(Image))]
	[AddComponentMenu("UI/Effects/Extensions/Shining Effect")]
	public class ShineEffector : MonoBehaviour
	{
		// Token: 0x17000A5A RID: 2650
		// (get) Token: 0x06004733 RID: 18227 RVA: 0x0016AB45 File Offset: 0x00168F45
		// (set) Token: 0x06004734 RID: 18228 RVA: 0x0016AB4D File Offset: 0x00168F4D
		public float YOffset
		{
			get
			{
				return this.yOffset;
			}
			set
			{
				this.ChangeVal(value);
				this.yOffset = value;
			}
		}

		// Token: 0x06004735 RID: 18229 RVA: 0x0016AB60 File Offset: 0x00168F60
		private void OnEnable()
		{
			if (this.effector == null)
			{
				GameObject gameObject = new GameObject("effector");
				this.effectRoot = new GameObject("ShineEffect");
				this.effectRoot.transform.SetParent(base.transform);
				this.effectRoot.AddComponent<Image>().sprite = base.gameObject.GetComponent<Image>().sprite;
				this.effectRoot.GetComponent<Image>().type = base.gameObject.GetComponent<Image>().type;
				this.effectRoot.AddComponent<Mask>().showMaskGraphic = false;
				this.effectRoot.transform.localScale = Vector3.one;
				this.effectRoot.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
				this.effectRoot.GetComponent<RectTransform>().anchorMax = Vector2.one;
				this.effectRoot.GetComponent<RectTransform>().anchorMin = Vector2.zero;
				this.effectRoot.GetComponent<RectTransform>().offsetMax = Vector2.zero;
				this.effectRoot.GetComponent<RectTransform>().offsetMin = Vector2.zero;
				this.effectRoot.transform.SetAsFirstSibling();
				gameObject.AddComponent<RectTransform>();
				gameObject.transform.SetParent(this.effectRoot.transform);
				this.effectorRect = gameObject.GetComponent<RectTransform>();
				this.effectorRect.localScale = Vector3.one;
				this.effectorRect.anchoredPosition3D = Vector3.zero;
				this.effectorRect.gameObject.AddComponent<ShineEffect>();
				this.effectorRect.anchorMax = Vector2.one;
				this.effectorRect.anchorMin = Vector2.zero;
				this.effectorRect.Rotate(0f, 0f, -8f);
				this.effector = gameObject.GetComponent<ShineEffect>();
				this.effectorRect.offsetMax = Vector2.zero;
				this.effectorRect.offsetMin = Vector2.zero;
				this.OnValidate();
			}
		}

		// Token: 0x06004736 RID: 18230 RVA: 0x0016AD54 File Offset: 0x00169154
		private void OnValidate()
		{
			this.effector.Yoffset = this.yOffset;
			this.effector.Width = this.width;
			if (this.yOffset <= -1f || this.yOffset >= 1f)
			{
				this.effectRoot.SetActive(false);
			}
			else if (!this.effectRoot.activeSelf)
			{
				this.effectRoot.SetActive(true);
			}
		}

		// Token: 0x06004737 RID: 18231 RVA: 0x0016ADD0 File Offset: 0x001691D0
		private void ChangeVal(float value)
		{
			this.effector.Yoffset = value;
			if (value <= -1f || value >= 1f)
			{
				this.effectRoot.SetActive(false);
			}
			else if (!this.effectRoot.activeSelf)
			{
				this.effectRoot.SetActive(true);
			}
		}

		// Token: 0x06004738 RID: 18232 RVA: 0x0016AE2C File Offset: 0x0016922C
		private void OnDestroy()
		{
			if (!Application.isPlaying)
			{
				global::UnityEngine.Object.DestroyImmediate(this.effectRoot);
			}
			else
			{
				global::UnityEngine.Object.Destroy(this.effectRoot);
			}
		}

		// Token: 0x04006E47 RID: 28231
		public ShineEffect effector;

		// Token: 0x04006E48 RID: 28232
		[SerializeField]
		[HideInInspector]
		private GameObject effectRoot;

		// Token: 0x04006E49 RID: 28233
		[Range(-1f, 1f)]
		public float yOffset = -1f;

		// Token: 0x04006E4A RID: 28234
		[Range(0.1f, 1f)]
		public float width = 0.5f;

		// Token: 0x04006E4B RID: 28235
		private RectTransform effectorRect;
	}
}
