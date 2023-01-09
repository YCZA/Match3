using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020004FC RID: 1276
	public class FieldSerializerProxy
	{
		// Token: 0x06002317 RID: 8983 RVA: 0x0009B943 File Offset: 0x00099D43
		public FieldSerializerProxy(ALevelLoader loader)
		{
			this.loader = loader;
		}

		// Token: 0x1700056C RID: 1388
		// (get) Token: 0x06002318 RID: 8984 RVA: 0x0009B952 File Offset: 0x00099D52
		// (set) Token: 0x06002319 RID: 8985 RVA: 0x0009B95A File Offset: 0x00099D5A
		public Move PrevMove { get; private set; }

		// Token: 0x1700056D RID: 1389
		// (get) Token: 0x0600231A RID: 8986 RVA: 0x0009B963 File Offset: 0x00099D63
		public SerializableFields PrevStepFields
		{
			get
			{
				return JsonUtility.FromJson<SerializableFields>(this.prevStep);
			}
		}

		// Token: 0x0600231B RID: 8987 RVA: 0x0009B970 File Offset: 0x00099D70
		public void PrevStep()
		{
			if (this.PrevStepFields != null)
			{
				this.Load(this.PrevStepFields.Deserialize());
			}
		}

		// Token: 0x0600231C RID: 8988 RVA: 0x0009B98E File Offset: 0x00099D8E
		public void HandleStepBegin(Fields fields, Move move)
		{
			if (fields.IsSwapPossible(move.from, move.to))
			{
				this.prevStep = JsonUtility.ToJson(new SerializableFields(fields));
				this.PrevMove = move;
			}
		}

		// Token: 0x0600231D RID: 8989 RVA: 0x0009B9C1 File Offset: 0x00099DC1
		public void Load(Fields fields)
		{
			this.loader.LoadBoard(fields);
		}

		// Token: 0x0600231E RID: 8990 RVA: 0x0009B9D0 File Offset: 0x00099DD0
		public void LoadFromPlayerPrefs()
		{
			Fields fields = FieldSerializer.LoadFromPlayerPrefs();
			this.Load(fields);
		}

		// Token: 0x04004ED3 RID: 20179
		private string prevStep;

		// Token: 0x04004ED4 RID: 20180
		private ALevelLoader loader;
	}
}
