using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

[Serializable]
public class DataStock {
	/// <summary> 日期 </summary>
	public string d;
	/// <summary> 开盘价（元） </summary>
	public string o;
	/// <summary> 最高价（元） </summary>
	public string h;
	/// <summary> 最低价（元） </summary>
	public string l;
	/// <summary> 收盘价（元） </summary>
	public string c;
	/// <summary> 成交量（手） </summary>
	public string v;
	/// <summary> 成交额（元） </summary>
	public string e;
	/// <summary> 振幅（%） </summary>
	public string zf;
	/// <summary> 换手率（%） </summary>
	public string hs;
	/// <summary> 涨跌幅（%） </summary>
	public string zd;
	/// <summary> 涨跌额（元） </summary>
	public string zde;
	/// <summary>  </summary>
	public string ud;

	public DataAnalysis To() {
		DataAnalysis analysis = new DataAnalysis();
		//analysis.dateTime = DateTime.ParseExact(d, "yyyy-MM-dd", CultureInfo.CurrentCulture);
		analysis.dateTime = d;
		analysis.max = float.Parse(h);
		analysis.min = float.Parse(l);
		return analysis;
	}
}