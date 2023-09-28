using Core.ChangeInfo;

namespace TelegramCore;

public class TelegramUser
{
    public int TelegramUserPK { get; init; }
    /// <summary>
    /// Айди телеграм юзера.
    /// </summary>
    public long ChatId { get; init; }
    /// <summary>
    /// Список курсов, о заменах которых, пользователь будет получать уведолмения.
    /// </summary>
    public List<Group>? SubcribedGroups { get; init; }
}
