using System;
using System.Runtime.InteropServices;
using HttpParser;
using HttpParser.Native;

namespace HttpParser.Example
{
	class MainClass
	{
		private static Func<HttpParser, string, int> printData (string label)
		{
			return (parser, field) =>
			{
				Console.WriteLine (label + ": " + field);
				return 0;
			};
		}
		
		private static Func<HttpParser, int> print (string label)
		{
			return _ => {
				Console.WriteLine (label + " fired");
				return 0;
			};
		}
		
		public static void Main (string[] args)
		{
			var data = "GET /test HTTP/1.1\r\n" +
				"User-Agent: curl/7.18.0 (i486-pc-linux-gnu) libcurl/7.18.0 OpenSSL/0.9.8g zlib/1.2.3.3 libidn/1.1\r\n" +
				"Host: 0.0.0.0=5000\r\n" +
				"Accept: */*\r\n" +
				"\r\n";
			
			var data2 = "POST /post_identity_body_world?q=search#hey HTTP/1.1\r\n" + 
				"Accept: */*\r\n" + 
				"Transfer-Encoding: identity\r\n" + 
				"Content-Length: 5\r\n" + 
				"Upgrade: WebSocket\r\n" +
				"\r\n" + 
				"World";
			
			Parse (data);
			Parse (data2);
		}
		
		private static void Parse (string data)
		{
			var parser = HttpParser.Create ();
			parser.Initialize (HttpParserType.Request);
			
			parser.OnHeaderField = printData ("on header field");
			parser.OnHeaderValue = printData ("on header value");
			parser.OnUrl = printData ("on url");
			parser.OnBody = printData ("on body");
			parser.OnHeadersComplete = print ("headers complete");
			parser.OnMessageBegin = print ("message begin");
			parser.OnMessageComplete = print ("message complete");
			
			parser.Execute (data);
		}
	}
}
