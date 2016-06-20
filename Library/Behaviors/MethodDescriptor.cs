using System.Reflection;

namespace Behaviors
{
	[Preserve(AllMembers = true)]
	internal sealed class MethodDescriptor
	{
		public MethodInfo MethodInfo { get; private set; }

		public ParameterInfo[] Parameters { get; private set; }

		public int ParameterCount { get { return Parameters.Length; } }

		public TypeInfo SecondParameterTypeInfo
		{
			get
			{
				if (ParameterCount < 2)
				{
					return null;
				}
				return Parameters[1].ParameterType.GetTypeInfo();
			}
		}

		public MethodDescriptor(MethodInfo methodInfo, ParameterInfo[] parameters)
		{
			MethodInfo = methodInfo;
			Parameters = parameters;
		}
	}
}

