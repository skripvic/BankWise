namespace Core;

public class CassetteModel
{
    public int DenominationValue { get; private set; }
    
    public int CurrentCount { get; set; }
    
    public int? Capacity { get; private set; }

    public CassetteModel(int denominationValue, int currentCount, int capacity)
    {
        DenominationValue = denominationValue;
        CurrentCount = currentCount;
        Capacity = capacity;
    }

    public CassetteModel(int denominationValue, int currentCount)
    {
        DenominationValue = denominationValue;
        CurrentCount = currentCount;
    }
}