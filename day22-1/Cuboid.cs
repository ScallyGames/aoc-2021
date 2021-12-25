public record struct Cuboid(int MinX, int MinY, int MinZ, int MaxX, int MaxY, int MaxZ)
{
    public bool IsWithinBounds(Cuboid bounds)
    {
        return this.MinX >= bounds.MinX &&
            this.MinY >= bounds.MinY &&
            this.MinZ >= bounds.MinZ && 
            this.MaxX <= bounds.MaxX &&
            this.MaxY <= bounds.MaxY &&
            this.MaxZ <= bounds.MaxZ;
    }
}