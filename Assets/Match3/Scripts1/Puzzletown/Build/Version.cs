using System;
using System.Text.RegularExpressions;

namespace Match3.Scripts1.Puzzletown.Build
{
	// Token: 0x02000AAB RID: 2731
	[Serializable]
	public class Version : IComparable<Version>
	{
		// Token: 0x060040D1 RID: 16593 RVA: 0x00151397 File Offset: 0x0014F797
		public Version()
		{
		}

		// Token: 0x060040D2 RID: 16594 RVA: 0x0015139F File Offset: 0x0014F79F
		public Version(int major, int minor)
		{
			this.major = major;
			this.minor = minor;
		}

		// Token: 0x060040D3 RID: 16595 RVA: 0x001513B8 File Offset: 0x0014F7B8
		public Version(string version)
		{
			string[] array = version.Split(".".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
			this.major = int.Parse(array[0]);
			this.minor = int.Parse(array[1]);
		}

		// Token: 0x060040D4 RID: 16596 RVA: 0x001513F9 File Offset: 0x0014F7F9
		public int AsInt()
		{
			return this.major * 1000 + this.minor;
		}

		// Token: 0x060040D5 RID: 16597 RVA: 0x00151410 File Offset: 0x0014F810
		public int CompareTo(Version other)
		{
			int num = this.major.CompareTo(other.major);
			if (num == 0)
			{
				num = this.minor.CompareTo(other.minor);
			}
			return num;
		}

		// Token: 0x060040D6 RID: 16598 RVA: 0x00151448 File Offset: 0x0014F848
		public override string ToString()
		{
			return string.Format("{0}.{1}", this.major, this.minor);
		}

		// Token: 0x060040D7 RID: 16599 RVA: 0x0015146A File Offset: 0x0014F86A
		public static bool IsValidVersionString(string version)
		{
			return !string.IsNullOrEmpty(version) && Regex.IsMatch(version, "^(?<major>[0-9]+).(?<minor>[0-9]+)(.(?<build>[0-9]+))?\\z");
		}

		// Token: 0x04006AB1 RID: 27313
		public int major;

		// Token: 0x04006AB2 RID: 27314
		public int minor;

		// Token: 0x04006AB3 RID: 27315
		[NonSerialized]
		private const string VERSION_REGEX_FORMAT = "^(?<major>[0-9]+).(?<minor>[0-9]+)(.(?<build>[0-9]+))?\\z";
	}
}
