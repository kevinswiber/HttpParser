using System;
using System.Runtime.InteropServices;

namespace HttpParser.Native
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate int http_data_cb(IntPtr parser, string at, int length);
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate int http_cb(IntPtr parser);
	
	public class NativeHttpParser
	{
		[DllImport("http_parser")]
		public static extern void http_parser_init(ref http_parser parser, HttpParserType type);
		[DllImport("http_parser")]
		public static extern uint http_parser_execute(ref http_parser parser, ref http_parser_settings settings, string data, uint len);
	}
	
	public struct http_parser
	{
		public byte typeAndFlags;
		public byte state;
		public byte header_state;
		public byte index;
		
		public uint nread;
		public ulong content_length;
		
		public ushort http_major;
		public ushort http_minor;
		public ushort status_code;
		public byte method;
		public byte httpErrNoAndUpgrade;
		public int error_lineno;
		public object data;
	}
	
	public struct http_parser_settings
	{
		public http_cb on_message_begin; 
		public http_data_cb on_url;
		public http_data_cb on_header_field;
		public http_data_cb on_header_value;
		public http_cb on_headers_complete;
		public http_data_cb on_body;
		public http_cb on_message_complete;
	}
}

