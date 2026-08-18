using System;
using System.Linq;

namespace Unity.Infrastructure.Windows
{
	[AttributeUsage(AttributeTargets.Class)]
	public class WindowAttribute : Attribute
	{
		public WindowAttribute(string name)
		{
			Name = name;
		}

		private string Name { get; }

		public static bool TryGetName<T>(out string name)
		{
			return TryGetName(typeof(T), out name);
		}

		public static bool TryGetName(Type t, out string name)
		{
			name = "";

			if (!(t.GetCustomAttributes(typeof(WindowAttribute), true)
				    .FirstOrDefault() is WindowAttribute attribute))
				return false;

			name = attribute.Name;
			return true;
		}
	}
}