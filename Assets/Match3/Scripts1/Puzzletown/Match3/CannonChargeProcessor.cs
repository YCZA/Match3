using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Shared.Pooling;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020005FC RID: 1532
	public class CannonChargeProcessor : AMatchProcessor
	{
		// Token: 0x0600274F RID: 10063 RVA: 0x000AE724 File Offset: 0x000ACB24
		protected override void ProcessMatch(IMatchGroup match, Fields fields)
		{
			if (match.Group.Count == 0)
			{
				return;
			}
			if (!this.isInitialised)
			{
				this.CheckForCannons(fields);
			}
			for (int i = 0; i < match.Group.Count; i++)
			{
				Gem gem = match.Group[i];
				if (CannonChargeProcessor.colorCannonPositions.ContainsKey(gem.color) && !gem.IsCannon)
				{
					this.ChargeCannon(gem, fields, 1);
				}
			}
		}

		// Token: 0x06002750 RID: 10064 RVA: 0x000AE7A8 File Offset: 0x000ACBA8
		protected override void CheckSurroundings(IntVector2 pos, Fields fields)
		{
		}

		// Token: 0x06002751 RID: 10065 RVA: 0x000AE7AA File Offset: 0x000ACBAA
		protected override void CheckField(Field field, IntVector2 createdFrom)
		{
		}

		// Token: 0x06002752 RID: 10066 RVA: 0x000AE7AC File Offset: 0x000ACBAC
		private void ChargeCannon(Gem gem, Fields fields, int chargeAmount = 1)
		{
			List<IntVector2> list = CannonChargeProcessor.colorCannonPositions[gem.color];
			List<Gem> list2 = ListPool<Gem>.Create(10);
			foreach (IntVector2 vec in list)
			{
				if (fields[vec].CanBeCharged)
				{
					list2.Add(fields[vec].gem);
				}
			}
			if (list2.Count == 0)
			{
				return;
			}
			List<Gem> list3 = list2;
			if (CannonChargeProcessor._003C_003Ef__mg_0024cache0 == null)
			{
				CannonChargeProcessor._003C_003Ef__mg_0024cache0 = new Comparison<Gem>(GemExtensions.SortByParameterDescending);
			}
			list3.Sort(CannonChargeProcessor._003C_003Ef__mg_0024cache0);
			IntVector2 position = list2[0].position;
			Field field = fields[position];
			field.gem.parameter = field.gem.parameter + chargeAmount;
			this.results.Add(new CannonCharged(gem, position, chargeAmount));
			if (fields[position].gem.parameter == 45)
			{
				this.results.Add(new CannonExplosionTrigger(position));
				list.Remove(position);
			}
			ListPool<Gem>.Release(list2);
		}

		// Token: 0x06002753 RID: 10067 RVA: 0x000AE8EC File Offset: 0x000ACCEC
		private void CheckForCannons(Fields fields)
		{
			CannonChargeProcessor.colorCannonPositions.Clear();
			IEnumerator enumerator = fields.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Field field = (Field)obj;
					if (field.isOn && field.HasGem && field.gem.IsCannon)
					{
						if (!CannonChargeProcessor.colorCannonPositions.ContainsKey(field.gem.color))
						{
							List<IntVector2> value = ListPool<IntVector2>.Create(10);
							CannonChargeProcessor.colorCannonPositions.Add(field.gem.color, value);
						}
						CannonChargeProcessor.colorCannonPositions[field.gem.color].Add(field.gridPosition);
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
			this.isInitialised = true;
		}

		// Token: 0x040051D9 RID: 20953
		private bool isInitialised;

		// Token: 0x040051DA RID: 20954
		public static Dictionary<GemColor, List<IntVector2>> colorCannonPositions = new Dictionary<GemColor, List<IntVector2>>();

		// Token: 0x040051DB RID: 20955
		[CompilerGenerated]
		private static Comparison<Gem> _003C_003Ef__mg_0024cache0;
	}
}
