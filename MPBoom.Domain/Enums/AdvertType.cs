using System.ComponentModel;

namespace MPBoom.Domain.Enums
{
	public enum AdvertType
	{
		[Description("Карточка товара")]
		ProductPage = 5,

		[Description("Поиск")]
		Search = 6
	}
}
