using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Xml;
using System.Xml.XPath; 

namespace GetBullionVaultData
{
	/// <summary>
	/// Proxy for web service operations to BullionVault's web API
	/// </summary>
	public class BullionVault
	{
		const string BaseURL = "http://live.bullionvault.com/";

		public BullionVault()
		{
		}

		public void Connect()
		{
		}

		public bool IsConnected()
		{
			return false;
		}

		public List<MarketPrice> MarketPrices()
		{
			List<MarketPrice> returnPrices = new List<MarketPrice>();
			XPathDocument document = new XPathDocument(CallAPI("view_market_xml.do"));
			XPathNavigator navigator = document.CreateNavigator();
			XPathNodeIterator nodes = navigator.Select("/envelope/message/market/pitches/pitch");
			while (nodes.MoveNext())
			{
				string buyPrice = "", buyAmount = "", sellPrice = "", sellAmount = "";
				string metal = nodes.Current.GetAttribute("securityClassNarrative", string.Empty);
				string vault = nodes.Current.GetAttribute("securityId", string.Empty);
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
				MarketPrice price = new MarketPrice(Utils.GetMetalType(metal), Utils.GetVaultLocation(vault),
					                    Utils.GetCurrencyType(currency), Convert.ToDecimal(buyAmount), Convert.ToDecimal(buyPrice),
					                    Convert.ToDecimal(sellAmount), Convert.ToDecimal(sellPrice));
				returnPrices.Add(price);
			}
			return returnPrices;
		}

		private Stream CallAPI(string URLPart)
		{
			string URL = string.Format("{0}/{1}", BaseURL, URLPart);
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
			request.Method = "GET";
			request.Accept = "gzip";
			HttpWebResponse response = (HttpWebResponse)request.GetResponse();

			// Display the status.
			if (response.StatusCode != HttpStatusCode.OK)
			{
				throw new Exception("Returned status: " + response.StatusDescription);
			}

			// Get the stream containing content returned by the server.
			return response.GetResponseStream();
		}
	}
}

