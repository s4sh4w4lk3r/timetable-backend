namespace Core.ChangeInfo;

public class ChangeEntity
{
    public int ChangePK { get; init; }

    /// <summary>
    /// Номер группы.
    /// </summary>
    public Group? Group { get; init; }

    /// <summary>
    /// Учитель, которого заменяют.
    /// </summary>
    public Teacher? MissingTeacher { get; init; }

    /// <summary>
    /// Учитель, который заменяет.
    /// </summary>
    public Teacher? SubstituteTeacher { get; init; }

    /// <summary>
    /// Предмет/дисциплина.
    /// </summary>
    public Subject? Subject { get; init; }
    public Cabinet? Cabinet { get; init; }
    public Lesson? Lesson { get; init; }
}
