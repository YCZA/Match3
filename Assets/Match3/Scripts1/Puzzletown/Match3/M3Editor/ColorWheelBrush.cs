using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000518 RID: 1304
	public class ColorWheelBrush : ABrush
	{
		// Token: 0x0600234B RID: 9035 RVA: 0x0009CA40 File Offset: 0x0009AE40
		public ColorWheelBrush(Sprite sprite, ABrush removal, bool isDefaultBrush = true) : base(sprite, removal, 0)
		{
			this.brushComponents.Add(new FieldBrushComponent(true));
			this.brushComponents.Add(new RemoveStoneBrushComponent());
			this.brushComponents.Add(new RemoveCratesBrushComponent());
			this.brushComponents.Add(new RemoveGemModifierComponent());
			this.brushComponents.Add(new RemoveDefinedGemSpawnComponent());
			this.brushComponents.Add(new RemoveGemBrushComponent());
			this.brushComponents.Add(new RemoveGrowingWindowComponent());
			this.brushComponents.Add(new ColorWheelBrushComponent(isDefaultBrush, false));
			this.cornerBrush = new ColorWheelBrushComponent(isDefaultBrush, true);
			this.removalBrush = new RemoveColorWheelBrushComponent();
		}

		// Token: 0x17000576 RID: 1398
		// (get) Token: 0x0600234C RID: 9036 RVA: 0x0009CAF1 File Offset: 0x0009AEF1
		public override bool RequiresRefreshAll
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600234D RID: 9037 RVA: 0x0009CAF4 File Offset: 0x0009AEF4
		public override void PaintField(Field field, Fields fields)
		{
			if (this.CanBePlaced(field, fields))
			{
				IEnumerable block = fields.GetBlock(field.gridPosition, 2, 2);
				IEnumerable block2 = fields.GetBlock(field.gridPosition, 2, 2);
				int num = 0;
				this.RemoveIntersectingColorWheels(block2, fields);
				IEnumerator enumerator = block.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						Field field2 = (Field)obj;
						foreach (IBrushComponent brushComponent in this.brushComponents)
						{
							if (field2 != null)
							{
								brushComponent.PaintField(field2, fields);
							}
						}
						if (num == 0)
						{
							this.cornerBrush.PaintField(field2, fields);
						}
						num++;
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
		}

		// Token: 0x0600234E RID: 9038 RVA: 0x0009CBF4 File Offset: 0x0009AFF4
		private bool CanBePlaced(Field field, Fields fields)
		{
			IEnumerator enumerator = fields.GetBlock(field.gridPosition, 2, 2).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					if ((Field)enumerator.Current == null)
					{
						return false;
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
			return true;
		}

		// Token: 0x0600234F RID: 9039 RVA: 0x0009CC6C File Offset: 0x0009B06C
		private void RemoveIntersectingColorWheels(IEnumerable fieldsToCheck, Fields fields)
		{
			List<IntVector2> list = new List<IntVector2>();
			IEnumerator enumerator = fieldsToCheck.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Field field = (Field)obj;
					this.CheckAndAddPosition(fields, field.gridPosition, list);
					IntVector2 neighbor = new IntVector2(field.gridPosition.x - 1, field.gridPosition.y);
					this.CheckAndAddPosition(fields, neighbor, list);
					neighbor = new IntVector2(field.gridPosition.x, field.gridPosition.y + 1);
					this.CheckAndAddPosition(fields, neighbor, list);
					neighbor = new IntVector2(field.gridPosition.x - 1, field.gridPosition.y + 1);
					this.CheckAndAddPosition(fields, neighbor, list);
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
			foreach (IntVector2 position in list)
			{
				this.RemoveColorWheel(position, fields);
			}
		}

		// Token: 0x06002350 RID: 9040 RVA: 0x0009CDA0 File Offset: 0x0009B1A0
		private void CheckAndAddPosition(Fields fields, IntVector2 neighbor, List<IntVector2> cornerPositions)
		{
			if (fields.IsValid(neighbor) && fields[neighbor] != null && fields[neighbor].IsColorWheelCorner)
			{
				cornerPositions.Add(fields[neighbor].gridPosition);
			}
		}

		// Token: 0x06002351 RID: 9041 RVA: 0x0009CDE0 File Offset: 0x0009B1E0
		private void RemoveColorWheel(IntVector2 position, Fields fields)
		{
			IEnumerable block = fields.GetBlock(position, 2, 2);
			IEnumerator enumerator = block.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Field field = (Field)obj;
					this.removalBrush.PaintField(field, fields);
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

		// Token: 0x04004EE0 RID: 20192
		private readonly IBrushComponent cornerBrush;

		// Token: 0x04004EE1 RID: 20193
		private readonly IBrushComponent removalBrush;
	}
}
