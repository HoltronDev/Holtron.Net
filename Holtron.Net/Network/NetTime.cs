namespace Holtron.Net.Network
{
    /// <summary>
    /// Time service
    /// </summary>
    public static partial class NetTime
	{
		/// <summary>
		/// Given seconds it will output a human friendly readable string (milliseconds if less than 60 seconds)
		/// </summary>
		public static string ToReadable(double seconds)
		{
			if (seconds > 60)
				return TimeSpan.FromSeconds(seconds).ToString();
			return (seconds * 1000.0).ToString("N2") + " ms";
		}
	}
}