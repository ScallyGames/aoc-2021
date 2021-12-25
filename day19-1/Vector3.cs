public record Vector3
{
    public int X;
    public int Y;
    public int Z;

    public Vector3() { }
    
    public Vector3(int x, int y, int z)
    {
        this.X = x;
        this.Y = y;
        this.Z = z;
    }
}