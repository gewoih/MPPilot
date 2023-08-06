namespace MPPilot.Domain.Models.Adverts
{
    public class AdvertUpdateResult
    {
        public bool IsStatusChanged = true;
        public bool IsNameChanged = true;
        public bool IsKeywordChanged = true;
        public bool IsUpdateFailed => !IsStatusChanged || !IsNameChanged || !IsKeywordChanged;

        public string GetNonUpdatedFieldsString()
        {
            var result = string.Empty;

            if (!IsStatusChanged)
                result += "статус";
            if (!IsNameChanged)
                result += ", название";
            if (!IsKeywordChanged)
                result += ", ключевая фраза";

            return result;
        }
    }
}
