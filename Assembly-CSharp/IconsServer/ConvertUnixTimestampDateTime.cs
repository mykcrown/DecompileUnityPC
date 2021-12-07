using System;

namespace IconsServer
{
	// Token: 0x020007AF RID: 1967
	public class ConvertUnixTimestampDateTime
	{
		// Token: 0x060030EF RID: 12527 RVA: 0x000F0AD4 File Offset: 0x000EEED4
		public static long CurrentUTCUnixTimestampSeconds()
		{
			return (long)(DateTime.UtcNow - ConvertUnixTimestampDateTime.UnixEpoch).TotalSeconds;
		}

		// Token: 0x060030F0 RID: 12528 RVA: 0x000F0AFC File Offset: 0x000EEEFC
		public static long UTCUnixTimestampSecondsFromUCTDateTime(DateTime time)
		{
			return (long)(time - ConvertUnixTimestampDateTime.UnixEpoch).TotalSeconds;
		}

		// Token: 0x060030F1 RID: 12529 RVA: 0x000F0B1D File Offset: 0x000EEF1D
		public static long UTCUnixTimestampSecondsFromLocalDateTime(DateTime time)
		{
			return ConvertUnixTimestampDateTime.UTCUnixTimestampSecondsFromUCTDateTime(time.ToUniversalTime());
		}

		// Token: 0x060030F2 RID: 12530 RVA: 0x000F0B2C File Offset: 0x000EEF2C
		public static DateTime UTCDateTimeFromUTCUnixTimestampSeconds(long seconds)
		{
			return ConvertUnixTimestampDateTime.UnixEpoch.AddSeconds((double)seconds);
		}

		// Token: 0x060030F3 RID: 12531 RVA: 0x000F0B48 File Offset: 0x000EEF48
		public static DateTime LocalDateTimeFromUTCUnixTimestampSeconds(long seconds)
		{
			return ConvertUnixTimestampDateTime.UTCDateTimeFromUTCUnixTimestampSeconds(seconds).ToLocalTime();
		}

		// Token: 0x04002232 RID: 8754
		private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
	}
}
