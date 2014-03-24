using System;

namespace BullionVaultProxy
{
	public class Order
	{
		public Order()
		{
		}

		public string OrderID { get; set; }
		public string ClientTransactionReference { get; set; }
		public ActionEnum Action { get; set; }
		public MetalTypeEnum Metal { get; set; }
		public CurrencyTypeEnum Currency { get; set; }
		public decimal Quantity { get; set; }
		public decimal QuantityFilled { get; set; }
		public decimal TotalConsideration { get; set; }
		public decimal TotalCommission { get; set; }
		public int Limit { get; set; }

	}
}

