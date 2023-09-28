namespace Core;

public abstract class AbstractPerson
{
    public int PersonPK { get; init; }
    public string? Surname { get; init; }
    public string? Forename { get; init; }
    public string? Patronymic { get; init; }
}
