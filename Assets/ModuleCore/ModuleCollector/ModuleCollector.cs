using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuHua;

/// <summary>
/// 数据采集
/// </summary>
public class ModuleCollector : Module<ModuleCollector> {
	/// <summary> 接口 ：https://api.mairui.club/hszbl/fsjy/股票代码(如000001)/分时级别/licence证书 </summary>
	public string API => "https://api.mairui.club/hszbl/fsjy/";

	public void GetStock(string code, Action<List<DataAnalysis>> action, bool isCache = true) {
		string json = SaveTool.LoadText(FileName.Create(code));
		if (json != null && json != "" && isCache) { StockToAnalysis(json, action); return; }

		//https://api.mairui.club/hszbl/fsjy/000001/60m/b997d4403688d5e66a
		string url = $"{API}{code}/dn/2E111385-7BF1-473D-9210-8E22AA75375A";
		Debug.Log($"{code}重新缓存了数据!");

		DataRequestGet request = new DataRequestGet(url);
		request.OnError = (json) => { Debug.Log(json); };
		request.OnCallback = (json) => {
			SaveTool.SaveText(FileName.Create(code), json);
			StockToAnalysis(json, action);
		};
		AsyncWebRequest.Execute(request);
	}

	public void StockToAnalysis(string json, Action<List<DataAnalysis>> action = null) {
		List<DataStock> stocks = JsonTool.FromJson<List<DataStock>>(json);
		List<DataAnalysis> analyses = new List<DataAnalysis>();
		for (int i = 0; i < stocks.Count; i++) { analyses.Add(stocks[i].To()); }
		action?.Invoke(analyses);
	}
}
