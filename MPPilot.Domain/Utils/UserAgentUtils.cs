using System.Text.RegularExpressions;

namespace MPPilot.Domain.Utils
{
	public static class UserAgentUtils
	{
		public static string GetOS(string userAgent)
		{
			var osInfo = "Unknown";

			if (userAgent.Contains("Windows NT 10.0"))
				osInfo = "Windows 10";
			else if (userAgent.Contains("Windows NT 6.3"))
				osInfo = "Windows 8.1";
			else if (userAgent.Contains("Windows NT 6.2"))
				osInfo = "Windows 8";
			else if (userAgent.Contains("Windows NT 6.1"))
				osInfo = "Windows 7";
			else if (userAgent.Contains("Mac OS X"))
				osInfo = "Mac OS X";
			else if (userAgent.Contains("Linux"))
				osInfo = "Linux";

			return osInfo;
		}

		public static string GetDevice(string userAgent)
		{
			if (userAgent.Contains("Mobile") || userAgent.Contains("Android"))
				return "Mobile";
			else if (userAgent.Contains("Tablet"))
				return "Tablet";
			else if (userAgent.Contains("iPad"))
				return "iPad";
			else if (userAgent.Contains("iPhone"))
				return "iPhone";
			else if (userAgent.Contains("Windows Phone"))
				return "Windows Phone";
			else if (userAgent.Contains("BlackBerry"))
				return "BlackBerry";
			else
				return "Desktop";
		}

		public static string GetBrowser(string userAgent)
		{
			var browser = "Unknown";
			var version = "Unknown";

			if (userAgent.Contains("MSIE") || userAgent.Contains("Trident/"))
			{
				browser = "Internet Explorer";
				version = GetVersionFromUserAgent(userAgent, "MSIE", "rv:");
			}
			else if (userAgent.Contains("Edge"))
			{
				browser = "Microsoft Edge";
				version = GetVersionFromUserAgent(userAgent, "Edge");
			}
			else if (userAgent.Contains("Chrome"))
			{
				browser = "Google Chrome";
				version = GetVersionFromUserAgent(userAgent, "Chrome");
			}
			else if (userAgent.Contains("Safari") && !userAgent.Contains("Chrome"))
			{
				browser = "Safari";
				version = GetVersionFromUserAgent(userAgent, "Version");
			}
			else if (userAgent.Contains("Firefox"))
			{
				browser = "Mozilla Firefox";
				version = GetVersionFromUserAgent(userAgent, "Firefox");
			}

			return $"{browser} {version}";
		}

		private static string GetVersionFromUserAgent(string userAgent, params string[] searchKeywords)
		{
			foreach (var keyword in searchKeywords)
			{
				var match = Regex.Match(userAgent, $@"{keyword}/([\d\.]+)");
				if (match.Success)
				{
					return match.Groups[1].Value;
				}
			}
			return "Unknown";
		}
	}
}
