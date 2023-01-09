using System;
using System.Collections;
using Match3.Scripts1.Wooga.Signals;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x0200055E RID: 1374
	public class LevelEditor
	{
		// Token: 0x06002415 RID: 9237 RVA: 0x000A06AC File Offset: 0x0009EAAC
		public LevelEditor(Fields fields, Brushes brushes, BoardView boardView, BoardViewExtension viewExtension)
		{
			this.fields = fields;
			this.brushes = brushes;
			this.boardView = boardView;
			this.viewExtension = viewExtension;
			IEnumerator enumerator = fields.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Field field = (Field)obj;
					FieldView fieldView = boardView.GetFieldView(field.gridPosition);
					fieldView.gameObject.GetComponent<InputControllerEditor>().onClicked.AddListener(new Action<IntVector2>(this.HandleClick));
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
			this.viewExtension.DisplayRandomHiddenItems(boardView);
		}

		// Token: 0x06002416 RID: 9238 RVA: 0x000A0770 File Offset: 0x0009EB70
		public void HandleClick(IntVector2 gridPosition)
		{
			Field field = this.fields[gridPosition];
			this.brushes.CurrentBrush.PaintField(field, this.fields);
			if (this.brushes.CurrentBrush.RequiresRefreshAll)
			{
				IEnumerator enumerator = this.fields.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						Field field2 = (Field)obj;
						this.ShowField(field2);
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
			}
			else
			{
				this.ShowField(field);
			}
			this.boardView.UpdateDirtBorder();
			this.boardView.RecreateBorders();
			this.OnChanged.Dispatch();
		}

		// Token: 0x06002417 RID: 9239 RVA: 0x000A083C File Offset: 0x0009EC3C
		public void ResetAllFields()
		{
			IEnumerator enumerator = this.fields.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Field field = (Field)obj;
					this.brushes.resetFieldBrush.PaintField(field, this.fields);
					this.ShowField(field);
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
			this.boardView.UpdateDirtBorder();
			this.boardView.RecreateBorders();
			this.OnChanged.Dispatch();
		}

		// Token: 0x06002418 RID: 9240 RVA: 0x000A08DC File Offset: 0x0009ECDC
		public void ShowFields()
		{
			IEnumerator enumerator = this.fields.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Field field = (Field)obj;
					this.ShowField(field);
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
			this.boardView.UpdateDirtBorder();
			this.boardView.RecreateBorders();
		}

		// Token: 0x06002419 RID: 9241 RVA: 0x000A0958 File Offset: 0x0009ED58
		public void ShowField(Field field)
		{
			FieldView fieldView = this.boardView.GetFieldView(field.gridPosition);
			fieldView.Show(field);
			this.boardView.CreateConnectedWindowViews(field, this.fields);
			if (field.isGrowingWindow)
			{
				this.boardView.ShowGrowingWindow(field, this.fields);
			}
			else
			{
				this.boardView.ClearGrowingWindow(field, this.fields);
			}
			this.viewExtension.DisplayFieldViewExtension(field, fieldView, this.boardView, this.fields);
			this.viewExtension.RemoveHiddenItemViews(this.boardView);
			this.viewExtension.RemoveColorWheels(this.boardView);
			HiddenItemInfoDict hiddenItems = HiddenItemProcessor.GetHiddenItems(this.fields);
			this.boardView.CreateHiddenItems(hiddenItems, null);
			this.boardView.CreateColorWheels(true);
			this.viewExtension.DisplayRandomHiddenItems(this.boardView);
			GemView gemView = this.boardView.GetGemView(field.gridPosition, false);
			if (field.HasGem)
			{
				if (gemView == null)
				{
					gemView = this.boardView.CreateGemView(field.gem, fieldView.transform);
				}
				gemView.Show(field.gem);
			}
			else if (gemView != null)
			{
				this.boardView.ReleaseView(gemView, 0f);
			}
			this.boardView.InitializeChameleonViews(true);
		}

		// Token: 0x04004F8D RID: 20365
		public Fields fields;

		// Token: 0x04004F8E RID: 20366
		private readonly BoardViewExtension viewExtension;

		// Token: 0x04004F8F RID: 20367
		private readonly Brushes brushes;

		// Token: 0x04004F90 RID: 20368
		private readonly BoardView boardView;

		// Token: 0x04004F91 RID: 20369
		private Sprite fieldSprite;

		// Token: 0x04004F92 RID: 20370
		public readonly Signal OnChanged = new Signal();
	}
}
