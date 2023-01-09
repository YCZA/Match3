using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Wooga.Newtonsoft.Json;
using UnityEngine;

namespace Match3.Scripts1.Shared.Util
{
	// Token: 0x02000B4E RID: 2894
	public static class Profiler
	{
		// Token: 0x170009BB RID: 2491
		// (get) Token: 0x060043B9 RID: 17337 RVA: 0x00159F30 File Offset: 0x00158330
		public static string ProfilerFile
		{
			get
			{
				return Application.temporaryCachePath + "/start_profile.txt";
			}
		}

		// Token: 0x060043BA RID: 17338 RVA: 0x00159F44 File Offset: 0x00158344
		public static bool TryGetDuration(string id, out float duration)
		{
			if (Profiler.entries != null && Profiler.entries.ContainsKey(id))
			{
				Profiler.ProfilerEntry profilerEntry = Profiler.entries[id];
				if (profilerEntry.startTime != default(DateTime) && profilerEntry.endTime != default(DateTime))
				{
					duration = profilerEntry.Duration;
					return true;
				}
			}
			duration = -1f;
			return false;
		}

		// Token: 0x060043BB RID: 17339 RVA: 0x00159FBC File Offset: 0x001583BC
		[Conditional("DEBUG")]
		public static void Start(string id)
		{
			Profiler.entries[id] = new Profiler.ProfilerEntry(id)
			{
				actualStart = Time.realtimeSinceStartup
			};
		}

		// Token: 0x060043BC RID: 17340 RVA: 0x00159FE7 File Offset: 0x001583E7
		[Conditional("DEBUG")]
		public static void End(string id)
		{
			Profiler.entries[id].endTime = DateTime.UtcNow;
			Profiler.entries[id].actualEnd = Time.realtimeSinceStartup;
		}

		// Token: 0x060043BD RID: 17341 RVA: 0x0015A014 File Offset: 0x00158414
		[Conditional("DEBUG")]
		public static void End(string id, string startId)
		{
			Profiler.ProfilerEntry value = new Profiler.ProfilerEntry(id)
			{
				startTime = Profiler.entries[startId].startTime,
				endTime = DateTime.UtcNow,
				actualStart = Profiler.entries[startId].actualStart,
				actualEnd = Time.realtimeSinceStartup
			};
			Profiler.entries[id] = value;
		}

		// Token: 0x060043BE RID: 17342 RVA: 0x0015A078 File Offset: 0x00158478
		[Conditional("DEBUG")]
		public static void Print()
		{
			StringBuilder sb = new StringBuilder();
			(from kvp in Profiler.entries
			orderby kvp.Value.Duration descending
			select kvp.Value).ForEach(delegate(Profiler.ProfilerEntry entry)
			{
				sb.AppendLine(entry.ToString());
			});
			global::UnityEngine.Debug.Log(sb);
		}

		// Token: 0x060043BF RID: 17343 RVA: 0x0015A0FC File Offset: 0x001584FC
		[Conditional("DEBUG")]
		public static void DumpToFile()
		{
			Profiler.ChartEntry[] array = (from kvp in Profiler.entries
			orderby kvp.Value.Duration descending
			select Profiler.ChartEntry.CreateFromEntry(kvp.Value)).ToArray<Profiler.ChartEntry>();
			float actualStart = (from kvp in Profiler.entries
			orderby kvp.Value.actualStart
			select kvp).FirstOrDefault<KeyValuePair<string, Profiler.ProfilerEntry>>().Value.actualStart;
			float actualEnd = (from kvp in Profiler.entries
			orderby kvp.Value.actualEnd
			select kvp).LastOrDefault<KeyValuePair<string, Profiler.ProfilerEntry>>().Value.actualEnd;
			Profiler.ChartData value = new Profiler.ChartData
			{
				entries = array,
				startTime = actualStart,
				endTime = actualEnd
			};
			try
			{
				string contents = JsonConvert.SerializeObject(value);
				File.WriteAllText(Profiler.ProfilerFile, contents);
			}
			catch (Exception)
			{
				global::UnityEngine.Debug.LogWarning("Failed to write profiler file to: " + Profiler.ProfilerFile);
			}
		}

		// Token: 0x060043C0 RID: 17344 RVA: 0x0015A234 File Offset: 0x00158634
		public static Profiler.ChartData LoadChartFromFile()
		{
			string value = File.ReadAllText(Profiler.ProfilerFile);
			return JsonConvert.DeserializeObject<Profiler.ChartData>(value);
		}

		// Token: 0x04006C22 RID: 27682
		private static readonly Dictionary<string, Profiler.ProfilerEntry> entries = new Dictionary<string, Profiler.ProfilerEntry>();

		// Token: 0x02000B4F RID: 2895
		public class ProfilerEntry
		{
			// Token: 0x060043C8 RID: 17352 RVA: 0x0015A2AF File Offset: 0x001586AF
			public ProfilerEntry(string id)
			{
				this.id = id;
				this.startTime = DateTime.UtcNow;
			}

			// Token: 0x170009BC RID: 2492
			// (get) Token: 0x060043C9 RID: 17353 RVA: 0x0015A2CC File Offset: 0x001586CC
			public float Duration
			{
				get
				{
					return (float)(this.endTime - this.startTime).TotalSeconds;
				}
			}

			// Token: 0x060043CA RID: 17354 RVA: 0x0015A2F3 File Offset: 0x001586F3
			public override string ToString()
			{
				return string.Format("{0} took {1}s.", this.id, this.Duration);
			}

			// Token: 0x04006C29 RID: 27689
			public string id;

			// Token: 0x04006C2A RID: 27690
			public DateTime startTime;

			// Token: 0x04006C2B RID: 27691
			public DateTime endTime;

			// Token: 0x04006C2C RID: 27692
			public float actualStart;

			// Token: 0x04006C2D RID: 27693
			public float actualEnd;

			// Token: 0x04006C2E RID: 27694
			public string fileName;

			// Token: 0x04006C2F RID: 27695
			public int fileNumber;
		}

		// Token: 0x02000B50 RID: 2896
		public class ChartEntry
		{
			// Token: 0x060043CC RID: 17356 RVA: 0x0015A318 File Offset: 0x00158718
			public static Profiler.ChartEntry CreateFromEntry(Profiler.ProfilerEntry entry)
			{
				return new Profiler.ChartEntry
				{
					actualStart = entry.actualStart,
					actualEnd = entry.actualEnd,
					fileName = string.Empty,
					fileNumber = 0,
					name = entry.id
				};
			}

			// Token: 0x04006C30 RID: 27696
			public float actualStart;

			// Token: 0x04006C31 RID: 27697
			public float actualEnd;

			// Token: 0x04006C32 RID: 27698
			public string fileName;

			// Token: 0x04006C33 RID: 27699
			public int fileNumber;

			// Token: 0x04006C34 RID: 27700
			public string name;
		}

		// Token: 0x02000B51 RID: 2897
		public class ChartData
		{
			// Token: 0x170009BD RID: 2493
			// (get) Token: 0x060043CE RID: 17358 RVA: 0x0015A36A File Offset: 0x0015876A
			public float TotalTime
			{
				get
				{
					return this.endTime - this.startTime;
				}
			}

			// Token: 0x04006C35 RID: 27701
			public float startTime;

			// Token: 0x04006C36 RID: 27702
			public float endTime;

			// Token: 0x04006C37 RID: 27703
			public Profiler.ChartEntry[] entries;
		}
	}
}
