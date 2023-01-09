using System;
using System.Collections;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Wooga.UI;
using TMPro;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Town
{
	// Token: 0x020009A0 RID: 2464
	public class UiSpeechBubble : AnimatedUi, IHandler<PopupOperation>, IEditorDescription
	{
		// Token: 0x1700090B RID: 2315
		// (get) Token: 0x06003BC6 RID: 15302 RVA: 0x00128F28 File Offset: 0x00127328
		// (set) Token: 0x06003BC7 RID: 15303 RVA: 0x00128F30 File Offset: 0x00127330
		public VillagerView speaker { get; private set; }

		// Token: 0x1700090C RID: 2316
		// (get) Token: 0x06003BC8 RID: 15304 RVA: 0x00128F39 File Offset: 0x00127339
		public AudioService AudioService
		{
			get
			{
				if (this._audioService == null)
				{
					this._audioService = base.transform.root.GetComponent<TownOverheadUiRoot>().audioService;
				}
				return this._audioService;
			}
		}

		// Token: 0x06003BC9 RID: 15305 RVA: 0x00128F67 File Offset: 0x00127367
		private new void OnEnable()
		{
			base.SetVisibility(this.speaker != null);
			base.GetComponentsInChildren<CanvasGroup>(false).ForEach(new Action<CanvasGroup>(this.ResetCanvasGroup));
		}

		// Token: 0x06003BCA RID: 15306 RVA: 0x00128F93 File Offset: 0x00127393
		public new void OnDisable()
		{
			if (this.speaker != null && this.onSpeechOver != null)
			{
				this.onSpeechOver();
				this.speaker = null;
			}
			this.onSpeechOver = null;
		}

		// Token: 0x06003BCB RID: 15307 RVA: 0x00128FCC File Offset: 0x001273CC
		public IEnumerator Show(VillagerView _speaker, string name, string text, SBUsage usage, Action onSpeechOver = null)
		{
			GameObject tempGO = null;
			if (_speaker == null)
			{
				tempGO = new GameObject();
				this.speaker = tempGO.AddComponent<VillagerView>();
			}
			else
			{
				this.speaker = _speaker;
			}
			this.onSpeechOver = onSpeechOver;
			if (this.orientation == SBOrientation.None)
			{
				this.SetOrientation(this.speaker.Orientation, (usage != SBUsage.InGameDialogue) ? 0.75f : 1f);
			}
			if (usage == SBUsage.InGameDialogue)
			{
				base.transform.position = this.speaker.pivot;
			}
			if (this.speakerName)
			{
				this.speakerName.text = name;
			}
			// 设置立绘 todo
			this.label.text = text;
			this.active = true;
			AOverheadUiView.SortByDepth(base.transform);
			base.StopAllCoroutines();
			base.Show();
			this.AudioService.PlaySFX(AudioId.SpeechBubble, false, false, false);
			this.hideTime = (float)text.Length * 0.1f + 1f;
			while (this.active)
			{
				yield return null;
			}
			this.speaker = null;
			if (tempGO != null)
			{
				Destroy(tempGO);
			}
			this.onSpeechOver = null;
			base.Hide();
			yield return new WaitForSeconds(0.5f);
			yield break;
		}

		// Token: 0x06003BCC RID: 15308 RVA: 0x0012900C File Offset: 0x0012740C
		public void Skip()
		{
			this.active = false;
		}

		// Token: 0x06003BCD RID: 15309 RVA: 0x00129018 File Offset: 0x00127418
		private IEnumerator WaitForBubbleToDisappear(float time)
		{
			yield return new WaitForSeconds(time);
			this.active = false;
			yield break;
		}

		// Token: 0x06003BCE RID: 15310 RVA: 0x0012903C File Offset: 0x0012743C
		public void Update()
		{
			if (!this.canvas)
			{
				this.canvas = base.GetComponentInParent<Canvas>();
			}
			if (this.hideTime > 0f)
			{
				this.hideTime -= Time.deltaTime;
				if (this.hideTime <= 0f)
				{
					this.active = false;
				}
			}
		}

		// Token: 0x06003BCF RID: 15311 RVA: 0x001290A0 File Offset: 0x001274A0
		private void SetOrientation(SBOrientation orientation, float scale)
		{
			base.transform.localScale = new Vector3((orientation != SBOrientation.Right) ? (-scale) : scale, scale, scale);
			this.label.transform.localScale = new Vector3((float)((orientation != SBOrientation.Right) ? -1 : 1), 1f, 1f);
		}

		// Token: 0x06003BD0 RID: 15312 RVA: 0x001290FC File Offset: 0x001274FC
		public void Handle(PopupOperation op)
		{
			if (op == PopupOperation.Close)
			{
				this.active = false;
			}
		}

		// Token: 0x06003BD1 RID: 15313 RVA: 0x00129116 File Offset: 0x00127516
		public string GetEditorDescription()
		{
			return this.orientation.ToString();
		}

		// Token: 0x06003BD2 RID: 15314 RVA: 0x00129129 File Offset: 0x00127529
		private void ResetCanvasGroup(CanvasGroup group)
		{
			group.alpha = 1f;
			group.transform.localScale = Vector3.one;
			group.transform.localRotation = Quaternion.identity;
		}

		// Token: 0x040063E5 RID: 25573
		private const float TEXT_SPEED = 0.1f;

		// Token: 0x040063E6 RID: 25574
		private const float PAUSE = 1f;

		// Token: 0x040063E7 RID: 25575
		private const float COOLDOWN = 0.5f;

		// Token: 0x040063E8 RID: 25576
		public TMP_Text label;

		// Token: 0x040063E9 RID: 25577
		public TMP_Text speakerName;

		// Token: 0x040063EA RID: 25578
		public SBOrientation orientation;

		// Token: 0x040063EB RID: 25579
		private Canvas canvas;

		// Token: 0x040063EC RID: 25580
		private bool active;

		// Token: 0x040063ED RID: 25581
		private AudioService _audioService;

		// Token: 0x040063EE RID: 25582
		private float hideTime;

		// Token: 0x040063F0 RID: 25584
		private Action onSpeechOver;
	}
}
