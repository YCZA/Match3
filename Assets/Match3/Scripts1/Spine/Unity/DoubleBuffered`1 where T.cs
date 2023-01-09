using System;

namespace Match3.Scripts1.Spine.Unity
{
	// Token: 0x02000234 RID: 564
	public class DoubleBuffered<T> where T : new()
	{
		// Token: 0x060011AC RID: 4524 RVA: 0x0003144D File Offset: 0x0002F84D
		public T GetNext()
		{
			this.usingA = !this.usingA;
			return (!this.usingA) ? this.b : this.a;
		}

		// Token: 0x040041E2 RID: 16866
		private readonly T a = Activator.CreateInstance<T>();

		// Token: 0x040041E3 RID: 16867
		private readonly T b = Activator.CreateInstance<T>();

		// Token: 0x040041E4 RID: 16868
		private bool usingA;
	}
}
