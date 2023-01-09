using System;
using System.Collections.Generic;
using System.Linq;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Services.ConfigServiceV2;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Town
{
	// Token: 0x020009A7 RID: 2471
	public class AbTestChooser : MonoBehaviour
	{
		// Token: 0x06003BEF RID: 15343 RVA: 0x00129DA4 File Offset: 0x001281A4
		public IEnumerator<AbTestChooser.ChosenAbTests> ShowAndGetForcedAbTestStringRoutine(ISbsConfigService configService, CheatProgressionDisplay statusView)
		{
			return configService.GetAbTestConfig().ContinueWith(delegate(AbTestConfig config)
			{
				this.abTests = configService.AbTests;
				if (config.ab_tests != null)
				{
					foreach (AbTestConfig.AbTest abTest in config.ab_tests)
					{
						AbTestConfig.AbTestGroup abTestGroup = new AbTestConfig.AbTestGroup();
						abTestGroup.key = "0";
						abTestGroup.name = "control";
						abTest.groups.Add(abTestGroup);
					}
				}
				this.config = config;
				this.SetupViews();
				return this.Await();
			}).ContinueWith((string abTestString) => new AbTestChooser.ChosenAbTests(abTestString, this.shouldChosenAbTestsBeApplied)).Catch(delegate(Exception exception)
			{
				WoogaDebug.LogWarning(new object[]
				{
					"AbTestChooser: ",
					exception.Message
				});
				statusView.OverrideText(exception.Message);
				return new AbTestChooser.ChosenAbTests(null, false);
			});
		}

		// Token: 0x06003BF0 RID: 15344 RVA: 0x00129E0C File Offset: 0x0012820C
		public IEnumerator<string> Await()
		{
			this.isRunning = true;
			while (this.isRunning)
			{
				yield return null;
			}
			this.abTestViewContainer.Cleanup();
			yield return this.abTests;
			yield break;
		}

		// Token: 0x06003BF1 RID: 15345 RVA: 0x00129E28 File Offset: 0x00128228
		private void SetupViews()
		{
			if (this.config != null && this.config.ab_tests != null && !this.abTests.IsNullOrEmpty())
			{
				this.chosenGroups = new int[this.config.ab_tests.Count];
				for (int i = 0; i < this.config.ab_tests.Count; i++)
				{
					AbTestConfig.AbTest abTest = this.config.ab_tests[i];
					int num = abTest.groups.FindIndex((AbTestConfig.AbTestGroup g) => g.key == this.abTests[abTest.position].ToString());
					this.chosenGroups[i] = num;
				}
				this.UpdateAbTests();
				this.abTestViewContainer.Setup(this.config.ab_tests, this.chosenGroups);
			}
			else
			{
				WoogaDebug.LogWarning(new object[]
				{
					"abTests string is null or empty. Are you using bundled only configs?"
				});
			}
		}

		// Token: 0x06003BF2 RID: 15346 RVA: 0x00129F20 File Offset: 0x00128320
		public void OnOkTapped()
		{
			if (!this.abTests.IsNullOrEmpty())
			{
				this.UpdateAbTests();
				this.shouldChosenAbTestsBeApplied = true;
				this.isRunning = false;
			}
			else
			{
				this.UseDefaultTapped();
			}
		}

		// Token: 0x06003BF3 RID: 15347 RVA: 0x00129F51 File Offset: 0x00128351
		public void UseDefaultTapped()
		{
			this.abTests = null;
			this.shouldChosenAbTestsBeApplied = true;
			this.isRunning = false;
		}

		// Token: 0x06003BF4 RID: 15348 RVA: 0x00129F68 File Offset: 0x00128368
		public void OnCancelTapped()
		{
			if (!this.abTests.IsNullOrEmpty())
			{
				this.UpdateAbTests();
			}
			this.shouldChosenAbTestsBeApplied = false;
			this.isRunning = false;
		}

		// Token: 0x06003BF5 RID: 15349 RVA: 0x00129F90 File Offset: 0x00128390
		private void UpdateAbTests()
		{
			int num;
			if (this.config.ab_tests == null)
			{
				num = 0;
			}
			else
			{
				num = this.config.ab_tests.Max((AbTestConfig.AbTest abtest) => abtest.position);
			}
			int num2 = num;
			string[] array = new string[num2 + 1];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = "-";
			}
			int num3 = (this.config.ab_tests != null) ? this.config.ab_tests.Count : 0;
			for (int j = 0; j < num3; j++)
			{
				AbTestConfig.AbTest abTest = this.config.ab_tests[j];
				array[abTest.position] = abTest.groups[this.chosenGroups[j]].key;
			}
			this.abTests = string.Concat(array);
		}

		// Token: 0x04006401 RID: 25601
		public AbTestConfig config;

		// Token: 0x04006402 RID: 25602
		[HideInInspector]
		public string abTests;

		// Token: 0x04006403 RID: 25603
		private const string REFERENCE_GROUP = "reference";

		// Token: 0x04006404 RID: 25604
		private int[] chosenGroups;

		// Token: 0x04006405 RID: 25605
		private bool isRunning;

		// Token: 0x04006406 RID: 25606
		private bool shouldChosenAbTestsBeApplied;

		// Token: 0x04006407 RID: 25607
		private Rect area;

		// Token: 0x04006408 RID: 25608
		public AbTestViewContainer abTestViewContainer;

		// Token: 0x020009A8 RID: 2472
		public class ChosenAbTests
		{
			// Token: 0x06003BF7 RID: 15351 RVA: 0x0012A08E File Offset: 0x0012848E
			public ChosenAbTests(string tests, bool shouldBeUsed)
			{
				this.abTests = tests;
				this.shouldBeApplied = shouldBeUsed;
			}

			// Token: 0x0400640A RID: 25610
			public readonly string abTests;

			// Token: 0x0400640B RID: 25611
			public readonly bool shouldBeApplied;
		}
	}
}
