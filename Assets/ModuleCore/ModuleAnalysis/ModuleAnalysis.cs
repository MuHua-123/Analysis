using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuHua;

/// <summary>
/// 分析模块
/// </summary>
public class ModuleAnalysis : MonoBehaviour {
	private void Start() {
		ModuleCollector.I.GetStock("601658", Trading);
	}

	private void Trading(List<DataAnalysis> analyses) {
		DataAnalysis analysis = analyses[0];
		TradingV1 trading = new TradingV1(100000, 1000, analysis.max, 0.1f);
		for (int i = 1; i < analyses.Count; i++) {
			trading.Execute(analyses[i]);
		}
		DataAnalysis last = analyses[analyses.Count - 1];
		Debug.Log($"可用现金：{trading.fund}");
		Debug.Log($"持仓份数：{trading.holds.Count}");
		Debug.Log($"持仓价值：{last.min * trading.holds.Count * 1000}");
		float value = 0;
		foreach (var item in trading.holds) {
			value += item.BuyTotal;
		}
		Debug.Log($"持仓价值：{value}");
		Debug.Log($"最大持仓：{trading.maxHold}");
	}
}
