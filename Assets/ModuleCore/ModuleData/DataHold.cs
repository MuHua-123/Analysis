using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 持有数据
/// </summary>
public class DataHold {
	/// <summary> 买入数量 </summary>
	public readonly float quantity;

	/// <summary> 买入单价 </summary>
	public readonly float buy;
	/// <summary> 买入总价 </summary>
	public float BuyTotal => buy * quantity;

	/// <summary> 卖出价格 </summary>
	public readonly float sell;
	/// <summary> 卖出价格 </summary>
	public float SellTotal => sell * quantity;

	public DataHold(float quantity, float buy, float selling) {
		this.quantity = quantity;
		this.buy = buy;
		this.sell = selling;
	}
}
