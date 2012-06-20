using System;
using System.Runtime.InteropServices;
using HttpParser.Native;

namespace HttpParser
{
	public class HttpParser
	{
		private http_parser _parser;
		private http_parser_settings _settings;
		
		public HttpParser () : this(new http_parser())
		{
		}
		
		public HttpParser (http_parser parser)
		{
			_parser = parser;
		}
		
		public void Initialize (HttpParserType type)
		{
			NativeHttpParser.http_parser_init (ref _parser, type);
		}
		
		public void Execute (string data)
		{
			NativeHttpParser.http_parser_execute (ref _parser, ref _settings, data, (uint)data.Length);
		}
		
		public static HttpParser Create ()
		{
			return new HttpParser ();
		}
		
		public static HttpParser Create (http_parser parser)
		{
			var httpParser = new HttpParser (parser);
			return httpParser;
		}
		
		public static HttpParser Create (IntPtr parserPtr)
		{
			var parser = (http_parser)Marshal.PtrToStructure (parserPtr, typeof(http_parser));
			return Create (parser);
		}
		
		public Func<HttpParser, int> OnMessageBegin
		{
			set { _settings.on_message_begin = CreateCallback(value); }
		}
		
		public Func<HttpParser, int> OnMessageComplete
		{
			set { _settings.on_message_complete = CreateCallback(value); }
		}
		
		public Func<HttpParser, int> OnHeadersComplete
		{
			set { _settings.on_headers_complete = CreateCallback(value); }
		}
		
		public Func<HttpParser, string, int> OnHeaderField
		{
			set { _settings.on_header_field = CreateDataCallback (value); }
		}
		
		public Func<HttpParser, string, int> OnHeaderValue
		{
			set { _settings.on_header_value = CreateDataCallback (value); }
		}
		
		public Func<HttpParser, string, int> OnUrl
		{
			set { _settings.on_url = CreateDataCallback (value); }
		}
		
		public Func<HttpParser, string, int> OnBody
		{
			set { _settings.on_body = CreateDataCallback (value); }
		}
		
		public HttpParserType Type
		{
			get { return (HttpParserType)(_parser.typeAndFlags & 0x3); }
		}
		
		public HttpParserFlags Flags 
		{ 
			get { return (HttpParserFlags)(_parser.typeAndFlags >> 2); }
		}
		
		public byte State 
		{
			get { return _parser.state; }
		}
		
		public byte HeaderState 
		{
			get { return _parser.header_state; }
		}
		
		public byte Index
		{
			get { return _parser.index; }
		}
		
		public uint NRead
		{
			get { return _parser.nread; }
		}
		
		public long ContentLength 
		{ 
			get { return _parser.content_length; } 
		}
		
		public ushort HttpMajor
		{
			get { return _parser.http_major; }
		}
		
		public ushort HttpMinor
		{
			get { return _parser.http_minor; }
		}
		
		public HttpMethod Method 
		{
			get { return (HttpMethod)_parser.method; }
		}
		
		public int HttpErrNo
		{
			get { return _parser.httpErrNoAndUpgrade & 0x127; }
		}
		
		public int Upgrade
		{
			get { return _parser.httpErrNoAndUpgrade >> 7; }
		}
		
		public int ErrorLineNo
		{
			get { return _parser.error_lineno; }
		}
		
		public object Data
		{
			get { return _parser.data; }
		}
		
		private static http_cb CreateCallback (Func<HttpParser, int> callback)
		{
			var inner = callback;
			return (http_cb)((parserPtr) =>
			{
				var parser = HttpParser.Create (parserPtr);
				return inner.Invoke (parser);
			});
		}
		
		private static http_data_cb CreateDataCallback (Func<HttpParser, string, int> callback)
		{
			var inner = callback;
			return (http_data_cb)((parserPtr, at, len) =>
			{
				var parser = HttpParser.Create (parserPtr);
				var data = at.Substring (0, len);
				return inner.Invoke (parser, data);
			});
		}
	}
}