using System;
using UnityEngine;

// Token: 0x02000014 RID: 20
public class SM_Create_MyLineMesh : MonoBehaviour
{
	// Token: 0x06000041 RID: 65 RVA: 0x00003BA4 File Offset: 0x00001DA4
	private void Start()
	{
		this.mesh = new Mesh();
		Vector3[] array = new Vector3[this.SplitMesh * 4];
		Vector2[] array2 = new Vector2[this.SplitMesh * 4];
		int[] array3 = new int[6 * this.SplitMesh];
		for (int i = 0; i < this.SplitMesh * 4; i += 2)
		{
			array[i] = new Vector3(-0.5f, 0f, (float)(i / 2));
			array[i + 1] = new Vector3(0.5f, 0f, (float)(i / 2));
			array2[i] = new Vector2((float)(i / 2) / (float)this.SplitMesh, 0f);
			array2[i + 1] = new Vector2((float)(i / 2) / (float)this.SplitMesh, 1f);
		}
		int j = 0;
		int num = 0;
		while (j < this.SplitMesh * 2 * 3)
		{
			array3[j] = num;
			array3[j + 1] = 2 + num;
			array3[j + 2] = 1 + num;
			array3[j + 3] = 2 + num;
			array3[j + 4] = 3 + num;
			array3[j + 5] = 1 + num;
			j += 6;
			num += 2;
		}
		this.mesh.vertices = array;
		this.mesh.uv = array2;
		this.mesh.triangles = array3;
		base.GetComponent<MeshFilter>().sharedMesh = this.mesh;
		base.GetComponent<MeshFilter>().sharedMesh.name = "MyLineMesh";
	}

	// Token: 0x06000042 RID: 66 RVA: 0x00002BE3 File Offset: 0x00000DE3
	private void Update()
	{
	}

	// Token: 0x0400004F RID: 79
	private Mesh mesh;

	// Token: 0x04000050 RID: 80
	public int SplitMesh = 1;
}
