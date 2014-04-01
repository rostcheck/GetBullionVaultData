using System;
using System.Collections.Generic;
using System.IO;

namespace BullionVaultProxy
{
	public static class FileSerializer
	{
		public static void Save(List<Order> orderList, string fileName)
		{
			StreamWriter writer = new StreamWriter(fileName, false); 
			writer.WriteLine("OrderDateTime\tOrderID\tAction\tVault\tValue\tClientTransRef\tCurrency\t"
				+ "Metal\tGoodTil\tLastModified\tPricePerKg\tOrderType\tStatus\tQuantity\tQtyFilled\tCommission\t"
				+ "Consideration\tTradeType");
				
			foreach (Order order in orderList)
			{
				writer.WriteLine(string.Format(
					"{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\t{12}\t{13}\t{14}\t{15}\t{16}\t{17}",
					order.OrderDateTime, order.OrderID, order.Action, order.Vault, order.Value, 
					order.ClientTransactionReference, order.Currency, order.Metal, order.GoodUntil,
					order.LastModified, order.Limit, order.OrderType, order.ProcessingStatus, 
					order.Quantity, order.QuantityFilled, order.TotalCommission, order.TotalConsideration,
					order.TradeType));
			}
			writer.Close();
		}
	}
}

