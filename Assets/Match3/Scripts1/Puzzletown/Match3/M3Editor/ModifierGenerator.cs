using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Match3.Scripts1.Shared.DataStructures;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000571 RID: 1393
	public static class ModifierGenerator
	{
		// Token: 0x0600247E RID: 9342 RVA: 0x000A2898 File Offset: 0x000A0C98
		public static void RemoveSpawner(Map<Field> fields)
		{
			IEnumerator enumerator = fields.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Field field = (Field)obj;
					field.spawnType = SpawnTypes.None;
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

		// Token: 0x0600247F RID: 9343 RVA: 0x000A28F8 File Offset: 0x000A0CF8
		public static void PlaceSpawnerAtTopPosition(Map<Field> fields)
		{
			int size = fields.size;
			for (int i = 0; i < size; i++)
			{
				for (int j = size - 1; j >= 0; j--)
				{
					Field field = fields[i, j];
					if (field.isOn)
					{
						field.spawnType = SpawnTypes.NormalSpawn;
						break;
					}
				}
			}
		}

		// Token: 0x06002480 RID: 9344 RVA: 0x000A2954 File Offset: 0x000A0D54
		public static void PlaceDropExitAtBottomPosition(Map<Field> fields)
		{
			int size = fields.size;
			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					Field field = fields[i, j];
					if (field.isOn)
					{
						field.isDropSpawner = false;
						field.isDropExit = true;
						break;
					}
				}
			}
		}

		// Token: 0x06002481 RID: 9345 RVA: 0x000A29B4 File Offset: 0x000A0DB4
		public static int PlaceTilesOnAllFields(Map<Field> fields, int amount = 1)
		{
			int num = 0;
			int size = fields.size;
			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					Field field = fields[i, j];
					if (field.isOn)
					{
						field.numTiles = amount;
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x06002482 RID: 9346 RVA: 0x000A2A11 File Offset: 0x000A0E11
		public static bool IsSuitableForCut(Field field)
		{
			return field.isOn;
		}

		// Token: 0x06002483 RID: 9347 RVA: 0x000A2A19 File Offset: 0x000A0E19
		public static void CutField(Field field)
		{
			field.isOn = false;
		}

		// Token: 0x06002484 RID: 9348 RVA: 0x000A2A22 File Offset: 0x000A0E22
		public static bool IsSuitableForStone(Field field)
		{
			return field.isOn && field.blockerIndex == 0 && !field.HasGem;
		}

		// Token: 0x06002485 RID: 9349 RVA: 0x000A2A46 File Offset: 0x000A0E46
		public static bool IsSuitableForCannonball(Field field)
		{
			return field.isOn && !field.IsBlocked;
		}

		// Token: 0x06002486 RID: 9350 RVA: 0x000A2A5F File Offset: 0x000A0E5F
		public static void SetStone(Field field)
		{
			field.blockerIndex = 1;
		}

		// Token: 0x06002487 RID: 9351 RVA: 0x000A2A68 File Offset: 0x000A0E68
		public static void SetCannonball(Field field)
		{
			field.AssignGem(new Gem(GemColor.Cannonball));
		}

		// Token: 0x06002488 RID: 9352 RVA: 0x000A2A77 File Offset: 0x000A0E77
		public static bool IsSuitableForChain(Field field)
		{
			return field.isOn && field.numChains == 0 && !field.IsBlocked;
		}

		// Token: 0x06002489 RID: 9353 RVA: 0x000A2A9B File Offset: 0x000A0E9B
		public static void SetChain(Field field)
		{
			if (!field.HasGem)
			{
				field.AssignGem(new Gem(GemColor.Random));
			}
			field.numChains = 1;
		}

		// Token: 0x0600248A RID: 9354 RVA: 0x000A2ABB File Offset: 0x000A0EBB
		public static bool IsSuitableForStackedGem(Field field)
		{
			return field.isOn && field.gem.IsMatchable;
		}

		// Token: 0x0600248B RID: 9355 RVA: 0x000A2AD6 File Offset: 0x000A0ED6
		public static void SetStackedGem(Field field)
		{
			field.gem.type = GemType.StackedGemMedium;
		}

		// Token: 0x0600248C RID: 9356 RVA: 0x000A2AE4 File Offset: 0x000A0EE4
		public static void PlaceStackedGems(Map<Field> fields, GemColor color, List<Reflection> functions)
		{
			int size = fields.size;
			IntVector2 invalidPos = Fields.invalidPos;
			int num = 0;
			do
			{
				invalidPos = new IntVector2(RandomHelper.Next(size), RandomHelper.Next(size));
				num++;
			}
			while (!ModifierGenerator.GenerateStackedGems.isSuitable(fields[invalidPos]) && num < 1000);
			fields[invalidPos].gem.color = color;
			ModifierGenerator.GenerateStackedGems.modification(fields[invalidPos]);
		}

		// Token: 0x0600248D RID: 9357 RVA: 0x000A2B68 File Offset: 0x000A0F68
		public static void ModifyGroups(Map<Field> fields, int count, int minLength, int maxLength, FieldModification mod)
		{
			int i = 0;
			while (i < count)
			{
				int length = RandomHelper.Next(minLength, maxLength + 1);
				Map<Field> map = ModifierGenerator.ModifyGroup(fields, length, mod);
				if (map != null)
				{
					fields = map;
					i++;
				}
			}
		}

		// Token: 0x0600248E RID: 9358 RVA: 0x000A2BA4 File Offset: 0x000A0FA4
		public static Map<Field> ModifyGroup(Map<Field> fields, int length, FieldModification mod)
		{
			int size = fields.size;
			for (int i = 0; i < 1000; i++)
			{
				IntVector2 currentPos = new IntVector2(RandomHelper.Next(size), RandomHelper.Next(size));
				Map<Field> map = ModifierGenerator.ModifyGroupPos(fields, currentPos, length, mod);
				if (map != null)
				{
					return map;
				}
			}
			return null;
		}

		// Token: 0x0600248F RID: 9359 RVA: 0x000A2BF4 File Offset: 0x000A0FF4
		public static Map<Field> ModifyGroupPos(Map<Field> fields, IntVector2 currentPos, int remainingLength, FieldModification mod)
		{
			if (remainingLength == 0)
			{
				return fields;
			}
			int num = RandomHelper.Next(0, IntVector2.Sides.Length);
			int i = 0;
			int num2 = IntVector2.Sides.Length;
			while (i < num2)
			{
				int num3 = (num + i) % num2;
				IntVector2 intVector = currentPos + IntVector2.Sides[num3];
				Map<Field> map = fields.Clone() as Map<Field>;
				Field field = map[intVector];
				if (fields.IsValid(intVector) && mod.isSuitable(field))
				{
					mod.modification(field);
					map = ModifierGenerator.ModifyGroupPos(map, intVector, remainingLength - 1, mod);
					if (map != null)
					{
						return map;
					}
				}
				i++;
			}
			return null;
		}

		// Token: 0x06002490 RID: 9360 RVA: 0x000A2CB0 File Offset: 0x000A10B0
		// Note: this type is marked as 'beforefieldinit'.
		static ModifierGenerator()
		{
			if (ModifierGenerator._003C_003Ef__mg_0024cache0 == null)
			{
				ModifierGenerator._003C_003Ef__mg_0024cache0 = new IsFieldSuitable(ModifierGenerator.IsSuitableForCut);
			}
			IsFieldSuitable isSuitable = ModifierGenerator._003C_003Ef__mg_0024cache0;
			if (ModifierGenerator._003C_003Ef__mg_0024cache1 == null)
			{
				ModifierGenerator._003C_003Ef__mg_0024cache1 = new Modification(ModifierGenerator.CutField);
			}
			ModifierGenerator.GenerateCut = new FieldModification(isSuitable, ModifierGenerator._003C_003Ef__mg_0024cache1);
			if (ModifierGenerator._003C_003Ef__mg_0024cache2 == null)
			{
				ModifierGenerator._003C_003Ef__mg_0024cache2 = new IsFieldSuitable(ModifierGenerator.IsSuitableForStone);
			}
			IsFieldSuitable isSuitable2 = ModifierGenerator._003C_003Ef__mg_0024cache2;
			if (ModifierGenerator._003C_003Ef__mg_0024cache3 == null)
			{
				ModifierGenerator._003C_003Ef__mg_0024cache3 = new Modification(ModifierGenerator.SetStone);
			}
			ModifierGenerator.GenerateStones = new FieldModification(isSuitable2, ModifierGenerator._003C_003Ef__mg_0024cache3);
			if (ModifierGenerator._003C_003Ef__mg_0024cache4 == null)
			{
				ModifierGenerator._003C_003Ef__mg_0024cache4 = new IsFieldSuitable(ModifierGenerator.IsSuitableForChain);
			}
			IsFieldSuitable isSuitable3 = ModifierGenerator._003C_003Ef__mg_0024cache4;
			if (ModifierGenerator._003C_003Ef__mg_0024cache5 == null)
			{
				ModifierGenerator._003C_003Ef__mg_0024cache5 = new Modification(ModifierGenerator.SetChain);
			}
			ModifierGenerator.GenerateChains = new FieldModification(isSuitable3, ModifierGenerator._003C_003Ef__mg_0024cache5);
			if (ModifierGenerator._003C_003Ef__mg_0024cache6 == null)
			{
				ModifierGenerator._003C_003Ef__mg_0024cache6 = new IsFieldSuitable(ModifierGenerator.IsSuitableForStackedGem);
			}
			IsFieldSuitable isSuitable4 = ModifierGenerator._003C_003Ef__mg_0024cache6;
			if (ModifierGenerator._003C_003Ef__mg_0024cache7 == null)
			{
				ModifierGenerator._003C_003Ef__mg_0024cache7 = new Modification(ModifierGenerator.SetStackedGem);
			}
			ModifierGenerator.GenerateStackedGems = new FieldModification(isSuitable4, ModifierGenerator._003C_003Ef__mg_0024cache7);
			if (ModifierGenerator._003C_003Ef__mg_0024cache8 == null)
			{
				ModifierGenerator._003C_003Ef__mg_0024cache8 = new IsFieldSuitable(ModifierGenerator.IsSuitableForCannonball);
			}
			IsFieldSuitable isSuitable5 = ModifierGenerator._003C_003Ef__mg_0024cache8;
			if (ModifierGenerator._003C_003Ef__mg_0024cache9 == null)
			{
				ModifierGenerator._003C_003Ef__mg_0024cache9 = new Modification(ModifierGenerator.SetCannonball);
			}
			ModifierGenerator.GenerateCannonballs = new FieldModification(isSuitable5, ModifierGenerator._003C_003Ef__mg_0024cache9);
		}

		// Token: 0x04005007 RID: 20487
		public static FieldModification GenerateCut;

		// Token: 0x04005008 RID: 20488
		public static FieldModification GenerateStones;

		// Token: 0x04005009 RID: 20489
		public static FieldModification GenerateChains;

		// Token: 0x0400500A RID: 20490
		public static FieldModification GenerateStackedGems;

		// Token: 0x0400500B RID: 20491
		public static FieldModification GenerateCannonballs;

		// Token: 0x0400500C RID: 20492
		[CompilerGenerated]
		private static IsFieldSuitable _003C_003Ef__mg_0024cache0;

		// Token: 0x0400500D RID: 20493
		[CompilerGenerated]
		private static Modification _003C_003Ef__mg_0024cache1;

		// Token: 0x0400500E RID: 20494
		[CompilerGenerated]
		private static IsFieldSuitable _003C_003Ef__mg_0024cache2;

		// Token: 0x0400500F RID: 20495
		[CompilerGenerated]
		private static Modification _003C_003Ef__mg_0024cache3;

		// Token: 0x04005010 RID: 20496
		[CompilerGenerated]
		private static IsFieldSuitable _003C_003Ef__mg_0024cache4;

		// Token: 0x04005011 RID: 20497
		[CompilerGenerated]
		private static Modification _003C_003Ef__mg_0024cache5;

		// Token: 0x04005012 RID: 20498
		[CompilerGenerated]
		private static IsFieldSuitable _003C_003Ef__mg_0024cache6;

		// Token: 0x04005013 RID: 20499
		[CompilerGenerated]
		private static Modification _003C_003Ef__mg_0024cache7;

		// Token: 0x04005014 RID: 20500
		[CompilerGenerated]
		private static IsFieldSuitable _003C_003Ef__mg_0024cache8;

		// Token: 0x04005015 RID: 20501
		[CompilerGenerated]
		private static Modification _003C_003Ef__mg_0024cache9;
	}
}
