using System.ComponentModel;
using System.Reflection;

namespace MPBoom.Domain.Utils
{
	public static class EnumUtils
	{
		public static string GetDescription(this Enum e)
		{
			var attribute = e.GetType()
								.GetTypeInfo()
								.GetMember(e.ToString())
								.FirstOrDefault(member => member.MemberType == MemberTypes.Field)
								.GetCustomAttributes(typeof(DescriptionAttribute), false)
								.SingleOrDefault()
								as DescriptionAttribute;

			return attribute?.Description ?? e.ToString();
		}
	}
}
