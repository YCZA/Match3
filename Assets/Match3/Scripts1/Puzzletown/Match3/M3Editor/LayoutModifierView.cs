using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000564 RID: 1380
	public class LayoutModifierView : MonoBehaviour
	{
		// Token: 0x0600243F RID: 9279 RVA: 0x000A1188 File Offset: 0x0009F588
		private void Start()
		{
			this.btnFlipVertical.onClick.AddListener(delegate()
			{
				if (LayoutModifierView._003C_003Ef__mg_0024cache0 == null)
				{
					LayoutModifierView._003C_003Ef__mg_0024cache0 = new ReflectedPosition(LayoutGenerator.HorizontallyReflectedPos);
				}
				this.ApplyModification(LayoutModifierView._003C_003Ef__mg_0024cache0);
			});
			this.btnFlipHorizontal.onClick.AddListener(delegate()
			{
				if (LayoutModifierView._003C_003Ef__mg_0024cache1 == null)
				{
					LayoutModifierView._003C_003Ef__mg_0024cache1 = new ReflectedPosition(LayoutGenerator.MirrorPosVerticallyReflectedPos);
				}
				this.ApplyModification(LayoutModifierView._003C_003Ef__mg_0024cache1);
			});
			this.btnRotateCW.onClick.AddListener(delegate()
			{
				if (LayoutModifierView._003C_003Ef__mg_0024cache2 == null)
				{
					LayoutModifierView._003C_003Ef__mg_0024cache2 = new ReflectedPosition(LayoutModifier.CWRotatedPos);
				}
				this.ApplyModification(LayoutModifierView._003C_003Ef__mg_0024cache2);
			});
			this.btnRotateCCW.onClick.AddListener(delegate()
			{
				if (LayoutModifierView._003C_003Ef__mg_0024cache3 == null)
				{
					LayoutModifierView._003C_003Ef__mg_0024cache3 = new ReflectedPosition(LayoutModifier.CCWRotatedPos);
				}
				this.ApplyModification(LayoutModifierView._003C_003Ef__mg_0024cache3);
			});
			this.btnShiftLeft.onClick.AddListener(delegate()
			{
				if (LayoutModifierView._003C_003Ef__mg_0024cache4 == null)
				{
					LayoutModifierView._003C_003Ef__mg_0024cache4 = new ReflectedPosition(LayoutModifier.LeftShiftedPos);
				}
				this.ApplyModification(LayoutModifierView._003C_003Ef__mg_0024cache4);
			});
			this.btnShiftRight.onClick.AddListener(delegate()
			{
				if (LayoutModifierView._003C_003Ef__mg_0024cache5 == null)
				{
					LayoutModifierView._003C_003Ef__mg_0024cache5 = new ReflectedPosition(LayoutModifier.RightShiftedPos);
				}
				this.ApplyModification(LayoutModifierView._003C_003Ef__mg_0024cache5);
			});
			this.btnShiftUp.onClick.AddListener(delegate()
			{
				if (LayoutModifierView._003C_003Ef__mg_0024cache6 == null)
				{
					LayoutModifierView._003C_003Ef__mg_0024cache6 = new ReflectedPosition(LayoutModifier.UpShiftedPos);
				}
				this.ApplyModification(LayoutModifierView._003C_003Ef__mg_0024cache6);
			});
			this.btnShiftDown.onClick.AddListener(delegate()
			{
				if (LayoutModifierView._003C_003Ef__mg_0024cache7 == null)
				{
					LayoutModifierView._003C_003Ef__mg_0024cache7 = new ReflectedPosition(LayoutModifier.DownShiftedPos);
				}
				this.ApplyModification(LayoutModifierView._003C_003Ef__mg_0024cache7);
			});
		}

		// Token: 0x06002440 RID: 9280 RVA: 0x000A1278 File Offset: 0x0009F678
		public void ApplyModification(ReflectedPosition modifiedPosition)
		{
			Fields fields = new Fields(this.levelEditor.fields.size);
			IEnumerator enumerator = this.levelEditor.fields.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Field field = (Field)obj;
					this.RemoveColorWheelCorner(field);
					field.gridPosition = modifiedPosition(field.gridPosition, fields.size);
					fields[field.gridPosition] = field;
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
			this.ReAddColorWheelCorners(fields);
			LayoutModifierView.LogBrokenColorWheels(fields);
			this.levelLoader.LoadBoard(fields);
			this.levelEditor.fields = fields;
		}

		// Token: 0x06002441 RID: 9281 RVA: 0x000A1344 File Offset: 0x0009F744
		private void RemoveColorWheelCorner(Field field)
		{
			if (field.IsColorWheelCorner)
			{
				field.blockerIndex = ((!field.IsColorWheelVariant) ? 10 : 12);
			}
		}

		// Token: 0x06002442 RID: 9282 RVA: 0x000A136C File Offset: 0x0009F76C
		private static void LogBrokenColorWheels(Fields fields)
		{
			IEnumerator enumerator = fields.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Field field = (Field)obj;
					if (field.IsColorWheel && fields.GetColorWheelCorner(field.gridPosition) == default(IntVector2))
					{
						WoogaDebug.LogWarningFormatted("Found a color wheel with no corner at position {0}", new object[]
						{
							field.gridPosition
						});
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
		}

		// Token: 0x06002443 RID: 9283 RVA: 0x000A1410 File Offset: 0x0009F810
		private void ReAddColorWheelCorners(Fields fields)
		{
			for (int i = this.levelEditor.fields.size - 1; i >= 0; i--)
			{
				for (int j = 0; j < this.levelEditor.fields.size; j++)
				{
					IntVector2 intVector = new IntVector2(i, j);
					if (fields[intVector].IsColorWheel)
					{
						bool isColorWheelVariant = fields[intVector].IsColorWheelVariant;
						IntVector2 pos = intVector + IntVector2.Left;
						IntVector2 intVector2 = intVector + IntVector2.Left + IntVector2.Up;
						IntVector2 pos2 = intVector + IntVector2.Up;
						if (LayoutModifierView.IsMatchingColorWheel(fields, pos, isColorWheelVariant) && LayoutModifierView.IsMatchingColorWheel(fields, intVector2, isColorWheelVariant) && LayoutModifierView.IsMatchingColorWheel(fields, pos2, isColorWheelVariant))
						{
							fields[intVector2].blockerIndex = ((!isColorWheelVariant) ? 9 : 11);
						}
					}
				}
			}
		}

		// Token: 0x06002444 RID: 9284 RVA: 0x000A1504 File Offset: 0x0009F904
		private static bool IsMatchingColorWheel(Fields fields, IntVector2 pos, bool isVariant)
		{
			return fields.IsValid(pos) && fields[pos].IsColorWheel && fields[pos].IsColorWheelVariant == isVariant && !fields[pos].IsColorWheelCorner && fields.GetColorWheelCorner(pos) == default(IntVector2);
		}

		// Token: 0x04004FA4 RID: 20388
		[HideInInspector]
		public LevelEditor levelEditor;

		// Token: 0x04004FA5 RID: 20389
		[HideInInspector]
		public LevelLoaderEditor levelLoader;

		// Token: 0x04004FA6 RID: 20390
		[Header("Modifier Tools")]
		[SerializeField]
		private Button btnFlipVertical;

		// Token: 0x04004FA7 RID: 20391
		[SerializeField]
		private Button btnFlipHorizontal;

		// Token: 0x04004FA8 RID: 20392
		[SerializeField]
		private Button btnRotateCW;

		// Token: 0x04004FA9 RID: 20393
		[SerializeField]
		private Button btnRotateCCW;

		// Token: 0x04004FAA RID: 20394
		[SerializeField]
		private Button btnShiftLeft;

		// Token: 0x04004FAB RID: 20395
		[SerializeField]
		private Button btnShiftRight;

		// Token: 0x04004FAC RID: 20396
		[SerializeField]
		private Button btnShiftUp;

		// Token: 0x04004FAD RID: 20397
		[SerializeField]
		private Button btnShiftDown;

		// Token: 0x04004FAE RID: 20398
		[CompilerGenerated]
		private static ReflectedPosition _003C_003Ef__mg_0024cache0;

		// Token: 0x04004FAF RID: 20399
		[CompilerGenerated]
		private static ReflectedPosition _003C_003Ef__mg_0024cache1;

		// Token: 0x04004FB0 RID: 20400
		[CompilerGenerated]
		private static ReflectedPosition _003C_003Ef__mg_0024cache2;

		// Token: 0x04004FB1 RID: 20401
		[CompilerGenerated]
		private static ReflectedPosition _003C_003Ef__mg_0024cache3;

		// Token: 0x04004FB2 RID: 20402
		[CompilerGenerated]
		private static ReflectedPosition _003C_003Ef__mg_0024cache4;

		// Token: 0x04004FB3 RID: 20403
		[CompilerGenerated]
		private static ReflectedPosition _003C_003Ef__mg_0024cache5;

		// Token: 0x04004FB4 RID: 20404
		[CompilerGenerated]
		private static ReflectedPosition _003C_003Ef__mg_0024cache6;

		// Token: 0x04004FB5 RID: 20405
		[CompilerGenerated]
		private static ReflectedPosition _003C_003Ef__mg_0024cache7;
	}
}
