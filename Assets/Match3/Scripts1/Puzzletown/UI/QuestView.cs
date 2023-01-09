using System.Linq;
using Match3.Scripts1.Puzzletown.Town;
using Match3.Scripts1.Wooga.UI;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x02000A08 RID: 2568
	public abstract class QuestView : ATableViewReusableCell, IDataView<QuestUIData>, IEditorDescription
	{
		// Token: 0x1700091C RID: 2332
		// (get) Token: 0x06003DD0 RID: 15824
		public abstract QuestView.CellType questStatus { get; }

		// Token: 0x1700091D RID: 2333
		// (get) Token: 0x06003DD1 RID: 15825 RVA: 0x001392F0 File Offset: 0x001376F0
		// (set) Token: 0x06003DD2 RID: 15826 RVA: 0x001392F8 File Offset: 0x001376F8
		private protected QuestsPopupRoot root {  get; private set; }

		// Token: 0x1700091E RID: 2334
		// (get) Token: 0x06003DD3 RID: 15827 RVA: 0x00139301 File Offset: 0x00137701
		// (set) Token: 0x06003DD4 RID: 15828 RVA: 0x00139309 File Offset: 0x00137709
		public QuestData Quest { get; private set; }

		// Token: 0x1700091F RID: 2335
		// (get) Token: 0x06003DD5 RID: 15829 RVA: 0x00139312 File Offset: 0x00137712
		// (set) Token: 0x06003DD6 RID: 15830 RVA: 0x0013931A File Offset: 0x0013771A
		private protected QuestProgress Progress {  get; private set; }

		// Token: 0x17000920 RID: 2336
		// (get) Token: 0x06003DD7 RID: 15831 RVA: 0x00139323 File Offset: 0x00137723
		public override int reusableId
		{
			get
			{
				return (int)this.questStatus;
			}
		}

		// Token: 0x06003DD8 RID: 15832 RVA: 0x0013932C File Offset: 0x0013772C
		public string GetEditorDescription()
		{
			return this.questStatus.ToString();
		}

		// Token: 0x06003DD9 RID: 15833 RVA: 0x0013934D File Offset: 0x0013774D
		protected string GetTitleKey(QuestData quest)
		{
			return "quest.title." + quest.id;
		}

		// Token: 0x06003DDA RID: 15834 RVA: 0x0013935F File Offset: 0x0013775F
		protected string GetDescriptionKey(QuestData quest)
		{
			return "quest.description." + quest.id;
		}

		// Token: 0x06003DDB RID: 15835 RVA: 0x00139371 File Offset: 0x00137771
		protected string GetClaimKey(QuestData quest)
		{
			return "quest.claim." + quest.id;
		}

		// Token: 0x06003DDC RID: 15836 RVA: 0x00139384 File Offset: 0x00137784
		public virtual void Show(QuestUIData data)
		{
			if (data != null)
			{
				this.Quest = data.data;
				this.Progress = data.progress;
			}
			if (this.root == null)
			{
				this.root = base.GetComponentsInParent<QuestsPopupRoot>(true).FirstOrDefault<QuestsPopupRoot>();
			}
			if (this.root != null && data != null && data.progress != null)
			{
				bool showTitle = data.progress.status != QuestProgress.Status.chapterIntro;
				this.root.ShowTitle(showTitle);
			}
		}

		// Token: 0x040066BB RID: 26299
		public SwipeView swipeView;

		// Token: 0x02000A09 RID: 2569
		public enum CellType
		{
			// Token: 0x040066C0 RID: 26304
			pending,
			// Token: 0x040066C1 RID: 26305
			done,
			// Token: 0x040066C2 RID: 26306
			current,
			// Token: 0x040066C3 RID: 26307
			endOfContent,
			// Token: 0x040066C4 RID: 26308
			dateLocked,
			// Token: 0x040066C5 RID: 26309
			chapterIntro
		}
	}
}
