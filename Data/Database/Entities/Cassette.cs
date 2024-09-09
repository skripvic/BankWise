namespace Data.Database;

public class Cassette
{
    public int Id { get; set; }

    public virtual Denomination Denomination { get; set; }
    
    public int Capacity { get; set; }
    
    public int CurrentCount { get; set; }

    public Cassette() { }
}