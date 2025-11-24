using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000009 RID: 9
public class MovementLocusCalculator
{
	// Token: 0x0600001E RID: 30 RVA: 0x00002EF4 File Offset: 0x000010F4
	public static List<Vector3> calcBezierCurvePoints(Vector3 pointStart, List<Vector3> pointControl, Vector3 pointEnd, int devideNum)
	{
		List<Vector3> list = new List<Vector3>
		{
			pointStart
		};
		list.AddRange(pointControl);
		list.Add(pointEnd);
		int count = list.Count;
		int[] table = MovementLocusCalculator._table[count - 1];
		List<Vector3> list2 = new List<Vector3>(devideNum);
		for (int i = 0; i < devideNum; i++)
		{
			List<float> multis = MovementLocusCalculator._calcBezierMultis((float)i * 1f / (float)(devideNum - 1), count, table);
			list2.Add(MovementLocusCalculator._calcBezierPoint(multis, list));
		}
		return list2;
	}

	// Token: 0x0600001F RID: 31 RVA: 0x00002F6C File Offset: 0x0000116C
	private static List<float> _calcBezierMultis(float t, int numPoint, int[] table)
	{
		List<float> list = new List<float>();
		for (int i = 0; i < numPoint; i++)
		{
			float num = (float)table[i];
			float item;
			if (i == 0)
			{
				item = num * Mathf.Pow(1f - t, (float)(numPoint - 1));
			}
			else if (i == numPoint - 1)
			{
				item = num * Mathf.Pow(t, (float)i);
			}
			else
			{
				item = num * Mathf.Pow(t, (float)i) * Mathf.Pow(1f - t, (float)(numPoint - 1 - i));
			}
			list.Add(item);
		}
		return list;
	}

	// Token: 0x06000020 RID: 32 RVA: 0x00002FE8 File Offset: 0x000011E8
	private static Vector3 _calcBezierPoint(List<float> multis, List<Vector3> points)
	{
		float num = 0f;
		for (int i = 0; i < points.Count; i++)
		{
			num += points[i].x * multis[i];
		}
		float num2 = 0f;
		for (int j = 0; j < points.Count; j++)
		{
			num2 += points[j].y * multis[j];
		}
		float num3 = 0f;
		for (int k = 0; k < points.Count; k++)
		{
			num3 += points[k].z * multis[k];
		}
		return new Vector3(num, num2, num3);
	}

	// Token: 0x06000021 RID: 33 RVA: 0x00003094 File Offset: 0x00001294
	public static List<Vector3> calcHermiteCurvePoints(Vector3 pointStart, Vector3 normalStart, Vector3 pointEnd, Vector3 normalEnd, int devideNum)
	{
		List<Vector3> list = new List<Vector3>(devideNum);
		for (int i = 0; i < devideNum; i++)
		{
			float num = (float)i * 1f / (float)(devideNum - 1);
			float num2 = num * num;
			float num3 = num2 * num;
			float num4 = 2f * num3 - 3f * num2 + 0f + 1f;
			float num5 = num3 - 2f * num2 + num;
			float num6 = -2f * num3 + 3f * num2;
			float num7 = num3 - num2;
			float num8 = pointStart.x * num4 + normalStart.x * num5 + pointEnd.x * num6 + normalEnd.x * num7;
			float num9 = pointStart.y * num4 + normalStart.y * num5 + pointEnd.y * num6 + normalEnd.y * num7;
			float num10 = pointStart.z * num4 + normalStart.z * num5 + pointEnd.z * num6 + normalEnd.z * num7;
			list.Add(new Vector3(num8, num9, num10));
		}
		return list;
	}

	// Token: 0x0400002A RID: 42
	private static readonly int[][] _table = new int[][]
	{
		new int[]
		{
			1
		},
		new int[]
		{
			1,
			1
		},
		new int[]
		{
			1,
			2,
			1
		},
		new int[]
		{
			1,
			3,
			3,
			1
		},
		new int[]
		{
			1,
			4,
			6,
			4,
			1
		},
		new int[]
		{
			1,
			5,
			10,
			10,
			5,
			1
		},
		new int[]
		{
			1,
			6,
			15,
			20,
			15,
			6,
			1
		},
		new int[]
		{
			1,
			7,
			21,
			35,
			35,
			21,
			7,
			1
		},
		new int[]
		{
			1,
			8,
			28,
			56,
			70,
			56,
			28,
			8,
			1
		},
		new int[]
		{
			1,
			9,
			36,
			84,
			126,
			126,
			84,
			36,
			9,
			1
		},
		new int[]
		{
			1,
			10,
			45,
			120,
			210,
			252,
			210,
			120,
			45,
			10,
			1
		},
		new int[]
		{
			1,
			11,
			55,
			165,
			330,
			464,
			464,
			330,
			165,
			55,
			11,
			1
		}
	};
}
