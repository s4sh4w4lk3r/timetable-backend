namespace Models.Entities.Timetables.Cells;

public class Cabinet
{
    public int CabinetId { get; init; }
    public string? Address { get; init; }
    public string? Number { get; init; }
    public List<TimetableCell>? TimetableCells { get; set; }

    private Cabinet() { }
    public Cabinet(int cabinetPK, string address, string number)
    {
        address.ThrowIfNull().IfWhiteSpace();
        number.ThrowIfNull().IfWhiteSpace();
        CabinetId = cabinetPK;
        Address = address;
        Number = number;
    }
}