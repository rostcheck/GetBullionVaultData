using System;

namespace BullionVaultProxy
{
	public static class Utils
	{
		public static MetalTypeEnum GetMetalType(string metalTypeAsString)
		{
			switch (metalTypeAsString.ToLower())
			{
				case "silver":
					return MetalTypeEnum.Silver;
				case "gold":
					return MetalTypeEnum.Gold;
				default:
					throw new Exception("Unknown metal type " + metalTypeAsString);
			}
		}

		public static VaultLocationEnum GetVaultLocation(string vaultSymbol)
		{
			if (vaultSymbol.Length < 5)
				throw new Exception("Unrecognized vault symbol " + vaultSymbol);

			switch (vaultSymbol.Substring(3, 2))
			{
				case "LN":
					return VaultLocationEnum.London;
				case "ZU":
					return VaultLocationEnum.Zurich;
				case "TR":
					return VaultLocationEnum.Toronto;
				case "SG":
					return VaultLocationEnum.Singapore;
				case "NY":
					return VaultLocationEnum.NewYork;
				default:
					throw new Exception("Unknown vault location " + vaultSymbol);
			}
		}

		public static CurrencyTypeEnum GetCurrencyType(string currencyTypeAsString)
		{
			switch (currencyTypeAsString)
			{
				case "EUR":
					return CurrencyTypeEnum.EUR;
				case "USD":
					return CurrencyTypeEnum.USD;
				case "GBP":
					return CurrencyTypeEnum.GBP;
				default:
					throw new Exception("Unknown currency type " + currencyTypeAsString);
			}
		}

		public static string GetVaultName(VaultLocationEnum vaultLocation)
		{
			switch (vaultLocation)
			{
				case VaultLocationEnum.London:
					return "London";
				case VaultLocationEnum.Zurich:
					return "Zurich";
				case VaultLocationEnum.Toronto:
					return "Toronto";
				case VaultLocationEnum.Singapore:
					return "Singapore";
				case VaultLocationEnum.NewYork:
					return "New York";
				default:
					return "Unknown";
			}
		}

		public static string GetOrderStatus(OrderStatusEnum orderStatus)
		{
			switch (orderStatus)
			{
				case OrderStatusEnum.Closed:
					return "CLOSED";
				case OrderStatusEnum.Dealt:
					return "DEALT";
				case OrderStatusEnum.OpenOrDealt:
					return "OPEN_DEALT";
				case OrderStatusEnum.Rejected:
					return "REJECTED";
				case OrderStatusEnum.Open:
					return "OPEN";
				default:
					return "Unknown";
			}
		}
	}
}

