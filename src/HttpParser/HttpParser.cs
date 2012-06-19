using System;
using System.Runtime.InteropServices;
using HttpParser.Native;

namespace HttpParser
{
	public class HttpParser
	{
		private http_parser _parser;
		
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
		
		public void Execute (http_parser_settings settings, string data)
		{
			NativeHttpParser.http_parser_execute (ref _parser, ref settings, data, (uint)data.Length);
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
			get { return _parser.httpErrNoAndUpgrade & 0127; }
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
	}
}