using System;
using System.Collections.Generic;

namespace Match3.Scripts1.Spine
{
	// Token: 0x02000220 RID: 544
	public class Skin
	{
		// Token: 0x060010FE RID: 4350 RVA: 0x0002DE0C File Offset: 0x0002C20C
		public Skin(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name cannot be null.");
			}
			this.name = name;
		}

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x060010FF RID: 4351 RVA: 0x0002DE3C File Offset: 0x0002C23C
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x06001100 RID: 4352 RVA: 0x0002DE44 File Offset: 0x0002C244
		public void AddAttachment(int slotIndex, string name, Attachment attachment)
		{
			if (attachment == null)
			{
				throw new ArgumentNullException("attachment cannot be null.");
			}
			this.attachments[new Skin.AttachmentKeyTuple(slotIndex, name)] = attachment;
		}

		// Token: 0x06001101 RID: 4353 RVA: 0x0002DE6C File Offset: 0x0002C26C
		public Attachment GetAttachment(int slotIndex, string name)
		{
			Attachment result;
			this.attachments.TryGetValue(new Skin.AttachmentKeyTuple(slotIndex, name), out result);
			return result;
		}

		// Token: 0x06001102 RID: 4354 RVA: 0x0002DE90 File Offset: 0x0002C290
		public void FindNamesForSlot(int slotIndex, List<string> names)
		{
			if (names == null)
			{
				throw new ArgumentNullException("names cannot be null.");
			}
			foreach (Skin.AttachmentKeyTuple attachmentKeyTuple in this.attachments.Keys)
			{
				if (attachmentKeyTuple.slotIndex == slotIndex)
				{
					names.Add(attachmentKeyTuple.name);
				}
			}
		}

		// Token: 0x06001103 RID: 4355 RVA: 0x0002DF18 File Offset: 0x0002C318
		public void FindAttachmentsForSlot(int slotIndex, List<Attachment> attachments)
		{
			if (attachments == null)
			{
				throw new ArgumentNullException("attachments cannot be null.");
			}
			foreach (KeyValuePair<Skin.AttachmentKeyTuple, Attachment> keyValuePair in this.attachments)
			{
				if (keyValuePair.Key.slotIndex == slotIndex)
				{
					attachments.Add(keyValuePair.Value);
				}
			}
		}

		// Token: 0x06001104 RID: 4356 RVA: 0x0002DFA0 File Offset: 0x0002C3A0
		public override string ToString()
		{
			return this.name;
		}

		// Token: 0x06001105 RID: 4357 RVA: 0x0002DFA8 File Offset: 0x0002C3A8
		internal void AttachAll(Skeleton skeleton, Skin oldSkin)
		{
			foreach (KeyValuePair<Skin.AttachmentKeyTuple, Attachment> keyValuePair in oldSkin.attachments)
			{
				int slotIndex = keyValuePair.Key.slotIndex;
				Slot slot = skeleton.slots.Items[slotIndex];
				if (slot.attachment == keyValuePair.Value)
				{
					Attachment attachment = this.GetAttachment(slotIndex, keyValuePair.Key.name);
					if (attachment != null)
					{
						slot.Attachment = attachment;
					}
				}
			}
		}

		// Token: 0x0400417A RID: 16762
		internal string name;

		// Token: 0x0400417B RID: 16763
		private Dictionary<Skin.AttachmentKeyTuple, Attachment> attachments = new Dictionary<Skin.AttachmentKeyTuple, Attachment>(Skin.AttachmentKeyTupleComparer.Instance);

		// Token: 0x02000221 RID: 545
		private struct AttachmentKeyTuple
		{
			// Token: 0x06001106 RID: 4358 RVA: 0x0002E058 File Offset: 0x0002C458
			public AttachmentKeyTuple(int slotIndex, string name)
			{
				this.slotIndex = slotIndex;
				this.name = name;
				this.nameHashCode = this.name.GetHashCode();
			}

			// Token: 0x0400417C RID: 16764
			public readonly int slotIndex;

			// Token: 0x0400417D RID: 16765
			public readonly string name;

			// Token: 0x0400417E RID: 16766
			internal readonly int nameHashCode;
		}

		// Token: 0x02000222 RID: 546
		private class AttachmentKeyTupleComparer : IEqualityComparer<Skin.AttachmentKeyTuple>
		{
			// Token: 0x06001108 RID: 4360 RVA: 0x0002E081 File Offset: 0x0002C481
			bool IEqualityComparer<Skin.AttachmentKeyTuple>.Equals(Skin.AttachmentKeyTuple o1, Skin.AttachmentKeyTuple o2)
			{
				return o1.slotIndex == o2.slotIndex && o1.nameHashCode == o2.nameHashCode && o1.name == o2.name;
			}

			// Token: 0x06001109 RID: 4361 RVA: 0x0002E0BF File Offset: 0x0002C4BF
			int IEqualityComparer<Skin.AttachmentKeyTuple>.GetHashCode(Skin.AttachmentKeyTuple o)
			{
				return o.slotIndex;
			}

			// Token: 0x0400417F RID: 16767
			internal static readonly Skin.AttachmentKeyTupleComparer Instance = new Skin.AttachmentKeyTupleComparer();
		}
	}
}
