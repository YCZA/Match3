using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace Wooga.Core.Utilities
{
	// Token: 0x020003B6 RID: 950
	[Serializable]
	public sealed class SemVersion : IComparable<SemVersion>, IComparable, ISerializable
	{
		// Token: 0x06001C96 RID: 7318 RVA: 0x0007CC1C File Offset: 0x0007B01C
		private SemVersion(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			SemVersion semVersion = SemVersion.Parse(info.GetString("SemVersion"), false);
			this.Major = semVersion.Major;
			this.Minor = semVersion.Minor;
			this.Patch = semVersion.Patch;
			this.Prerelease = semVersion.Prerelease;
			this.Build = semVersion.Build;
		}

		// Token: 0x06001C97 RID: 7319 RVA: 0x0007CC90 File Offset: 0x0007B090
		public SemVersion(int major, int minor = 0, int patch = 0, string prerelease = "", string build = "")
		{
			this.Major = major;
			this.Minor = minor;
			this.Patch = patch;
			this.Prerelease = string.Intern(prerelease ?? string.Empty);
			this.Build = string.Intern(build ?? string.Empty);
		}

		// Token: 0x06001C98 RID: 7320 RVA: 0x0007CCEC File Offset: 0x0007B0EC
		public SemVersion(Version version)
		{
			version = (version ?? new Version());
			this.Major = version.Major;
			this.Minor = version.Minor;
			if (version.Revision >= 0)
			{
				this.Patch = version.Revision;
			}
			this.Prerelease = string.Intern(string.Empty);
			if (version.Build > 0)
			{
				this.Build = string.Intern(version.Build.ToString());
			}
			else
			{
				this.Build = string.Intern(string.Empty);
			}
		}

		// Token: 0x06001C99 RID: 7321 RVA: 0x0007CD90 File Offset: 0x0007B190
		public static SemVersion Parse(string version, bool strict = false)
		{
			System.Text.RegularExpressions.Match match = SemVersion.parseEx.Match(version);
			if (!match.Success)
			{
				throw new ArgumentException("Invalid version.", "version");
			}
			int major = int.Parse(match.Groups["major"].Value, CultureInfo.InvariantCulture);
			Group group = match.Groups["minor"];
			int minor = 0;
			if (group.Success)
			{
				minor = int.Parse(group.Value, CultureInfo.InvariantCulture);
			}
			else if (strict)
			{
				throw new InvalidOperationException("Invalid version (no minor version given in strict mode)");
			}
			Group group2 = match.Groups["patch"];
			int patch = 0;
			if (group2.Success)
			{
				patch = int.Parse(group2.Value, CultureInfo.InvariantCulture);
			}
			else if (strict)
			{
				throw new InvalidOperationException("Invalid version (no patch version given in strict mode)");
			}
			string value = match.Groups["pre"].Value;
			string value2 = match.Groups["build"].Value;
			return new SemVersion(major, minor, patch, value, value2);
		}

		// Token: 0x06001C9A RID: 7322 RVA: 0x0007CEB0 File Offset: 0x0007B2B0
		public static bool TryParse(string version, out SemVersion semver, bool strict = false)
		{
			bool result;
			try
			{
				semver = SemVersion.Parse(version, strict);
				result = true;
			}
			catch (Exception)
			{
				semver = null;
				result = false;
			}
			return result;
		}

		// Token: 0x06001C9B RID: 7323 RVA: 0x0007CEEC File Offset: 0x0007B2EC
		public static bool Equals(SemVersion versionA, SemVersion versionB)
		{
			if (object.ReferenceEquals(versionA, null))
			{
				return object.ReferenceEquals(versionB, null);
			}
			return versionA.Equals(versionB);
		}

		// Token: 0x06001C9C RID: 7324 RVA: 0x0007CF09 File Offset: 0x0007B309
		public static int Compare(SemVersion versionA, SemVersion versionB)
		{
			if (object.ReferenceEquals(versionA, null))
			{
				return (!object.ReferenceEquals(versionB, null)) ? -1 : 0;
			}
			return versionA.CompareTo(versionB);
		}

		// Token: 0x06001C9D RID: 7325 RVA: 0x0007CF34 File Offset: 0x0007B334
		public SemVersion Change(int? major = null, int? minor = null, int? patch = null, string prerelease = null, string build = null)
		{
			return new SemVersion((major == null) ? this.Major : major.Value, (minor == null) ? this.Minor : minor.Value, (patch == null) ? this.Patch : patch.Value, prerelease ?? this.Prerelease, build ?? this.Build);
		}

		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x06001C9E RID: 7326 RVA: 0x0007CFBE File Offset: 0x0007B3BE
		// (set) Token: 0x06001C9F RID: 7327 RVA: 0x0007CFC6 File Offset: 0x0007B3C6
		public int Major { get; private set; }

		// Token: 0x1700047B RID: 1147
		// (get) Token: 0x06001CA0 RID: 7328 RVA: 0x0007CFCF File Offset: 0x0007B3CF
		// (set) Token: 0x06001CA1 RID: 7329 RVA: 0x0007CFD7 File Offset: 0x0007B3D7
		public int Minor { get; private set; }

		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x06001CA2 RID: 7330 RVA: 0x0007CFE0 File Offset: 0x0007B3E0
		// (set) Token: 0x06001CA3 RID: 7331 RVA: 0x0007CFE8 File Offset: 0x0007B3E8
		public int Patch { get; private set; }

		// Token: 0x1700047D RID: 1149
		// (get) Token: 0x06001CA4 RID: 7332 RVA: 0x0007CFF1 File Offset: 0x0007B3F1
		// (set) Token: 0x06001CA5 RID: 7333 RVA: 0x0007CFF9 File Offset: 0x0007B3F9
		public string Prerelease { get; private set; }

		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x06001CA6 RID: 7334 RVA: 0x0007D002 File Offset: 0x0007B402
		// (set) Token: 0x06001CA7 RID: 7335 RVA: 0x0007D00A File Offset: 0x0007B40A
		public string Build { get; private set; }

		// Token: 0x06001CA8 RID: 7336 RVA: 0x0007D014 File Offset: 0x0007B414
		public override string ToString()
		{
			string text = string.Concat(new object[]
			{
				string.Empty,
				this.Major,
				".",
				this.Minor,
				".",
				this.Patch
			});
			if (!string.IsNullOrEmpty(this.Prerelease))
			{
				text = text + "-" + this.Prerelease;
			}
			if (!string.IsNullOrEmpty(this.Build))
			{
				text = text + "+" + this.Build;
			}
			return text;
		}

		// Token: 0x06001CA9 RID: 7337 RVA: 0x0007D0B4 File Offset: 0x0007B4B4
		public int CompareTo(object obj)
		{
			return this.CompareTo((SemVersion)obj);
		}

		// Token: 0x06001CAA RID: 7338 RVA: 0x0007D0C4 File Offset: 0x0007B4C4
		public int CompareTo(SemVersion other)
		{
			if (object.ReferenceEquals(other, null))
			{
				return 1;
			}
			int num = this.CompareByPrecedence(other);
			if (num != 0)
			{
				return num;
			}
			return SemVersion.CompareComponent(this.Build, other.Build, false);
		}

		// Token: 0x06001CAB RID: 7339 RVA: 0x0007D103 File Offset: 0x0007B503
		public bool PrecedenceMatches(SemVersion other)
		{
			return this.CompareByPrecedence(other) == 0;
		}

		// Token: 0x06001CAC RID: 7340 RVA: 0x0007D110 File Offset: 0x0007B510
		public int CompareByPrecedence(SemVersion other)
		{
			if (object.ReferenceEquals(other, null))
			{
				return 1;
			}
			int num = this.Major.CompareTo(other.Major);
			if (num != 0)
			{
				return num;
			}
			num = this.Minor.CompareTo(other.Minor);
			if (num != 0)
			{
				return num;
			}
			num = this.Patch.CompareTo(other.Patch);
			if (num != 0)
			{
				return num;
			}
			return SemVersion.CompareComponent(this.Prerelease, other.Prerelease, true);
		}

		// Token: 0x06001CAD RID: 7341 RVA: 0x0007D198 File Offset: 0x0007B598
		private static int CompareComponent(string a, string b, bool lower = false)
		{
			bool flag = string.IsNullOrEmpty(a);
			bool flag2 = string.IsNullOrEmpty(b);
			if (flag && flag2)
			{
				return 0;
			}
			if (flag)
			{
				return (!lower) ? -1 : 1;
			}
			if (flag2)
			{
				return (!lower) ? 1 : -1;
			}
			string[] array = a.Split(new char[]
			{
				'.'
			});
			string[] array2 = b.Split(new char[]
			{
				'.'
			});
			int num = Math.Min(array.Length, array2.Length);
			for (int i = 0; i < num; i++)
			{
				string text = array[i];
				string text2 = array2[i];
				int num2;
				bool flag3 = int.TryParse(text, out num2);
				int value;
				bool flag4 = int.TryParse(text2, out value);
				if (flag3 && flag4)
				{
					return num2.CompareTo(value);
				}
				if (flag3)
				{
					return -1;
				}
				if (flag4)
				{
					return 1;
				}
				int num3 = string.CompareOrdinal(text, text2);
				if (num3 != 0)
				{
					return num3;
				}
			}
			return array.Length.CompareTo(array2.Length);
		}

		// Token: 0x06001CAE RID: 7342 RVA: 0x0007D2A4 File Offset: 0x0007B6A4
		public override bool Equals(object obj)
		{
			if (object.ReferenceEquals(obj, null))
			{
				return false;
			}
			if (object.ReferenceEquals(this, obj))
			{
				return true;
			}
			SemVersion semVersion = (SemVersion)obj;
			return this.Major == semVersion.Major && this.Minor == semVersion.Minor && this.Patch == semVersion.Patch && object.ReferenceEquals(this.Prerelease, semVersion.Prerelease) && object.ReferenceEquals(this.Build, semVersion.Build);
		}

		// Token: 0x06001CAF RID: 7343 RVA: 0x0007D334 File Offset: 0x0007B734
		public override int GetHashCode()
		{
			int num = this.Major.GetHashCode();
			num = num * 31 + this.Minor.GetHashCode();
			num = num * 31 + this.Patch.GetHashCode();
			num = num * 31 + this.Prerelease.GetHashCode();
			return num * 31 + this.Build.GetHashCode();
		}

		// Token: 0x06001CB0 RID: 7344 RVA: 0x0007D3AD File Offset: 0x0007B7AD
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("SemVersion", this.ToString());
		}

		// Token: 0x06001CB1 RID: 7345 RVA: 0x0007D3D1 File Offset: 0x0007B7D1
		public static implicit operator SemVersion(string version)
		{
			return SemVersion.Parse(version, false);
		}

		// Token: 0x06001CB2 RID: 7346 RVA: 0x0007D3DA File Offset: 0x0007B7DA
		public static bool operator ==(SemVersion left, SemVersion right)
		{
			return SemVersion.Equals(left, right);
		}

		// Token: 0x06001CB3 RID: 7347 RVA: 0x0007D3E3 File Offset: 0x0007B7E3
		public static bool operator !=(SemVersion left, SemVersion right)
		{
			return !SemVersion.Equals(left, right);
		}

		// Token: 0x06001CB4 RID: 7348 RVA: 0x0007D3EF File Offset: 0x0007B7EF
		public static bool operator >(SemVersion left, SemVersion right)
		{
			return SemVersion.Compare(left, right) == 1;
		}

		// Token: 0x06001CB5 RID: 7349 RVA: 0x0007D3FB File Offset: 0x0007B7FB
		public static bool operator >=(SemVersion left, SemVersion right)
		{
			return left == right || left > right;
		}

		// Token: 0x06001CB6 RID: 7350 RVA: 0x0007D413 File Offset: 0x0007B813
		public static bool operator <(SemVersion left, SemVersion right)
		{
			return SemVersion.Compare(left, right) == -1;
		}

		// Token: 0x06001CB7 RID: 7351 RVA: 0x0007D41F File Offset: 0x0007B81F
		public static bool operator <=(SemVersion left, SemVersion right)
		{
			return left == right || left < right;
		}

		// Token: 0x0400499F RID: 18847
		private static Regex parseEx = new Regex("^(?<major>\\d+)(\\.(?<minor>\\d+))?(\\.(?<patch>\\d+))?(\\-(?<pre>[0-9A-Za-z\\-\\.]+))?(\\+(?<build>[0-9A-Za-z\\-\\.]+))?$", RegexOptions.ExplicitCapture | RegexOptions.CultureInvariant);
	}
}
