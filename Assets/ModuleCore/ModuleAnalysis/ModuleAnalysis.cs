using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuHua;

/// <summary>
/// 分析模块
/// </summary>
public class ModuleAnalysis : MonoBehaviour {
	public List<DataAnalysis> analyses;
	private void Start() {
		ModuleCollector.I.GetStock("601658", (analyses) => { this.analyses = analyses; });


	}
}
