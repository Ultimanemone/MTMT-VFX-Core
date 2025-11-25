using System;

// Token: 0x02000004 RID: 4
public class BM_Selectable<T>
{
	// Token: 0x17000001 RID: 1
	// (get) Token: 0x06000008 RID: 8 RVA: 0x000024D3 File Offset: 0x000006D3
	// (set) Token: 0x06000009 RID: 9 RVA: 0x000024DB File Offset: 0x000006DB
	public T Value
	{
		get
		{
			return this.mValue;
		}
		set
		{
			this.mValue = value;
			this.OnChanged(this.mValue);
		}
	}

	// Token: 0x0600000A RID: 10 RVA: 0x000024F0 File Offset: 0x000006F0
	public BM_Selectable()
	{
		this.mValue = default(T);
	}

	// Token: 0x0600000B RID: 11 RVA: 0x00002504 File Offset: 0x00000704
	public BM_Selectable(T value)
	{
		this.mValue = value;
	}

	// Token: 0x0600000C RID: 12 RVA: 0x00002513 File Offset: 0x00000713
	public void SetValueWithoutCallback(T value)
	{
		this.mValue = value;
	}

	// Token: 0x0600000D RID: 13 RVA: 0x0000251C File Offset: 0x0000071C
	public void SetValueIfNotEqual(T value)
	{
		if (this.mValue.Equals(value))
		{
			return;
		}
		this.mValue = value;
		this.OnChanged(this.mValue);
	}

	// Token: 0x0600000E RID: 14 RVA: 0x0000254C File Offset: 0x0000074C
	private void OnChanged(T value)
	{
		Action<T> action = this.mChanged;
		if (action == null)
		{
			return;
		}
		action(value);
	}

	// Token: 0x0400000B RID: 11
	private T mValue;

	// Token: 0x0400000C RID: 12
	public Action<T> mChanged;
}
