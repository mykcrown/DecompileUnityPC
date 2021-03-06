// Decompile from assembly: Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
	public enum EHTTPStatusCode
	{
		k_EHTTPStatusCodeInvalid,
		k_EHTTPStatusCode100Continue = 100,
		k_EHTTPStatusCode101SwitchingProtocols,
		k_EHTTPStatusCode200OK = 200,
		k_EHTTPStatusCode201Created,
		k_EHTTPStatusCode202Accepted,
		k_EHTTPStatusCode203NonAuthoritative,
		k_EHTTPStatusCode204NoContent,
		k_EHTTPStatusCode205ResetContent,
		k_EHTTPStatusCode206PartialContent,
		k_EHTTPStatusCode300MultipleChoices = 300,
		k_EHTTPStatusCode301MovedPermanently,
		k_EHTTPStatusCode302Found,
		k_EHTTPStatusCode303SeeOther,
		k_EHTTPStatusCode304NotModified,
		k_EHTTPStatusCode305UseProxy,
		k_EHTTPStatusCode307TemporaryRedirect = 307,
		k_EHTTPStatusCode400BadRequest = 400,
		k_EHTTPStatusCode401Unauthorized,
		k_EHTTPStatusCode402PaymentRequired,
		k_EHTTPStatusCode403Forbidden,
		k_EHTTPStatusCode404NotFound,
		k_EHTTPStatusCode405MethodNotAllowed,
		k_EHTTPStatusCode406NotAcceptable,
		k_EHTTPStatusCode407ProxyAuthRequired,
		k_EHTTPStatusCode408RequestTimeout,
		k_EHTTPStatusCode409Conflict,
		k_EHTTPStatusCode410Gone,
		k_EHTTPStatusCode411LengthRequired,
		k_EHTTPStatusCode412PreconditionFailed,
		k_EHTTPStatusCode413RequestEntityTooLarge,
		k_EHTTPStatusCode414RequestURITooLong,
		k_EHTTPStatusCode415UnsupportedMediaType,
		k_EHTTPStatusCode416RequestedRangeNotSatisfiable,
		k_EHTTPStatusCode417ExpectationFailed,
		k_EHTTPStatusCode4xxUnknown,
		k_EHTTPStatusCode429TooManyRequests = 429,
		k_EHTTPStatusCode500InternalServerError = 500,
		k_EHTTPStatusCode501NotImplemented,
		k_EHTTPStatusCode502BadGateway,
		k_EHTTPStatusCode503ServiceUnavailable,
		k_EHTTPStatusCode504GatewayTimeout,
		k_EHTTPStatusCode505HTTPVersionNotSupported,
		k_EHTTPStatusCode5xxUnknown = 599
	}
}
