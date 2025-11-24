using System;
using System.Reflection;

namespace BM_EffectUpdate
{
	// Token: 0x02000034 RID: 52
	internal static class BM_Reflection
	{
		// Token: 0x060000AC RID: 172 RVA: 0x00006AC0 File Offset: 0x00004CC0
		public static Type GetClass(string name, Type obj)
		{
			return (Type)obj.GetType().GetField(name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField).GetValue(obj);
		}
	}
}
