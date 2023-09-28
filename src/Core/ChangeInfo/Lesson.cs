namespace Core.ChangeInfo;

public class Lesson
{
    /// <summary>
    /// Номер пары/урока, который должен был быть.
    /// </summary>
    public int From { get; init; }

    /// <summary>
    /// Номер урока/пары, на который было перенесено занятие.
    /// </summary>
    public int To { get; init; }

    /// <summary>
    /// Отменен ли урок/пара.
    /// </summary>
    public bool IsCanceled { get; init; }

    /// <summary>
    /// Перенесена ли пара/урок на другое время.
    /// </summary>
    public bool IsMoved { get; init; }

}