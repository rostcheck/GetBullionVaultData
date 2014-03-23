using System;
using System.Net;
using System.IO;
using System.Xml;
using System.Xml.XPath; 

namespace GetBullionVaultData
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://live.bullionvault.com/view_market_xml.do");
			request.Method = "GET";
			request.Accept = "gzip";
			HttpWebResponse response = (HttpWebResponse)request.GetResponse();

			// Display the status.
			if (response.StatusCode != HttpStatusCode.OK)
			{
				Console.WriteLine("Returned status: " + response.StatusDescription);
				return;
			}

			// Get the stream containing content returned by the server.
			Stream dataStream = response.GetResponseStream();

			// Open the stream using a StreamReader for easy access.
			//StreamReader reader = new StreamReader(dataStream);

			// Read the content.
			//string responseFromServer = reader.ReadToEnd();

			// Display the content.
			//Console.WriteLine(responseFromServer);

			XPathDocument document = new XPathDocument(dataStream);
			XPathNavigator navigator = document.CreateNavigator();
			XPathNodeIterator nodes = navigator.Select("/envelope/message/market/pitches/pitch");
			while (nodes.MoveNext())
			{
				//Console.WriteLine(nodes.Current.InnerXml);
				string buyPrice = "", buyAmount = "", sellPrice = "", sellAmount = "";
				string metal = nodes.Current.GetAttribute("securityClassNarrative", string.Empty);
				string vault = GetVaultName(nodes.Current.GetAttribute("securityId", string.Empty));
				string currency = nodes.Current.GetAttribute("considerationCurrency", string.Empty);
				XPathNodeIterator priceNodes = nodes.Current.Select("*/price");
				string innerXML = nodes.Current.InnerXml;
				while (priceNodes.MoveNext())
				{
					if (priceNodes.Current.GetAttribute("actionIndicator", string.Empty) == "B")
					{
						buyPrice = priceNodes.Current.GetAttribute("limit", string.Empty);
						buyAmount = priceNodes.Current.GetAttribute("quantity", string.Empty);
					}
					else
					{
						sellPrice = priceNodes.Current.GetAttribute("limit", string.Empty);
						sellAmount = priceNodes.Current.GetAttribute("quantity", string.Empty);
					}
				}
				Console.WriteLine(string.Format("{0} {1} {2} buy {3} @ {4}, sell {5} @ {6}", metal, vault, 
					currency, buyAmount, buyPrice, sellAmount, sellPrice));
			}
		}

		private static string GetVaultName(string vaultSymbol)
		{
			if (vaultSymbol.Length < 5)
				throw new Exception("Unrecognized vault symbol " + vaultSymbol);

			switch (vaultSymbol.Substring(3, 2))
			{
				case "LN":
					return "London";
				case "ZU":
					return "Zurich";
				case "TR":
					return "Toronto";
				case "SG":
					return "Singapore";
				case "NY":
					return "New York";
				default:
					return "Unknown";
			}
		}
	}
}
