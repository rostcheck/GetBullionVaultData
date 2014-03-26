using System;

namespace BullionVaultProxy
{
	public class Order
	{
		public Order(UInt64 orderID, string clientTransactionReference, ActionEnum action, MetalTypeEnum metal,
			CurrencyTypeEnum currency, decimal quantity, decimal quantityFilled, decimal totalConsideration,
			decimal totalCommission, int limit, OrderTypeEnum orderType, DateTime orderDateTime, 
			DateTime? goodUntil, DateTime lastModified, OrderProcessingStatusEnum processingStatus, 
			TradeTypeEnum tradeType, decimal value, VaultLocationEnum vault)
		{
			OrderID = orderID;
			ClientTransactionReference = clientTransactionReference;
			Action = action;
			Metal = metal;
			Currency = currency;
			Quantity = quantity;
			QuantityFilled = quantityFilled;
			TotalConsideration = totalConsideration;
			TotalCommission = totalCommission;
			Limit = limit;
			OrderType = orderType;
			OrderDateTime = orderDateTime;
			GoodUntil = goodUntil;
			LastModified = lastModified;
			ProcessingStatus = processingStatus;
			TradeType = tradeType;
			Value = value; 
			Vault = vault;
		}

		public UInt64 OrderID { get; set; }
		public string ClientTransactionReference { get; set; }
		public ActionEnum Action { get; set; }
		public MetalTypeEnum Metal { get; set; }
		public CurrencyTypeEnum Currency { get; set; }
		public decimal Quantity { get; set; }
		public decimal QuantityFilled { get; set; }
		public decimal TotalConsideration { get; set; }
		public decimal TotalCommission { get; set; }
		public int Limit { get; set; }
		public OrderTypeEnum OrderType { get; set; }
		public DateTime OrderDateTime { get; set; }
		public DateTime? GoodUntil { get; set; }
		public DateTime LastModified { get; set; }
		public OrderProcessingStatusEnum ProcessingStatus { get; set; }
		public TradeTypeEnum TradeType { get; set; }
		public decimal Value { get; set; }
		public VaultLocationEnum Vault { get; set; }
	}
}

