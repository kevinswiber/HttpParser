using System;
using System.Runtime.InteropServices;
using HttpParser;
using HttpParser.Native;

namespace HttpParser.Example
{
	class MainClass
	{
		private static http_data_cb printData (string label)
		{
			return (parserPtr, at, len) =>
			{
				var field = at.Substring (0, len);
				Console.WriteLine (label + ": " + field);
					
				return 0;
			};
		}
		
		private static http_cb print (string label)
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
			var settings = new http_parser_settings ();
			settings.on_header_field = printData ("on header field");
			settings.on_header_value = printData ("on header value");
			settings.on_url = printData ("on url");
			settings.on_body = printData ("on body");
			settings.on_headers_complete = print ("headers complete");
			settings.on_message_begin = print ("message begin");
			settings.on_message_complete = print ("message complete");

			var parser = HttpParser.Create ();
			parser.Initialize (HttpParserType.Request);
			
			parser.Execute (settings, data);
		}
	}
}
