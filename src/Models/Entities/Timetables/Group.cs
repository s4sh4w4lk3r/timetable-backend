using Models.Entities.Identity;
using Models.Entities.Identity.Users;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Models.Entities.Timetables;

public class Group
{
    public int GroupId { get; init; }
    public required string Name { get; set; }
    public string? AscId { get; set; }

    [JsonIgnore]
    public ICollection<Student>? Students { get; set; }

    [JsonIgnore]
    public ICollection<RegistrationEntity>? RegistrationEntities { get; set; }
    private Group() { }

    [SetsRequiredMembers]
    public Group(int groupPk, string name)
    {
        name.ThrowIfNull().IfWhiteSpace();

        GroupId = groupPk;
        Name = name;
    }

    public override string ToString()
    {
        return $"Id: {GroupId}, Name: {Name}";
    }
}
