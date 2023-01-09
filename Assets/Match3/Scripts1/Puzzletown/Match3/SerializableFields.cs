using System;
using System.Collections;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020004F9 RID: 1273
	[Serializable]
	public class SerializableFields
	{
		// Token: 0x06002312 RID: 8978 RVA: 0x0009B2D0 File Offset: 0x000996D0
		public SerializableFields(Fields fields)
		{
			this.fields = new M3SerializableField[fields.size * fields.size];
			int num = 0;
			IEnumerator enumerator = fields.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Field field = (Field)obj;
					this.fields[num++] = new M3SerializableField(field);
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

		// Token: 0x06002313 RID: 8979 RVA: 0x0009B35C File Offset: 0x0009975C
		public Fields Deserialize()
		{
			int num = (int)Mathf.Sqrt((float)this.fields.Length);
			Fields fields = new Fields(num);
			for (int i = 0; i < num; i++)
			{
				for (int j = 0; j < num; j++)
				{
					IntVector2 intVector = new IntVector2(i, j);
					int num2 = i * num + j;
					fields[intVector] = this.fields[num2].Deserialize();
					IntVector2 position = (!fields[intVector].HasGem) ? IntVector2.Zero : intVector;
					fields[intVector].gem.position = position;
				}
			}
			return fields;
		}

		// Token: 0x04004ECE RID: 20174
		public M3SerializableField[] fields;
	}
}
