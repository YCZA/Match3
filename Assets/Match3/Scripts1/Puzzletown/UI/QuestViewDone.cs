using System.Collections;
using DG.Tweening;
using Match3.Scripts1.UnityEngine;
using Wooga.Coroutines;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x02000A0E RID: 2574
	public class QuestViewDone : QuestView
	{
		// Token: 0x17000924 RID: 2340
		// (get) Token: 0x06003DF1 RID: 15857 RVA: 0x00139ED8 File Offset: 0x001382D8
		public override QuestView.CellType questStatus
		{
			get
			{
				return QuestView.CellType.done;
			}
		}

		// Token: 0x06003DF2 RID: 15858 RVA: 0x00139EDC File Offset: 0x001382DC
		public override void Show(QuestUIData data)
		{
			base.Show(data);
			if (base.root == null)
			{
				return;
			}
			this.questUiData = data;
			string text = base.root.loc.GetText(base.GetTitleKey(data.data), new LocaParam[0]);
			string text2 = base.root.loc.GetText(this.GetCompletedDescriptionKey(data.data), new LocaParam[0]);
			PopupTextCell.SetText(this, TextType.Title, text);
			PopupTextCell.SetText(this, TextType.Content, text2);
			this.illustration.gameObject.SetActive(false);
			base.StartCoroutine(this.LoadAssetsRoutine());
			base.transform.HandleOnParent(data.data);
		}

		// Token: 0x06003DF3 RID: 15859 RVA: 0x00139F90 File Offset: 0x00138390
		private void SetIllustration(Sprite sprite)
		{
			if (sprite != null)
			{
				this.illustration.sprite = sprite;
			}
			this.illustration.gameObject.SetActive(true);
			this.illustration.DOFade(0f, 0.2f).From<Tweener>();
		}

		// Token: 0x06003DF4 RID: 15860 RVA: 0x00139FE1 File Offset: 0x001383E1
		protected string GetCompletedDescriptionKey(QuestData quest)
		{
			return "quest.roundup." + quest.id;
		}

		// Token: 0x06003DF5 RID: 15861 RVA: 0x00139FF4 File Offset: 0x001383F4
		private IEnumerator LoadAssetsRoutine()
		{
			Wooroutine<Sprite> spriteFlow = new QuestIllustrationSpriteFlow().Start(this.questUiData.data);
			yield return spriteFlow;
			this.SetIllustration(spriteFlow.ReturnValue);
			yield break;
		}

		// Token: 0x040066D6 RID: 26326
		public Image illustration;

		// Token: 0x040066D7 RID: 26327
		private QuestUIData questUiData;
	}
}
