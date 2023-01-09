using System;
using Match3.Scripts1.UnityEngine;
using Match3.Scripts1.Wooga.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x020009FD RID: 2557
	public class QuestDataSource : ArrayDataSource<QuestView, QuestUIData>
	{
		// Token: 0x06003D9E RID: 15774 RVA: 0x00137930 File Offset: 0x00135D30
		private void DoPanRequest(ChapterInfo chapter, ChapterConfig config)
		{
			ATableViewReusableCell atableViewReusableCell = null;
			foreach (ATableViewReusableCell atableViewReusableCell2 in this.tableView.GetAllCells())
			{
				QuestView questView = atableViewReusableCell2 as QuestView;
				if (questView.Quest != null)
				{
					int num = config.ChapterForLevel(questView.Quest.level);
					if (num == chapter.id)
					{
						if (atableViewReusableCell == null)
						{
							atableViewReusableCell = atableViewReusableCell2;
						}
						if (questView.questStatus == QuestView.CellType.current)
						{
							atableViewReusableCell = atableViewReusableCell2;
							break;
						}
						if (atableViewReusableCell2.transform.localPosition.y > atableViewReusableCell.transform.localPosition.y)
						{
							atableViewReusableCell = atableViewReusableCell2;
						}
					}
				}
			}
			if (atableViewReusableCell != null)
			{
				RectTransform component = atableViewReusableCell.GetComponent<RectTransform>();
				float num2 = (!(component != null)) ? 0f : component.rect.center.y;
				this.viewPortRect.content.transform.localPosition = new Vector3(0f, -(atableViewReusableCell.transform.localPosition.y + num2 - this.viewPortRect.viewport.rect.center.y), 0f);
			}
		}

		// Token: 0x06003D9F RID: 15775 RVA: 0x00137ABC File Offset: 0x00135EBC
		private Rect GetRectTransformWorldRect(RectTransform trans)
		{
			Vector3[] array = new Vector3[4];
			trans.GetWorldCorners(array);
			return new Rect(array[0].x, array[0].y, array[1].x - array[0].x, array[1].y - array[0].y);
		}

		// Token: 0x06003DA0 RID: 15776 RVA: 0x00137B28 File Offset: 0x00135F28
		private void FixedUpdate()
		{
			ATableViewReusableCell atableViewReusableCell = null;
			this.tableView.GetVisibleCells(this.viewPortRect.viewport, this.viewPortRect.content);
			float num = 1000000f;
			Rect rectTransformWorldRect = this.GetRectTransformWorldRect(this.viewPortRect.viewport);
			foreach (ATableViewReusableCell atableViewReusableCell2 in this.tableView.GetVisibleCells(this.viewPortRect.viewport, this.viewPortRect.content))
			{
				if (atableViewReusableCell == null)
				{
					atableViewReusableCell = atableViewReusableCell2;
				}
				RectTransform component = atableViewReusableCell2.GetComponent<RectTransform>();
				Rect rectTransformWorldRect2 = this.GetRectTransformWorldRect(component);
				float num2 = Math.Abs(rectTransformWorldRect.center.y - rectTransformWorldRect2.center.y);
				if (num2 < num)
				{
					atableViewReusableCell = atableViewReusableCell2;
					num = num2;
				}
			}
			if (atableViewReusableCell != null)
			{
				QuestView component2 = atableViewReusableCell.GetComponent<QuestView>();
				if (component2 != null && component2.Quest != null && component2.Quest.level != this.currentLevel)
				{
					this.currentLevel = component2.Quest.level;
					this.HandleOnParent(new QuestDataSource.CurrentLevelNumber
					{
						levelNumber = this.currentLevel
					});
				}
			}
		}

		// Token: 0x06003DA1 RID: 15777 RVA: 0x00137CA0 File Offset: 0x001360A0
		public override int GetReusableIdForIndex(int index)
		{
			QuestUIData dataForIndex = this.GetDataForIndex(index);
			if (dataForIndex == null || dataForIndex.progress == null)
			{
				return 0;
			}
			QuestProgress.Status status = dataForIndex.progress.status;
			switch (status)
			{
			case QuestProgress.Status.undefined:
			case QuestProgress.Status.locked:
			case QuestProgress.Status.unstarted:
				break;
			default:
				if (status != QuestProgress.Status.unlocked)
				{
					if (status == QuestProgress.Status.chapterIntro)
					{
						return 5;
					}
					if (status != QuestProgress.Status.collected && status != QuestProgress.Status.done)
					{
						throw new ArgumentOutOfRangeException();
					}
					return 1;
				}
				break;
			case QuestProgress.Status.started:
			case QuestProgress.Status.completed:
				return 2;
			}
			return 0;
		}

		// Token: 0x04006683 RID: 26243
		[SerializeField]
		private ScrollRect viewPortRect;

		// Token: 0x04006684 RID: 26244
		private int currentLevel = -1;

		// Token: 0x020009FE RID: 2558
		public struct CurrentLevelNumber
		{
			// Token: 0x04006685 RID: 26245
			public int levelNumber;
		}
	}
}
