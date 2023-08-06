using System.ComponentModel;

namespace MPPilot.Domain.Models.Advert
{
    public enum AdvertType
    {
        [Description("Карточка товара")]
        ProductPage = 5,

        [Description("Поиск")]
        Search = 6
    }
}
