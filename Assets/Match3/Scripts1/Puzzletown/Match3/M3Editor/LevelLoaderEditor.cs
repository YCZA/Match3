using System;
using System.Collections;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000573 RID: 1395
	public class LevelLoaderEditor : ALevelLoader
	{
		// Token: 0x17000582 RID: 1410
		// (get) Token: 0x06002499 RID: 9369 RVA: 0x000A3278 File Offset: 0x000A1678
		public override Fields Fields
		{
			get
			{
				return this.fields;
			}
		}

		// Token: 0x0600249A RID: 9370 RVA: 0x000A3280 File Offset: 0x000A1680
		public override void LoadBoard(Fields fields)
		{
			this.fields = fields;
			this.viewExtension.RemoveIndicators();
			this.boardView.CreateViews(fields);
			this.boardView.RemoveHiddenItems();
			this.boardView.CreateHiddenItems(HiddenItemProcessor.GetHiddenItems(fields), null);
			this.boardView.RemoveColorWheels();
			this.boardView.CreateColorWheels(true);
			this.boardView.InitializeChameleonViews(true);
			IEnumerator enumerator = fields.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Field field = (Field)obj;
					FieldView fieldView = this.boardView.GetFieldView(field.gridPosition);
					this.viewExtension.DisplayFieldViewExtension(field, fieldView, this.boardView, fields);
					fieldView.gameObject.AddComponent<InputControllerEditor>();
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

		// Token: 0x0600249B RID: 9371 RVA: 0x000A336C File Offset: 0x000A176C
		public override APTBoardFactory CreateBoardFactory()
		{
			return new PTEditorBoardFactory();
		}

		// Token: 0x0400501D RID: 20509
		public BoardViewExtension viewExtension;

		// Token: 0x0400501E RID: 20510
		private Fields fields;
	}
}
