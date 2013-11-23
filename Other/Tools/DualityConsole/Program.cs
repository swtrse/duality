namespace Duality.Tools.DualityConsole
{
	using System;
	using System.Net;
	using System.Net.Sockets;
	using System.Reflection;
	using System.Text;

	using Duality.Tools.DualityConsole.Properties;

	using log4net;

	internal static class Program
	{
		#region Static Fields

		private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		#endregion

		#region Methods

		private static void Main()
		{
			var sender = new IPEndPoint(IPAddress.Any, 0);
			
			try
			{
				using (var client = new UdpClient(Settings.Default.Port))
				{
					while (true)
					{
						byte[] buffer = client.Receive(ref sender);
						string logLine = Encoding.Default.GetString(buffer);
						if (logLine.Contains(Settings.Default.DebugMessage))
						{
							Log.Debug(logLine);
						}
						else if (logLine.Contains(Settings.Default.InfoMessage))
						{
							Log.Info(logLine);
						}
						else if (logLine.Contains(Settings.Default.WarnMessage))
						{
							Log.Warn(logLine);
						}
						else if (logLine.Contains(Settings.Default.ErrorMessage))
						{
							Log.Error(logLine);
						}
						else if (logLine.Contains(Settings.Default.FatalMessage))
						{
							Log.Fatal(logLine);
						}
						else
						{
							Log.ErrorFormat("Unknown Messagetype.{0}Message: {1}", Environment.NewLine, logLine);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Log.Fatal("Unexpected termination of the debug console.", ex);
				Console.WriteLine(Resources.Program_Main_Close_Message);
				Console.ReadLine();
			}
		}

		#endregion
	}
}