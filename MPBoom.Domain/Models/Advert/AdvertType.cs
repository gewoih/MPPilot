using System.ComponentModel;

namespace MPBoom.Domain.Models.Advert
{
    public enum AdvertType
    {
        [Description("Карточка товара")]
        ProductPage = 5,

        [Description("Поиск")]
        Search = 6
    }
}
