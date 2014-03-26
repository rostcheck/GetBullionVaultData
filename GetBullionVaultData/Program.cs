using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using BullionVaultProxy;

namespace GetBullionVaultData
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			BullionVault vaultProxy = new BullionVault();
			// This doesn't work on Xamarin Mac - NetworkCredential.Password doesn't expose the decrypted password
//			NetworkCredential credential = new NetworkCredential();
//			credential.UserName = GetString("User: ");
//			credential.SecurePassword = GetSecureString("Password: ");
			vaultProxy.Connect(GetString("User: "), GetString("Password: "));
			//vaultProxy.Connect(credential);
			List<Order> orderList = vaultProxy.GetOrders(OrderStatusEnum.Closed, new DateTime(2006, 1, 1), new DateTime(2013, 12, 31));
			List<Order> successfulOrders = orderList.Where(s => s.ProcessingStatus == OrderProcessingStatusEnum.Done)
				.OrderBy(s => s.OrderID).ToList();
			List<Order> cancelledOrders = orderList.Where(s => s.ProcessingStatus == OrderProcessingStatusEnum.Cancelled)
				.OrderBy(s => s.OrderID).ToList();
			Console.WriteLine("Found " + orderList.Count + " orders (" + cancelledOrders.Count() + " cancelled)");
			foreach (Order order in successfulOrders)
			{
				Console.WriteLine(string.Format("{0} {1} {2} {3} {4} for {5} {6}", 
					order.OrderID, order.OrderDateTime, order.Action, order.Quantity, order.Metal, order.Value,
					order.Currency));
			}
			FileSerializer.Save(successfulOrders, "BullionVault.txt");
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
			return result;
		}
	}
}
