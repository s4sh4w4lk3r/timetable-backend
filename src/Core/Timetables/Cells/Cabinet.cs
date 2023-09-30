namespace Core.Timetables.Cells;

public class Cabinet
{
    public int CabinetPK { get; init; }
    public string? Address { get; init; }
    public string? Number { get; init; }

    public Cabinet(int cabinetPK, string address, string number)
    {
        address.ThrowIfNull().IfWhiteSpace();
        number.ThrowIfNull().IfWhiteSpace();
        CabinetPK = cabinetPK;
        Address = address;
        Number = number;
    }
}