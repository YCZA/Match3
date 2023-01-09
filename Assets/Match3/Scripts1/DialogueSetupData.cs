using System;
using UnityEngine;

// Token: 0x020008C0 RID: 2240
namespace Match3.Scripts1
{
	[Serializable]
	public class DialogueSetupData
	{
		// Token: 0x17000858 RID: 2136
		// (get) Token: 0x060036A0 RID: 13984 RVA: 0x0010A7D3 File Offset: 0x00108BD3
		// (set) Token: 0x060036A1 RID: 13985 RVA: 0x0010A7DB File Offset: 0x00108BDB
		public DialogueTrigger Trigger { get; private set; }

		// Token: 0x17000859 RID: 2137
		// (get) Token: 0x060036A2 RID: 13986 RVA: 0x0010A7E4 File Offset: 0x00108BE4
		// (set) Token: 0x060036A3 RID: 13987 RVA: 0x0010A7EC File Offset: 0x00108BEC
		public CameraAfterStory CameraAfter { get; private set; }

		// Token: 0x1700085A RID: 2138
		// (get) Token: 0x060036A4 RID: 13988 RVA: 0x0010A7F5 File Offset: 0x00108BF5
		// (set) Token: 0x060036A5 RID: 13989 RVA: 0x0010A7FD File Offset: 0x00108BFD
		public string[] Characters { get; private set; }

		// Token: 0x060036A6 RID: 13990 RVA: 0x0010A808 File Offset: 0x00108C08
		public void Init()
		{
			this.Trigger = EnumExtensions.EnumParse<DialogueTrigger>(this.trigger_condition, true);
			this.CameraAfter = EnumExtensions.EnumParse<CameraAfterStory>(this.camera_after, true);
			this.Characters = this.characters.Split(new char[]
			{
				','
			});
		}

		// Token: 0x04005EB9 RID: 24249
		public string dialogue_id;

		// Token: 0x04005EBA RID: 24250
		public int island_id;

		// Token: 0x04005EBB RID: 24251
		[SerializeField]
		private string trigger_condition;

		// Token: 0x04005EBC RID: 24252
		public string trigger_value;

		// Token: 0x04005EBD RID: 24253
		public string place;

		// Token: 0x04005EBE RID: 24254
		[SerializeField]
		private string characters;

		// Token: 0x04005EBF RID: 24255
		[SerializeField]
		private string camera_after;
	}
}
