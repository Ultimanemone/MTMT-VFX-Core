using System;
using System.Linq;
using UnityEngine;

// Token: 0x0200000A RID: 10
public static class GameObjectExtensions
{
	// Token: 0x06000024 RID: 36 RVA: 0x000032B8 File Offset: 0x000014B8
	public static GameObject[] GetChildren(this GameObject self, bool includeInactive = false)
	{
		return (from c in self.GetComponentsInChildren<Transform>(includeInactive)
		where c != self.transform
		select c.gameObject).ToArray<GameObject>();
	}
}
