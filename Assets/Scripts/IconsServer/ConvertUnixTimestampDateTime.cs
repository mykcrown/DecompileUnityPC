// Decompile from assembly: Assembly-CSharp.dll

using System;

namespace IconsServer
{
	public class ConvertUnixTimestampDateTime
	{
		private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		public static long CurrentUTCUnixTimestampSeconds()
		{
			return (long)(DateTime.UtcNow - ConvertUnixTimestampDateTime.UnixEpoch).TotalSeconds;
		}

		public static long UTCUnixTimestampSecondsFromUCTDateTime(DateTime time)
		{
			return (long)(time - ConvertUnixTimestampDateTime.UnixEpoch).TotalSeconds;
		}

		public static long UTCUnixTimestampSecondsFromLocalDateTime(DateTime time)
		{
			return ConvertUnixTimestampDateTime.UTCUnixTimestampSecondsFromUCTDateTime(time.ToUniversalTime());
		}

		public static DateTime UTCDateTimeFromUTCUnixTimestampSeconds(long seconds)
		{
			return ConvertUnixTimestampDateTime.UnixEpoch.AddSeconds((double)seconds);
		}

		public static DateTime LocalDateTimeFromUTCUnixTimestampSeconds(long seconds)
		{
			return ConvertUnixTimestampDateTime.UTCDateTimeFromUTCUnixTimestampSeconds(seconds).ToLocalTime();
		}
	}
}
