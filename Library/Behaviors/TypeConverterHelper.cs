using System;

namespace Behaviors
{
	[Preserve(AllMembers = true)]
	internal static class TypeConverterHelper
	{
		public static object Convert(string value, string destinationTypeFullName)
		{
			if (string.IsNullOrWhiteSpace(destinationTypeFullName))
			{
				throw new ArgumentNullException(destinationTypeFullName);
			}

			string scope = GetScope(destinationTypeFullName);

			if (string.Equals(scope, "System", StringComparison.Ordinal))
			{
				if (string.Equals(destinationTypeFullName, typeof(string).FullName, StringComparison.Ordinal))
				{
					return value;
				}
				else if (string.Equals(destinationTypeFullName, typeof(bool).FullName, StringComparison.Ordinal))
				{
					return bool.Parse(value);
				}
				else if (string.Equals(destinationTypeFullName, typeof(int).FullName, StringComparison.Ordinal))
				{
					return int.Parse(value);
				}
				else if (string.Equals(destinationTypeFullName, typeof(double).FullName, StringComparison.Ordinal))
				{
					return double.Parse(value);
				}
			}

			return null;
		}

		static string GetScope(string name)
		{
			int indexOfLastPeriod = name.LastIndexOf('.');
			if (indexOfLastPeriod != name.Length - 1)
			{
				return name.Substring(0, indexOfLastPeriod);
			}
			return name;
		}
	}
}

