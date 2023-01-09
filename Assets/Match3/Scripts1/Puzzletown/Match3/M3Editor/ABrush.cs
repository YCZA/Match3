using System.Collections.Generic;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x020004FE RID: 1278
	public abstract class ABrush
	{
		// Token: 0x06002326 RID: 8998 RVA: 0x0009BF0E File Offset: 0x0009A30E
		public ABrush(Sprite sprite, ABrush removal = null, int rotationAngle = 0)
		{
			this.sprite = sprite;
			this.removal = removal;
			this.rotationAngle = rotationAngle;
		}

		// Token: 0x1700056E RID: 1390
		// (get) Token: 0x06002327 RID: 8999 RVA: 0x0009BF36 File Offset: 0x0009A336
		public Sprite Sprite
		{
			get
			{
				return this.sprite;
			}
		}

		// Token: 0x1700056F RID: 1391
		// (get) Token: 0x06002328 RID: 9000 RVA: 0x0009BF3E File Offset: 0x0009A33E
		public ABrush RemovalBrush
		{
			get
			{
				return this.removal;
			}
		}

		// Token: 0x17000570 RID: 1392
		// (get) Token: 0x06002329 RID: 9001 RVA: 0x0009BF46 File Offset: 0x0009A346
		public bool HasRemovalBrush
		{
			get
			{
				return this.removal != null;
			}
		}

		// Token: 0x17000571 RID: 1393
		// (get) Token: 0x0600232A RID: 9002 RVA: 0x0009BF54 File Offset: 0x0009A354
		public int RotationAngle
		{
			get
			{
				return this.rotationAngle;
			}
		}

		// Token: 0x17000572 RID: 1394
		// (get) Token: 0x0600232B RID: 9003 RVA: 0x0009BF5C File Offset: 0x0009A35C
		public virtual bool RequiresRefreshAll
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600232C RID: 9004 RVA: 0x0009BF60 File Offset: 0x0009A360
		public virtual void PaintField(Field field, Fields fields)
		{
			foreach (IBrushComponent brushComponent in this.brushComponents)
			{
				brushComponent.PaintField(field, fields);
			}
		}

		// Token: 0x04004EDC RID: 20188
		public List<IBrushComponent> brushComponents = new List<IBrushComponent>();

		// Token: 0x04004EDD RID: 20189
		protected Sprite sprite;

		// Token: 0x04004EDE RID: 20190
		protected ABrush removal;

		// Token: 0x04004EDF RID: 20191
		protected readonly int rotationAngle;
	}
}
