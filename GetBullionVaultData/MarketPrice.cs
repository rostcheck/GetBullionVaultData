using System;

namespace GetBullionVaultData
{
	public class MarketPrice
	{
		public MarketPrice(MetalTypeEnum metalType, VaultLocationEnum vaultLocation, 
			CurrencyTypeEnum currencyType, decimal buyAmount, decimal buyPrice, decimal sellAmount,
			decimal sellPrice)
		{
			MetalType = metalType;
			VaultLocation = vaultLocation;
			CurrencyType = currencyType;
			BuyAmount = buyAmount;
			BuyPrice = buyPrice;
			SellAmount = sellAmount;
			SellPrice = sellPrice;
		}

		public MetalTypeEnum MetalType { get; set; }
		public VaultLocationEnum VaultLocation { get; set; }
		public CurrencyTypeEnum CurrencyType { get; set; }
		public decimal BuyAmount { get; set; }
		public decimal BuyPrice { get; set; }
		public decimal SellAmount { get; set; }
		public decimal SellPrice { get; set; }
	}
}

