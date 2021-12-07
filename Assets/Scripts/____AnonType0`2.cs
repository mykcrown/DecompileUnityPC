// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

internal sealed class ____AnonType0<_a___T, _t___T>
{
	private readonly _a___T _a_;

	private readonly _t___T _t_;

	public _a___T a
	{
		get
		{
			return this._a_;
		}
	}

	public _t___T t
	{
		get
		{
			return this._t_;
		}
	}

	public ____AnonType0(_a___T a, _t___T t)
	{
		this._a_ = a;
		this._t_ = t;
	}

	public override bool Equals(object obj)
	{
		var ____AnonType = obj as ____AnonType0<_a___T, _t___T>;
		return ____AnonType != null && EqualityComparer<_a___T>.Default.Equals(this._a_, ____AnonType._a_) && EqualityComparer<_t___T>.Default.Equals(this._t_, ____AnonType._t_);
	}

	public override int GetHashCode()
	{
		int num = ((-2128831035 ^ EqualityComparer<_a___T>.Default.GetHashCode(this._a_)) * 16777619 ^ EqualityComparer<_t___T>.Default.GetHashCode(this._t_)) * 16777619;
		num += num << 13;
		num ^= num >> 7;
		num += num << 3;
		num ^= num >> 17;
		return num + (num << 5);
	}

	public override string ToString()
	{
		string[] expr_06 = new string[6];
		expr_06[0] = "{";
		expr_06[1] = " a = ";
		int arg_46_1 = 2;
		string arg_46_2;
		if (this._a_ != null)
		{
			_a___T _a___T = this._a_;
			arg_46_2 = _a___T.ToString();
		}
		else
		{
			arg_46_2 = string.Empty;
		}
		expr_06[arg_46_1] = arg_46_2;
		expr_06[3] = ", t = ";
		int arg_7F_1 = 4;
		string arg_7F_2;
		if (this._t_ != null)
		{
			_t___T _t___T = this._t_;
			arg_7F_2 = _t___T.ToString();
		}
		else
		{
			arg_7F_2 = string.Empty;
		}
		expr_06[arg_7F_1] = arg_7F_2;
		expr_06[5] = " }";
		return string.Concat(expr_06);
	}
}
