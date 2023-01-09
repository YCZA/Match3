using Match3.Scripts1.Puzzletown.Build;
using Match3.Scripts1.Shared.Build;
using Match3.Scripts1.Wooga.Services;
using Match3.Scripts2.Env;
using TMPro;
using UnityEngine;

// Token: 0x02000855 RID: 2133
namespace Match3.Scripts1
{
	public class BuildDisplay : MonoBehaviour
	{
		// Token: 0x060034C2 RID: 13506 RVA: 0x000FC871 File Offset: 0x000FAC71
		private void Awake()
		{
			if (this.fps)
			{
				this.fps.enabled = (this.showEnvironment || global::UnityEngine.Debug.isDebugBuild);
			}
			this.RefreshStatic();
		}

		// Token: 0x060034C3 RID: 13507 RVA: 0x000FC8A8 File Offset: 0x000FACA8
		public void RefreshStatic()
		{
			DeviceInfo.Resolution resolution = DeviceInfo.Load().resolution;
			if (SBS.Authentication == null || SBS.Authentication.GetUserContext() == null)
			{
				return;
			}
			string user_id = SBS.Authentication.GetUserContext().user_id;
			this.authentication.text = user_id;
			if (this.showEnvironment)
			{
				this.build.text = string.Concat(new object[]
				{
					BuildVersion.ShortVersion,
					"/",
					GameEnvironment.CurrentEnvironment,
					"/",
					resolution
				});
			}
			else
			{
				this.build.text = string.Concat(new object[]
				{
					"v",
					BuildVersion.ShortVersion,
					"/",
					resolution
				});
			}
		}

		// Token: 0x060034C4 RID: 13508 RVA: 0x000FC984 File Offset: 0x000FAD84
		private void Update()
		{
			if (!this.fps)
			{
				return;
			}
			this.deltaTime += (Time.unscaledDeltaTime - this.deltaTime) * 0.1f;
			int num = Mathf.Clamp(Mathf.FloorToInt(1f / this.deltaTime), 0, 99);
			int num2 = Mathf.Clamp(Mathf.CeilToInt(this.deltaTime * 1000f), 0, 99);
			this.fps.text = this.FPS_STRING_VALUES[num];
			this.frametime.text = this.MS_STRING_VALUES[num2];
			if (!this.applyFPSColoring)
			{
				return;
			}
			if ((float)num < (float)Application.targetFrameRate * 0.25f)
			{
				this.fps.color = Color.red;
				this.frametime.color = Color.red;
			}
			else if ((float)num < (float)Application.targetFrameRate * 0.5f)
			{
				this.fps.color = Color.yellow;
				this.frametime.color = Color.yellow;
			}
			else
			{
				this.fps.color = Color.white;
				this.frametime.color = Color.white;
			}
		}

		// Token: 0x04005CA6 RID: 23718
		public TMP_Text authentication;

		// Token: 0x04005CA7 RID: 23719
		public TMP_Text fps;

		// Token: 0x04005CA8 RID: 23720
		public TMP_Text frametime;

		// Token: 0x04005CA9 RID: 23721
		public TMP_Text build;

		// Token: 0x04005CAA RID: 23722
		public bool showEnvironment = true;

		// Token: 0x04005CAB RID: 23723
		public bool applyFPSColoring = true;

		// Token: 0x04005CAC RID: 23724
		private float deltaTime;

		// Token: 0x04005CAD RID: 23725
		private const int MAX_FPS_DISPLAY = 99;

		// Token: 0x04005CAE RID: 23726
		private const float CRITICAL_FPS = 0.25f;

		// Token: 0x04005CAF RID: 23727
		private const float NORMAL_FPS = 0.5f;

		// Token: 0x04005CB0 RID: 23728
		private readonly string[] FPS_STRING_VALUES = new string[]
		{
			" 0",
			" 1",
			" 2",
			" 3",
			" 4",
			" 5",
			" 6",
			" 7",
			" 8",
			" 9",
			"10",
			"11",
			"12",
			"13",
			"14",
			"15",
			"16",
			"17",
			"18",
			"19",
			"20",
			"21",
			"22",
			"23",
			"24",
			"25",
			"26",
			"27",
			"28",
			"29",
			"30",
			"31",
			"32",
			"33",
			"34",
			"35",
			"36",
			"37",
			"38",
			"39",
			"40",
			"41",
			"42",
			"43",
			"44",
			"45",
			"46",
			"47",
			"48",
			"49",
			"50",
			"51",
			"52",
			"53",
			"54",
			"55",
			"56",
			"57",
			"58",
			"59",
			"60",
			"61",
			"62",
			"63",
			"64",
			"65",
			"66",
			"67",
			"68",
			"69",
			"70",
			"71",
			"72",
			"73",
			"74",
			"75",
			"76",
			"77",
			"78",
			"79",
			"80",
			"81",
			"82",
			"83",
			"84",
			"85",
			"86",
			"87",
			"88",
			"89",
			"90",
			"91",
			"92",
			"93",
			"94",
			"95",
			"96",
			"97",
			"98",
			"99"
		};

		// Token: 0x04005CB1 RID: 23729
		private readonly string[] MS_STRING_VALUES = new string[]
		{
			"0ms",
			"1ms",
			"2ms",
			"3ms",
			"4ms",
			"5ms",
			"6ms",
			"7ms",
			"8ms",
			"9ms",
			"10ms",
			"11ms",
			"12ms",
			"13ms",
			"14ms",
			"15ms",
			"16ms",
			"17ms",
			"18ms",
			"19ms",
			"20ms",
			"21ms",
			"22ms",
			"23ms",
			"24ms",
			"25ms",
			"26ms",
			"27ms",
			"28ms",
			"29ms",
			"30ms",
			"31ms",
			"32ms",
			"33ms",
			"34ms",
			"35ms",
			"36ms",
			"37ms",
			"38ms",
			"39ms",
			"40ms",
			"41ms",
			"42ms",
			"43ms",
			"44ms",
			"45ms",
			"46ms",
			"47ms",
			"48ms",
			"49ms",
			"50ms",
			"51ms",
			"52ms",
			"53ms",
			"54ms",
			"55ms",
			"56ms",
			"57ms",
			"58ms",
			"59ms",
			"60ms",
			"61ms",
			"62ms",
			"63ms",
			"64ms",
			"65ms",
			"66ms",
			"67ms",
			"68ms",
			"69ms",
			"70ms",
			"71ms",
			"72ms",
			"73ms",
			"74ms",
			"75ms",
			"76ms",
			"77ms",
			"78ms",
			"79ms",
			"80ms",
			"81ms",
			"82ms",
			"83ms",
			"84ms",
			"85ms",
			"86ms",
			"87ms",
			"88ms",
			"89ms",
			"90ms",
			"91ms",
			"92ms",
			"93ms",
			"94ms",
			"95ms",
			"96ms",
			"97ms",
			"98ms",
			" "
		};
	}
}
