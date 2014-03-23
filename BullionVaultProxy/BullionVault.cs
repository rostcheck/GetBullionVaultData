using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Security;
using System.Xml;
using System.Xml.XPath; 

namespace BullionVaultProxy
{
	/// <summary>
	/// Proxy for web service operations to BullionVault's web API
	/// </summary>
	public class BullionVault
	{	
		const string InsecureBaseURL = "http://live.bullionvault.com/";
		const string SecureLoginBaseURL = "https://live.bullionvault.com/secure/";
		const string SecureBaseURL = "https://live.bullionvault.com/secure/api/v2/";
		private CookieContainer cookieContainer;
		private bool isConnected = false;

		public BullionVault()
		{
			cookieContainer = new CookieContainer();
		}

		public void Connect(string userName, string password)
		{
			string URL = SecureLoginBaseURL + string.Format("j_security_check?j_username={0}&j_password={1}", userName,
				             password);
			CallAPI(URL); // Throws exception on auth error
			//StreamReader reader = new StreamReader(loginResult);
			//Console.WriteLine(reader.ReadToEnd());
			isConnected = true;
		}

		public int NumberOfCookies 
		{
			get 
			{
				return cookieContainer.Count;
			}
		}

		public bool IsConnected()
		{
			return isConnected;
		}

		public List<MarketPrice> MarketPrices()
		{
			List<MarketPrice> returnPrices = new List<MarketPrice>();
			const string urlPart = "view_market_xml.do";
			string URL = isConnected ? SecureBaseURL + urlPart : InsecureBaseURL + urlPart ; 
			XPathDocument document = new XPathDocument(CallAPI(URL));
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

		private Stream CallAPI(string URL)
		{
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
			request.Method = "GET";
			request.Accept = "gzip";
			request.CookieContainer = cookieContainer;
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

