record struct RoomSetup(char[,] Value) : IEquatable<RoomSetup>
{
    public bool Equals(RoomSetup other)
    {
        return Enumerable.SequenceEqual(Value.Cast<char>(), other.Value.Cast<char>());
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int HashCode = 1;
            for(int y = 0; y < Value.GetLength(0); y++)
            {
                for(int x = 0; x < Value.GetLength(1); x++)
                {
                    HashCode *= 397;
                    HashCode ^= Value[y, x].GetHashCode();
                }
            }
            return HashCode;
        }
    }
};