using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Models.Entities.Timetables.Cells.CellMembers;

public class Cabinet
{
    public int CabinetId { get; init; }
    public required string Address { get; set; }
    public required string Number { get; set; }


    [JsonIgnore]
    public ICollection<ActualTimetableCell>? ActualTimetableCells { get; set; }

    [JsonIgnore]
    public ICollection<StableTimetableCell>? StableTimetableCells { get; set; }

    private Cabinet() { }

    [SetsRequiredMembers]
    public Cabinet(int id, string address, string number)
    {
        address.ThrowIfNull().IfWhiteSpace();
        number.ThrowIfNull().IfWhiteSpace();

        CabinetId = id;
        Address = address;
        Number = number;
    }

    public override string ToString()
    {
        return $"Id: {CabinetId}, Number: {Number}, Address: {Address}";
    }
}