
namespace WebApi.Types
{
    public static class ResponseMessage
    {
        /// <summary>
        /// Формирует строку с  сообщением о том, что указанное поле == default.
        /// </summary>
        /// <param name="propertyName">Свойство, имя которого надо высветить в сообщении.</param>
        /// <returns></returns>
        public static string GetMessageIfDefaultValue(string propertyName) => $"Указанный {propertyName} не должен быть равен нулю";


        /// <summary>
        /// Формирует строку с  сообщением о том, что указанное поле не найдено в бд.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static string GetMessageIfNotFoundInDb(string propertyName) => $"Запрашиваемый {propertyName} не найден в базе данных.";
    }
}
