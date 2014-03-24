﻿using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Security;
using System.Text;
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
			string URL = SecureLoginBaseURL + "j_security_check";
			string postData = string.Format("j_username={0}&j_password={1}", userName, password);
			CallAPI(URL, postData); // Throws exception on auth error
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

		// Note: BullionVault strongly recommends restricting order type to only Open for bots
		public void GetOrders(OrderStatusEnum orderStatus, DateTime fromDate, DateTime toDate)
		{
			while (true)
			{
				int pageNumber = 0;
				string URL = SecureBaseURL + "view_orders_xml.do";
				Dictionary<string, string> postData = new Dictionary<string, string>();
				postData.Add("status", Utils.GetOrderStatus(orderStatus));
				postData.Add("fromDate", fromDate.ToString("yyyyMMdd"));
				postData.Add("toDate", toDate.ToString("yyyyMMdd"));
				postData.Add("page", pageNumber.ToString());

				XPathDocument document = new XPathDocument(CallAPI(URL, postData));
				XPathNavigator navigator = document.CreateNavigator();
				XPathNodeIterator nodes = navigator.Select("/envelope/message");
				int pageSize = Convert.ToInt32(nodes.Current.GetAttribute("pageSize", string.Empty));
				int thisPageNumber = Convert.ToInt32(nodes.Current.GetAttribute("pageSize", string.Empty));
				nodes = navigator.Select("/envelope/message/orders/order");
				while (nodes.MoveNext())
				{

				}
			}
//			Stream resultStream = CallAPI(URL, postData);
//			StreamReader reader = new StreamReader(resultStream);
//			Console.WriteLine(reader.ReadToEnd());
		}

		private Stream CallAPI(string URL, Dictionary<string, string> postValues)
		{
			StringBuilder sb = new StringBuilder();
			foreach (string key in postValues.Keys)
			{
				if (sb.Length > 0)
					sb.Append(@"&");
				sb.AppendFormat("{0}={1}", key, postValues[key]);
			}
			return CallAPI(URL, sb.ToString());

		}

		private Stream CallAPI(string URL, string postData = "")
		{
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
			request.Method = (postData != null && postData.Length > 0) ? "POST" : "GET";
			request.Accept = "gzip";
			request.CookieContainer = cookieContainer;

			// Create POST data, convert it to a byte array, write it to the request stream
			if ((postData != null && postData.Length > 0))
			{
				byte[] byteArray = Encoding.UTF8.GetBytes(postData);
				request.ContentType = "application/x-www-form-urlencoded";
				request.ContentLength = byteArray.Length;
				Stream dataStream = request.GetRequestStream();
				dataStream.Write(byteArray, 0, byteArray.Length);
				dataStream.Close();
			}

			HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			if (response.StatusCode != HttpStatusCode.OK)
			{
				throw new Exception("Returned status: " + response.StatusDescription);
			}

			// Get the stream containing content returned by the server.
			return response.GetResponseStream();
		}
	}
}
