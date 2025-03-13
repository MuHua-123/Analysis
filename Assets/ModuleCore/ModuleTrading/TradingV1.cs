using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 交易V1
/// </summary>
public class TradingV1 : ModuleTrading {
	/// <summary> 单位 </summary>
	public int unit;
	/// <summary> 资金 </summary>
	public float fund;
	/// <summary> 交易间隔 </summary>
	public float interval;
	/// <summary> 最大持仓 </summary>
	public float maxHold;
	/// <summary> 持仓 </summary>
	public List<DataHold> holds = new List<DataHold>();

	/// <summary>
	/// 初始化操作
	/// 设定每份多少股
	/// 按入场当天价格取一半资金买入持仓
	/// 以整数段为基础向上排序
	/// </summary>
	/// <param name="fund">资金量</param>
	public TradingV1(float fund, int unit, float buy, float interval) {
		this.unit = unit;
		this.fund = fund;
		this.interval = interval;

		//取一半资金
		float half = fund * 0.5f;
		//计算最大买入量
		int quantity = (int)(half / buy);
		//计算买入份数
		int share = quantity / unit;
		//卖出起步价
		float selling = ((int)(buy / interval) + 1) * interval;
		//买入持仓
		for (int i = 0; i < share; i++) { BuyHold(buy, selling + ((1 + i) * interval)); }
	}

	public override void Execute(DataAnalysis analysis) {
		//循环判断买入
		BuyHold(analysis);
		//循环判断卖出
		SellHold(analysis);
	}

	/// <summary> 买入持仓(循环) </summary>
	private void BuyHold(DataAnalysis analysis) {
		DataHold hold = MinBuy();
		if (hold == null) {
			//卖出起步价
			float selling = ((int)(analysis.min / interval) + 1) * interval;
			BuyHold(analysis.min, selling);
			return;
		}
		float differ = hold.buy - analysis.min;
		if ((differ - interval) < 0) { return; }
		//买入价=最小持仓购买价-间隔，卖出价=最小持仓购买价
		BuyHold(hold.buy - interval, hold.buy);
		BuyHold(analysis);
	}
	private void BuyHold(float buy, float selling) {
		DataHold hold = new DataHold(unit, buy, selling);
		fund -= hold.BuyTotal;
		holds.Add(hold);
		maxHold = Mathf.Max(maxHold, HoldValue());
		Debug.LogError($"买入价格：{hold.BuyTotal} , 可用现金:{fund} , 持仓:{HoldValue()}");
	}

	/// <summary> 卖出持仓(循环) </summary>
	private void SellHold(DataAnalysis analysis) {
		DataHold hold = MinSell();
		if (hold == null) { return; }
		float differ = analysis.max - hold.sell;
		if ((differ - interval) < 0) { return; }
		SellHold(hold);
		SellHold(analysis);
	}
	private void SellHold(DataHold hold) {
		fund += hold.SellTotal;
		holds.Remove(hold);
		Debug.LogWarning($"卖出价格:{hold.SellTotal} , 盈利:{hold.SellTotal - hold.BuyTotal} , 可用现金:{fund} , 持仓:{HoldValue()}");
	}

	private DataHold MinBuy() {
		float buy = float.MaxValue; DataHold hold = null;
		foreach (var item in holds) {
			if (item.buy < buy) { buy = item.buy; hold = item; }
		}
		return hold;
	}
	private DataHold MinSell() {
		float sell = float.MaxValue; DataHold hold = null;
		foreach (var item in holds) {
			if (item.sell < sell) { sell = item.sell; hold = item; }
		}
		return hold;
	}
	private float HoldValue() {
		float value = 0;
		holds.ForEach(obj => value += obj.BuyTotal);
		return value;
	}
}
