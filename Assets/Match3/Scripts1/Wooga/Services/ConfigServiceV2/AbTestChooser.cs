using System;
using System.Collections.Generic;
using System.Linq;
using Wooga.Coroutines;
using UnityEngine;

namespace Match3.Scripts1.Wooga.Services.ConfigServiceV2
{
	// Token: 0x0200032D RID: 813
	public class AbTestChooser : MonoBehaviour
	{
		// Token: 0x0600193B RID: 6459 RVA: 0x000724CC File Offset: 0x000708CC
		public static IEnumerator<string> Show(ISbsConfigService configService)
		{
			GameObject go = new GameObject("AbTestChooser", new Type[]
			{
				typeof(AbTestChooser)
			});
			AbTestChooser chooser = go.GetComponent<AbTestChooser>();
			go.SetActive(false);
			return configService.GetAbTestConfig().ContinueWith(delegate(AbTestConfig config)
			{
				foreach (AbTestConfig.AbTest abTest in config.ab_tests)
				{
					AbTestConfig.AbTestGroup abTestGroup = new AbTestConfig.AbTestGroup();
					abTestGroup.key = "-";
					abTestGroup.name = "reference";
					abTest.groups.Add(abTestGroup);
				}
				chooser.config = config;
				go.SetActive(true);
				return chooser.Await();
			}).ContinueWith(delegate(string abTestString)
			{
				global::UnityEngine.Object.Destroy(go);
				return abTestString;
			});
		}

		// Token: 0x0600193C RID: 6460 RVA: 0x00072548 File Offset: 0x00070948
		public IEnumerator<string> Await()
		{
			this.isRunning = true;
			while (this.isRunning)
			{
				yield return null;
			}
			yield return this.abTests;
			yield break;
		}

		// Token: 0x0600193D RID: 6461 RVA: 0x00072564 File Offset: 0x00070964
		private void Start()
		{
			if (this.config != null)
			{
				this.chosenGroups = new int[this.config.ab_tests.Count];
				int num = 30;
				Vector2 size = new Vector2(400f, (float)((this.config.ab_tests.Count + 1) * num));
				Vector2 position = new Vector2((float)(Screen.width / 2) - size.x / 2f, (float)(Screen.height / 2) - size.y / 2f);
				this.area = new Rect(position, size);
			}
		}

		// Token: 0x0600193E RID: 6462 RVA: 0x000725FC File Offset: 0x000709FC
		private void OnGUI()
		{
			GUILayout.BeginArea(this.area, GUI.skin.box);
			for (int i = 0; i < this.config.ab_tests.Count; i++)
			{
				AbTestConfig.AbTest abTest = this.config.ab_tests[i];
				this.DisplayTest(abTest, i);
			}
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			if (GUILayout.Button("Ok", new GUILayoutOption[0]))
			{
				this.UpdateAbTests();
				this.isRunning = false;
			}
			if (GUILayout.Button("Use Default", new GUILayoutOption[0]))
			{
				this.abTests = null;
				this.isRunning = false;
			}
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
		}

		// Token: 0x0600193F RID: 6463 RVA: 0x000726B4 File Offset: 0x00070AB4
		private void UpdateAbTests()
		{
			int num = (from abtest in this.config.ab_tests
			select abtest.position).Max();
			string[] array = new string[num + 1];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = "-";
			}
			for (int j = 0; j < this.config.ab_tests.Count; j++)
			{
				AbTestConfig.AbTest abTest = this.config.ab_tests[j];
				array[j] = abTest.groups[this.chosenGroups[j]].key;
			}
			this.abTests = string.Concat(array);
		}

		// Token: 0x06001940 RID: 6464 RVA: 0x00072778 File Offset: 0x00070B78
		private void DisplayTest(AbTestConfig.AbTest abTest, int index)
		{
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Label(abTest.name, new GUILayoutOption[0]);
			if (GUILayout.Button(abTest.groups[this.chosenGroups[index]].name, new GUILayoutOption[0]))
			{
				this.chosenGroups[index] = (this.chosenGroups[index] + 1) % abTest.groups.Count;
			}
			GUILayout.EndHorizontal();
		}

		// Token: 0x040047FC RID: 18428
		public AbTestConfig config;

		// Token: 0x040047FD RID: 18429
		public string abTests;

		// Token: 0x040047FE RID: 18430
		private const string REFERENCE_GROUP = "reference";

		// Token: 0x040047FF RID: 18431
		private int[] chosenGroups;

		// Token: 0x04004800 RID: 18432
		private bool isRunning;

		// Token: 0x04004801 RID: 18433
		private Rect area;
	}
}
