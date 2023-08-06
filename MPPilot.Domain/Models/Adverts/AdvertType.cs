using System.ComponentModel;

namespace MPPilot.Domain.Models.Adverts
{
    public enum AdvertType
    {
        [Description("Карточка товара")]
        ProductPage = 5,

        [Description("Поиск")]
        Search = 6
    }
}
