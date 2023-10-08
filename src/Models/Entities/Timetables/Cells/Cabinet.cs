namespace Models.Entities.Timetables.Cells;

public class Cabinet
{
    public int CabinetId { get; init; }
    public string? Address { get; set; }
    public string? Number { get; set; }
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

    public override string ToString()
    {
        return $"Id: {CabinetId}, Number: {Number}, Address: {Address}";
    }
}