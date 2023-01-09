using Match3.Scripts1.Wooga.UI;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x02000A05 RID: 2565
	public class QuestTaskProgressView : ATableViewReusableCell, IDataView<QuestViewCurrent.TaskProgressViewData>, IEditorDescription, IHandler<PopupOperation>
	{
		// Token: 0x06003DC9 RID: 15817 RVA: 0x001391D0 File Offset: 0x001375D0
		public void Show(QuestViewCurrent.TaskProgressViewData taskProgressViewData)
		{
			this.data = taskProgressViewData;
			if (this.label)
			{
				this.label.text = this.data.label;
			}
			if (this.material)
			{
				this.material.Show(new MaterialAmount
				{
					type = this.data.taskData.item,
					MaxAmount = this.data.taskData.count,
					amount = this.data.countProgress
				});
			}
		}

		// Token: 0x06003DCA RID: 15818 RVA: 0x0013926E File Offset: 0x0013766E
		public void Handle(PopupOperation op)
		{
			if (op == PopupOperation.Details)
			{
				base.GetComponentInParent<QuestViewCurrent>().ActionButton(this.data);
			}
		}

		// Token: 0x1700091B RID: 2331
		// (get) Token: 0x06003DCB RID: 15819 RVA: 0x00139292 File Offset: 0x00137692
		public override int reusableId
		{
			get
			{
				return (int)this.usage;
			}
		}

		// Token: 0x06003DCC RID: 15820 RVA: 0x0013929A File Offset: 0x0013769A
		public string GetEditorDescription()
		{
			return this.usage.ToString();
		}

		// Token: 0x040066B1 RID: 26289
		private QuestViewCurrent.TaskProgressViewData data;

		// Token: 0x040066B2 RID: 26290
		public Text label;

		// Token: 0x040066B3 RID: 26291
		public QuestTaskProgressView.State usage;

		// Token: 0x040066B4 RID: 26292
		public Button button;

		// Token: 0x040066B5 RID: 26293
		public MaterialAmountView material;

		// Token: 0x02000A06 RID: 2566
		public enum State
		{
			// Token: 0x040066B7 RID: 26295
			Pending,
			// Token: 0x040066B8 RID: 26296
			Active,
			// Token: 0x040066B9 RID: 26297
			Collectable,
			// Token: 0x040066BA RID: 26298
			Completed
		}
	}
}
