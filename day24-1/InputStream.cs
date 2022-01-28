public class InputStream
{
    private string inputDigits;
    
    public int CurrentIndex { get; private set; } 

    public InputStream(string inputDigits)
    {
        this.inputDigits = inputDigits;
    }

    public InputStream Clone()
    {
        var clone = new InputStream(this.inputDigits);
        clone.CurrentIndex = this.CurrentIndex;
        return clone;
    }
    
    public int Peek()
    {
        return inputDigits[CurrentIndex];
    }

    public int Next()
    {
        int current = inputDigits[CurrentIndex];
        CurrentIndex++;
        return current;
    }
}