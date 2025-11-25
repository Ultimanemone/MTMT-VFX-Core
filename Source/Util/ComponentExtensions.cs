using System;
using System.Linq;
using UnityEngine;

// Token: 0x0200000B RID: 11
public static class ComponentExtensions
{
	// Token: 0x06000025 RID: 37 RVA: 0x00003318 File Offset: 0x00001518
	public static GameObject[] GetChildren(this Component self, bool includeInactive = false)
	{
		return (from c in self.GetComponentsInChildren<Transform>(includeInactive)
		where c != self.transform
		select c.gameObject).ToArray<GameObject>();
	}
}
