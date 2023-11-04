using System.Diagnostics.CodeAnalysis;
using Models.Entities.Timetables.Cells;

namespace Models.Entities.Timetables.Cells.CellMembers;

public class Cabinet
{
    public int CabinetId { get; init; }
    public required string Address { get; set; }
    public required string Number { get; set; }

    public List<TimetableCell>? TimetableCells { get; set; }

    private Cabinet() { }

    [SetsRequiredMembers]
    public Cabinet(int id, string address, string number)
    {
        id.Throw().IfDefault();
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