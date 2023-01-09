using System;
using System.Collections.Generic;

// Token: 0x02000934 RID: 2356
namespace Match3.Scripts1
{
	[Serializable]
	public class QuestProgressCollection
	{
		// Token: 0x06003942 RID: 14658 RVA: 0x0011A07D File Offset: 0x0011847D
		public QuestProgressCollection()
		{
		}

		// Token: 0x06003943 RID: 14659 RVA: 0x0011A0B4 File Offset: 0x001184B4
		public QuestProgressCollection(Dictionary<string, QuestProgress> quests)
		{
			foreach (string text in quests.Keys)
			{
				this.Add(text, quests[text]);
			}
		}

		// Token: 0x06003944 RID: 14660 RVA: 0x0011A14C File Offset: 0x0011854C
		public void Add(string name, QuestProgress progress)
		{
			progress.questID = name;
			this.questProgressions.Add(progress);
		}

		// Token: 0x06003945 RID: 14661 RVA: 0x0011A161 File Offset: 0x00118561
		public void SetCompleted(string questID)
		{
			if (this.IsComplete(questID) || this.IsCollected(questID))
			{
				return;
			}
			this.completedQuests.Add(questID);
		}

		// Token: 0x06003946 RID: 14662 RVA: 0x0011A188 File Offset: 0x00118588
		public void SetCollected(string questID)
		{
			int i = 0;
			int count = this.questProgressions.Count;
			while (i < count)
			{
				if (questID == this.questProgressions[i].questID)
				{
					this.questProgressions.RemoveAt(i);
					break;
				}
				i++;
			}
			int j = 0;
			int count2 = this.completedQuests.Count;
			while (j < count2)
			{
				if (questID == this.completedQuests[j])
				{
					this.completedQuests.RemoveAt(j);
					break;
				}
				j++;
			}
			this.collectedQuests.AddIfNotAlreadyPresent(questID, false);
		}

		// Token: 0x06003947 RID: 14663 RVA: 0x0011A234 File Offset: 0x00118634
		public bool IsComplete(string questID)
		{
			int i = 0;
			int count = this.completedQuests.Count;
			while (i < count)
			{
				if (questID == this.completedQuests[i])
				{
					return true;
				}
				i++;
			}
			return false;
		}

		// Token: 0x06003948 RID: 14664 RVA: 0x0011A27C File Offset: 0x0011867C
		public bool IsCollected(string questID)
		{
			int i = 0;
			int count = this.collectedQuests.Count;
			while (i < count)
			{
				if (questID == this.collectedQuests[i])
				{
					return true;
				}
				i++;
			}
			return false;
		}

		// Token: 0x06003949 RID: 14665 RVA: 0x0011A2C4 File Offset: 0x001186C4
		public QuestProgress Get(string name)
		{
			for (int i = 0; i < this.questProgressions.Count; i++)
			{
				if (this.questProgressions[i].questID == name)
				{
					return this.questProgressions[i];
				}
			}
			return null;
		}

		// Token: 0x040061A0 RID: 24992
		public List<QuestProgress> questProgressions = new List<QuestProgress>();

		// Token: 0x040061A1 RID: 24993
		public List<string> completedQuests = new List<string>();

		// Token: 0x040061A2 RID: 24994
		public List<string> collectedQuests = new List<string>();

		// Token: 0x040061A3 RID: 24995
		public List<string> completedQuestDialogs = new List<string>();
	}
}
