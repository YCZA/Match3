using System.Collections;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Tutorials
{
	// Token: 0x02000A91 RID: 2705
	public class TutorialSpeechBubble : MonoBehaviour
	{
		// Token: 0x06004078 RID: 16504 RVA: 0x0014DEAB File Offset: 0x0014C2AB
		private void Awake()
		{
			// if (this.animators != null)
			// {
			// 	this.animatorStateInfos = new AnimatorStateInfo[this.animators.Length];
			// }
		}

		// Token: 0x17000943 RID: 2371
		// (get) Token: 0x06004079 RID: 16505 RVA: 0x0014DECC File Offset: 0x0014C2CC
		// (set) Token: 0x0600407A RID: 16506 RVA: 0x0014DF14 File Offset: 0x0014C314
		public TutorialStepLayout Info
		{
			get
			{
				return new TutorialStepLayout
				{
					box = new SimpleTransform(this.box),
					bubble = new SimpleTransform(this.bubble),
					character = new SimpleTransform(this.character)
				};
			}
			set
			{
				if (value == null || value.isDefault)
				{
					return;
				}
				value.bubble.ApplyTo(this.bubble);
				value.box.ApplyTo(this.box);
				value.character.ApplyTo(this.character);
			}
		}

		// Token: 0x0600407B RID: 16507 RVA: 0x0014DF68 File Offset: 0x0014C368
		public void SetCharacter(TutorialSpeechBubble.TutorialCharacter character)
		{
			for (int i = 0; i < this.characters.Length; i++)
			{
				if (i == (int)character)
				{
					this.characters[i].SetActive(true);
					this.character = this.characters[i].GetComponent<RectTransform>();
				}
				else
				{
					this.PreserveAnimations();
					this.characters[i].SetActive(false);
				}
			}
		}

		// Token: 0x0600407C RID: 16508 RVA: 0x0014DFD0 File Offset: 0x0014C3D0
		public IEnumerator Setup3DCharacterAnimator(TutorialSpeechBubble.TutorialCharacter character)
		{
			// if (this.animators != null && (TutorialSpeechBubble.TutorialCharacter)this.animators.Length > character)
			// {
			// 	while (!this.animators[(int)character].isInitialized)
			// 	{
			// 		yield return null;
			// 	}
			// 	this.animators[(int)character].Play(this.animatorStateInfos[(int)character].shortNameHash, 0, this.animatorStateInfos[(int)character].normalizedTime);
			// }
			yield break;
		}

		// Token: 0x0600407D RID: 16509 RVA: 0x0014DFF4 File Offset: 0x0014C3F4
		public void PreserveAnimations()
		{
			// if (this.animators == null)
			// {
			// 	return;
			// }
			// for (int i = 0; i < this.animators.Length; i++)
			// {
			// 	if (this.animators[i].gameObject.activeInHierarchy)
			// 	{
			// 		this.animatorStateInfos[i] = this.animators[i].GetCurrentAnimatorStateInfo(0);
			// 	}
			// }
		}

		// Token: 0x04006A17 RID: 27159
		public RectTransform bubble;

		// Token: 0x04006A18 RID: 27160
		public RectTransform box;

		// Token: 0x04006A19 RID: 27161
		public RectTransform character;

		// Token: 0x04006A1A RID: 27162
		public RectTransform icon;

		// Token: 0x04006A1B RID: 27163
		public GameObject[] characters;

		// Token: 0x04006A1C RID: 27164
		// public Animator[] animators;

		// Token: 0x04006A1D RID: 27165
		private AnimatorStateInfo[] animatorStateInfos;

		// Token: 0x02000A92 RID: 2706
		public enum TutorialCharacter
		{
			// Token: 0x04006A1F RID: 27167
			Elsie,
			// Token: 0x04006A20 RID: 27168
			Ava
		}

		// Token: 0x02000A93 RID: 2707
		public enum SpeechBubleAnimation
		{
			// Token: 0x04006A22 RID: 27170
			Show,
			// Token: 0x04006A23 RID: 27171
			Change,
			// Token: 0x04006A24 RID: 27172
			Hide
		}
	}
}
