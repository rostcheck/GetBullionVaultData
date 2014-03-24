using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using BullionVaultProxy;

namespace GetBullionVaultData
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			BullionVault vaultProxy = new BullionVault();
			//			SecureString secureUser = GetSecureString("User: ");
			//SecureString securePwd = GetSecureString("Password: ");

//			ListPrices(vaultProxy);
//			Console.WriteLine("Cookies: " + vaultProxy.NumberOfCookies);
//			vaultProxy.Connect(GetString("User: "), GetString("Password: "));
//			Console.WriteLine("Cookies: " + vaultProxy.NumberOfCookies);
//			ListPrices(vaultProxy);
//			Console.WriteLine("Cookies: " + vaultProxy.NumberOfCookies);

			vaultProxy.Connect(GetString("User: "), GetString("Password: "));
			List<Order> orderList = vaultProxy.GetOrders(OrderStatusEnum.Closed, new DateTime(2006, 1, 1), new DateTime(2006, 12, 31));
			Console.WriteLine("Found " + orderList.Count + " orders (" + 
				orderList.Where(s => s.ProcessingStatus == OrderProcessingStatusEnum.Cancelled).Count() +
				" cancelled)");
			foreach (Order order in orderList.Where(s => s.ProcessingStatus != OrderProcessingStatusEnum.Cancelled).ToList())
			{
				Console.WriteLine(string.Format("{0} {1} {2} {3} for {4} {5}", 
					order.OrderID, order.Action, order.Quantity, order.Metal, order.Value,
					order.Currency));
			}
		}

		private static void ListPrices(BullionVault vaultProxy)
		{
			foreach (MarketPrice price in 
				vaultProxy.MarketPrices().Where(s => s.CurrencyType == CurrencyTypeEnum.USD))
			{
				Console.WriteLine(string.Format("{0} {1} in {2} buy {3} @ {4}, sell {5} @ {6}", 
					price.VaultLocation, price.MetalType, price.CurrencyType, 
					price.BuyAmount, price.BuyPrice, price.SellAmount, price.SellPrice));
			}
		}

		private static string GetString(string userPrompt)
		{
			string result = string.Empty;
			Console.Write(userPrompt);
			ConsoleKeyInfo key = Console.ReadKey();
			while (key.Key != ConsoleKey.Enter)
			{
				result += key.KeyChar;
				key = Console.ReadKey();
			}
			Console.WriteLine();
			return result;
		}

		private static SecureString GetSecureString(string userPrompt)
		{
			SecureString result = new SecureString();
			Console.Write(userPrompt);
			ConsoleKeyInfo key = Console.ReadKey();
			while (key.Key != ConsoleKey.Enter)
			{
				result.AppendChar(key.KeyChar);
				key = Console.ReadKey();
			}
			Console.WriteLine();
			return result;
		}
	}
}
