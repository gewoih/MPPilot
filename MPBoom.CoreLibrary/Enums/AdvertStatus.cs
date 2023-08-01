using System.ComponentModel;

namespace MPBoom.Core.Enums
{
	public enum AdvertStatus
	{
		[Description("Готова к запуску")]
		Ready = 4,

		[Description("Завершена")]
		Finished = 7,

		[Description("Запущена")]
		InProgress = 9,

		[Description("Приостановлена")]
		Stopped = 11
	}
}
