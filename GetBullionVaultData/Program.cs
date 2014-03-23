using System;
using System.Linq;


namespace GetBullionVaultData
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			BullionVault vaultProxy = new BullionVault();
			foreach (MarketPrice price in 
				vaultProxy.MarketPrices().Where(s => s.CurrencyType == CurrencyTypeEnum.USD))
			{
				Console.WriteLine(string.Format("{0} {1} in {2} buy {3} @ {4}, sell {5} @ {6}", 
					price.VaultLocation, price.MetalType, price.CurrencyType, 
					price.BuyAmount, price.BuyPrice, price.SellAmount, price.SellPrice));
			}

		}
	}
}
