using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Match3.Scripts1.Shared.DataStructures;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000562 RID: 1378
	public static class LayoutGenerator
	{
		// Token: 0x06002423 RID: 9251 RVA: 0x000A0AC1 File Offset: 0x0009EEC1
		public static bool ReflectsHorizontally(IntVector2 pos, int size)
		{
			return pos.x < size / 2;
		}

		// Token: 0x06002424 RID: 9252 RVA: 0x000A0ACF File Offset: 0x0009EECF
		public static IntVector2 HorizontallyReflectedPos(IntVector2 pos, int size)
		{
			return new IntVector2(size - 1 - pos.x, pos.y);
		}

		// Token: 0x06002425 RID: 9253 RVA: 0x000A0AE8 File Offset: 0x0009EEE8
		public static bool ReflectsVertically(IntVector2 pos, int size)
		{
			return pos.y < size / 2;
		}

		// Token: 0x06002426 RID: 9254 RVA: 0x000A0AF6 File Offset: 0x0009EEF6
		public static IntVector2 MirrorPosVerticallyReflectedPos(IntVector2 pos, int size)
		{
			return new IntVector2(pos.x, size - 1 - pos.y);
		}

		// Token: 0x06002427 RID: 9255 RVA: 0x000A0B0F File Offset: 0x0009EF0F
		public static bool ReflectsDiagonallyNW(IntVector2 pos, int size)
		{
			return pos.x + pos.y >= size - 1;
		}

		// Token: 0x06002428 RID: 9256 RVA: 0x000A0B28 File Offset: 0x0009EF28
		public static IntVector2 DiagonallyNWReflectedPos(IntVector2 pos, int size)
		{
			return new IntVector2(size - 1 - pos.y, size - 1 - pos.x);
		}

		// Token: 0x06002429 RID: 9257 RVA: 0x000A0B45 File Offset: 0x0009EF45
		public static bool ReflectsDiagonallyNE(IntVector2 pos, int size)
		{
			return pos.x > pos.y;
		}

		// Token: 0x0600242A RID: 9258 RVA: 0x000A0B57 File Offset: 0x0009EF57
		public static IntVector2 DiagonallyNEReflectedPos(IntVector2 pos, int size)
		{
			return new IntVector2(pos.y, pos.x);
		}

		// Token: 0x0600242B RID: 9259 RVA: 0x000A0B6C File Offset: 0x0009EF6C
		public static bool ReflectsPoint(IntVector2 pos, int size)
		{
			return pos.x >= pos.y;
		}

		// Token: 0x0600242C RID: 9260 RVA: 0x000A0B81 File Offset: 0x0009EF81
		public static IntVector2 PointReflectedPos(IntVector2 pos, int size)
		{
			return new IntVector2(size - 1 - pos.x, size - 1 - pos.y);
		}

		// Token: 0x0600242D RID: 9261 RVA: 0x000A0BA0 File Offset: 0x0009EFA0
		public static List<IntVector2> ReflectPositions(CanReflect canReflect, ReflectedPosition reflectedPos, List<IntVector2> positions, int size)
		{
			return (from pt in positions
			select reflectedPos(pt, size)).ToList<IntVector2>();
		}

		// Token: 0x0600242E RID: 9262 RVA: 0x000A0BD8 File Offset: 0x0009EFD8
		public static void ApplyReflection(Map<Field> fields, Reflection function)
		{
			int size = fields.size;
			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					IntVector2 pos = new IntVector2(i, j);
					LayoutGenerator.ReflectLayout(fields, pos, size, function);
				}
			}
		}

		// Token: 0x0600242F RID: 9263 RVA: 0x000A0C24 File Offset: 0x0009F024
		private static void ReflectLayout(Map<Field> fields, IntVector2 pos, int size, Reflection function)
		{
			if (!function.canReflect(pos, size))
			{
				return;
			}
			IntVector2 vec = function.reflectedPos(pos, size);
			bool isOn = fields[vec].isOn && fields[pos].isOn;
			fields[vec].isOn = isOn;
			fields[pos].isOn = isOn;
		}

		// Token: 0x06002430 RID: 9264 RVA: 0x000A0C90 File Offset: 0x0009F090
		public static void ReflectRemainingModifiers(Map<Field> fields, Reflection function)
		{
			int size = fields.size;
			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					IntVector2 intVector = new IntVector2(i, j);
					if (function.canReflect(intVector, size))
					{
						Field field = fields[intVector];
						Field field2 = fields[function.reflectedPos(intVector, size)];
						if (field.HasGem && field.gem.IsStackedGem)
						{
							LayoutGenerator.ReflectStackedGem(field, field2);
						}
						else if (field2.HasGem && field2.gem.IsStackedGem)
						{
							LayoutGenerator.ReflectStackedGem(field2, field);
						}
					}
				}
			}
		}

		// Token: 0x06002431 RID: 9265 RVA: 0x000A0D54 File Offset: 0x0009F154
		public static void ReflectLayoutModifiers(Map<Field> fields, Reflection function)
		{
			int size = fields.size;
			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					IntVector2 intVector = new IntVector2(i, j);
					if (function.canReflect(intVector, size))
					{
						Field field = fields[intVector];
						Field field2 = fields[function.reflectedPos(intVector, size)];
						if (field.numChains > 0)
						{
							LayoutGenerator.ReflectChain(field, field2);
						}
						else if (field2.numChains > 0)
						{
							LayoutGenerator.ReflectChain(field2, field);
						}
						if (field.blockerIndex > 0 && !field2.gem.IsDroppable)
						{
							LayoutGenerator.ReflectStone(field, field2);
						}
						else if (field2.blockerIndex > 0 && !field.gem.IsDroppable)
						{
							LayoutGenerator.ReflectStone(field2, field);
						}
						if (field.HasGem && field.gem.color == GemColor.Cannonball && !field2.gem.IsDroppable)
						{
							LayoutGenerator.ReflectCannonball(field, field2);
						}
						else if (field2.gem.color == GemColor.Cannonball && !field.gem.IsDroppable)
						{
							LayoutGenerator.ReflectCannonball(field2, field);
						}
					}
				}
			}
		}

		// Token: 0x06002432 RID: 9266 RVA: 0x000A0EB6 File Offset: 0x0009F2B6
		private static void ReflectChain(Field origin, Field reflected)
		{
			if (!reflected.HasGem)
			{
				reflected.AssignGem(new Gem(GemColor.Random));
			}
			reflected.blockerIndex = 0;
			reflected.numChains = origin.numChains;
		}

		// Token: 0x06002433 RID: 9267 RVA: 0x000A0EE2 File Offset: 0x0009F2E2
		private static void ReflectStone(Field origin, Field reflected)
		{
			reflected.HasGem = false;
			reflected.numChains = 0;
			reflected.blockerIndex = origin.blockerIndex;
		}

		// Token: 0x06002434 RID: 9268 RVA: 0x000A0EFE File Offset: 0x0009F2FE
		private static void ReflectStackedGem(Field origin, Field reflected)
		{
			if (reflected.HasGem)
			{
				reflected.gem.color = origin.gem.color;
				reflected.gem.type = origin.gem.type;
			}
		}

		// Token: 0x06002435 RID: 9269 RVA: 0x000A0F37 File Offset: 0x0009F337
		private static void ReflectCannonball(Field origin, Field reflected)
		{
			if (!reflected.HasGem)
			{
				reflected.AssignGem(new Gem(GemColor.Cannonball));
			}
			else
			{
				reflected.gem.color = GemColor.Cannonball;
			}
			reflected.blockerIndex = 0;
		}

		// Token: 0x06002436 RID: 9270 RVA: 0x000A0F6C File Offset: 0x0009F36C
		// Note: this type is marked as 'beforefieldinit'.
		static LayoutGenerator()
		{
			if (LayoutGenerator._003C_003Ef__mg_0024cache0 == null)
			{
				LayoutGenerator._003C_003Ef__mg_0024cache0 = new CanReflect(LayoutGenerator.ReflectsPoint);
			}
			CanReflect canReflect = LayoutGenerator._003C_003Ef__mg_0024cache0;
			if (LayoutGenerator._003C_003Ef__mg_0024cache1 == null)
			{
				LayoutGenerator._003C_003Ef__mg_0024cache1 = new ReflectedPosition(LayoutGenerator.PointReflectedPos);
			}
			LayoutGenerator.ReflectPoint = new Reflection(canReflect, LayoutGenerator._003C_003Ef__mg_0024cache1);
			if (LayoutGenerator._003C_003Ef__mg_0024cache2 == null)
			{
				LayoutGenerator._003C_003Ef__mg_0024cache2 = new CanReflect(LayoutGenerator.ReflectsDiagonallyNE);
			}
			CanReflect canReflect2 = LayoutGenerator._003C_003Ef__mg_0024cache2;
			if (LayoutGenerator._003C_003Ef__mg_0024cache3 == null)
			{
				LayoutGenerator._003C_003Ef__mg_0024cache3 = new ReflectedPosition(LayoutGenerator.DiagonallyNEReflectedPos);
			}
			LayoutGenerator.ReflectDiagonallyNE = new Reflection(canReflect2, LayoutGenerator._003C_003Ef__mg_0024cache3);
			if (LayoutGenerator._003C_003Ef__mg_0024cache4 == null)
			{
				LayoutGenerator._003C_003Ef__mg_0024cache4 = new CanReflect(LayoutGenerator.ReflectsDiagonallyNW);
			}
			CanReflect canReflect3 = LayoutGenerator._003C_003Ef__mg_0024cache4;
			if (LayoutGenerator._003C_003Ef__mg_0024cache5 == null)
			{
				LayoutGenerator._003C_003Ef__mg_0024cache5 = new ReflectedPosition(LayoutGenerator.DiagonallyNWReflectedPos);
			}
			LayoutGenerator.ReflectDiagonallyNW = new Reflection(canReflect3, LayoutGenerator._003C_003Ef__mg_0024cache5);
			if (LayoutGenerator._003C_003Ef__mg_0024cache6 == null)
			{
				LayoutGenerator._003C_003Ef__mg_0024cache6 = new CanReflect(LayoutGenerator.ReflectsVertically);
			}
			CanReflect canReflect4 = LayoutGenerator._003C_003Ef__mg_0024cache6;
			if (LayoutGenerator._003C_003Ef__mg_0024cache7 == null)
			{
				LayoutGenerator._003C_003Ef__mg_0024cache7 = new ReflectedPosition(LayoutGenerator.MirrorPosVerticallyReflectedPos);
			}
			LayoutGenerator.ReflectVertically = new Reflection(canReflect4, LayoutGenerator._003C_003Ef__mg_0024cache7);
			if (LayoutGenerator._003C_003Ef__mg_0024cache8 == null)
			{
				LayoutGenerator._003C_003Ef__mg_0024cache8 = new CanReflect(LayoutGenerator.ReflectsHorizontally);
			}
			CanReflect canReflect5 = LayoutGenerator._003C_003Ef__mg_0024cache8;
			if (LayoutGenerator._003C_003Ef__mg_0024cache9 == null)
			{
				LayoutGenerator._003C_003Ef__mg_0024cache9 = new ReflectedPosition(LayoutGenerator.HorizontallyReflectedPos);
			}
			LayoutGenerator.ReflectHorizontally = new Reflection(canReflect5, LayoutGenerator._003C_003Ef__mg_0024cache9);
		}

		// Token: 0x04004F95 RID: 20373
		public static Reflection ReflectPoint;

		// Token: 0x04004F96 RID: 20374
		public static Reflection ReflectDiagonallyNE;

		// Token: 0x04004F97 RID: 20375
		public static Reflection ReflectDiagonallyNW;

		// Token: 0x04004F98 RID: 20376
		public static Reflection ReflectVertically;

		// Token: 0x04004F99 RID: 20377
		public static Reflection ReflectHorizontally;

		// Token: 0x04004F9A RID: 20378
		[CompilerGenerated]
		private static CanReflect _003C_003Ef__mg_0024cache0;

		// Token: 0x04004F9B RID: 20379
		[CompilerGenerated]
		private static ReflectedPosition _003C_003Ef__mg_0024cache1;

		// Token: 0x04004F9C RID: 20380
		[CompilerGenerated]
		private static CanReflect _003C_003Ef__mg_0024cache2;

		// Token: 0x04004F9D RID: 20381
		[CompilerGenerated]
		private static ReflectedPosition _003C_003Ef__mg_0024cache3;

		// Token: 0x04004F9E RID: 20382
		[CompilerGenerated]
		private static CanReflect _003C_003Ef__mg_0024cache4;

		// Token: 0x04004F9F RID: 20383
		[CompilerGenerated]
		private static ReflectedPosition _003C_003Ef__mg_0024cache5;

		// Token: 0x04004FA0 RID: 20384
		[CompilerGenerated]
		private static CanReflect _003C_003Ef__mg_0024cache6;

		// Token: 0x04004FA1 RID: 20385
		[CompilerGenerated]
		private static ReflectedPosition _003C_003Ef__mg_0024cache7;

		// Token: 0x04004FA2 RID: 20386
		[CompilerGenerated]
		private static CanReflect _003C_003Ef__mg_0024cache8;

		// Token: 0x04004FA3 RID: 20387
		[CompilerGenerated]
		private static ReflectedPosition _003C_003Ef__mg_0024cache9;
	}
}
