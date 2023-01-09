using System;
using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Services;
using UnityEngine;

// Token: 0x020008C2 RID: 2242
namespace Match3.Scripts1
{
	[Serializable]
	public class StoryDialogueConfig : IInitializable
	{
		// Token: 0x1700085B RID: 2139
		// (get) Token: 0x060036A9 RID: 13993 RVA: 0x0010A865 File Offset: 0x00108C65
		public IEnumerable<DialogueSetupData> AllDialogues
		{
			get
			{
				return this.dialogue_setup;
			}
		}

		// Token: 0x060036AA RID: 13994 RVA: 0x0010A870 File Offset: 0x00108C70
		public DialogueDetailsData[] GetDialogue(string id)
		{
			Debug.Log("Get dialogue");
			return Array.FindAll<DialogueDetailsData>(this.dialogue_details, (DialogueDetailsData dd) => dd.dialogue_id == id);
		}

		// Token: 0x060036AB RID: 13995 RVA: 0x0010A8A4 File Offset: 0x00108CA4
		public DialogueSetupData FindDialogue(string id)
		{
			Debug.Log("Find dialogue, id:" + id);
			return Array.Find<DialogueSetupData>(this.dialogue_setup, (DialogueSetupData ds) => ds.dialogue_id == id);
		}

		// Token: 0x060036AC RID: 13996 RVA: 0x0010A8D8 File Offset: 0x00108CD8
		public DialogueSetupData FindDialogue(DialogueTrigger trigger, string value)
		{
			Debug.Log("Find dialogue by tigger, value:" + value);
			return Array.Find<DialogueSetupData>(this.dialogue_setup, (DialogueSetupData ds) => ds.Trigger == trigger && ds.trigger_value == value);
		}

		// Token: 0x060036AD RID: 13997 RVA: 0x0010A910 File Offset: 0x00108D10
		public void Init()
		{
			foreach (DialogueSetupData dialogueSetupData in this.dialogue_setup)
			{
				dialogueSetupData.Init();
			}
		}

		// Token: 0x04005ECA RID: 24266
		[SerializeField]
		private DialogueSetupData[] dialogue_setup;

		// Token: 0x04005ECB RID: 24267
		[SerializeField]
		private DialogueDetailsData[] dialogue_details;
	}
}
