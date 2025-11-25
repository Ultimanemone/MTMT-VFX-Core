using System;
using UnityEngine;

// Token: 0x0200002A RID: 42
public static class TransformExtension
{
	// Token: 0x06000089 RID: 137 RVA: 0x00005348 File Offset: 0x00003548
	public static GameObject[] FindRootObject(this Transform transform)
	{
		return Array.FindAll<GameObject>(UnityEngine.Object.FindObjectsOfType<GameObject>(), (GameObject item) => item.transform.parent == null);
	}
}