using System;
namespace HttpParser
{
	public enum HttpMethod {
		Delete = 0,
		Get,
		Head,
		Post,
		Put,
		/* pathological */
		Connect,
		Options,
		Trace,
		/* webdav */
		Copy,
		Lock,
		Mkcol,
		Move,
		Propfind,
		Proppatch,
		Search,
		Unlock,
		/* subversion */
		Report,
		Mkactivity,
		Checkout,
		Merge,
		/* upnp */
		Msearch,
		Notify,
		Subscribe,
		Unsubscribe,
		/* RFC-5789 */
		Patch,
		Purge
	}
	
	public enum HttpParserType {
		Request,
		Response,
		Both
	}
	
	[Flags]
	public enum HttpParserFlags
	{
		Chunked = 1,
		ConnectionKeepAlive = 2,
		ConnectionClose = 4,
		Trailing = 8,
		Upgrade = 16,
		SkipBody = 32
	}
	
	// TODO: Fill in http_errno
	public enum HttpErrNo { }
}

