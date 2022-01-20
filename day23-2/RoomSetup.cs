using System.Text;

public record struct RoomSetup(char[,] Value) : IEquatable<RoomSetup>
{
    private string toStringMemoized = "";

    public override string ToString()
    {
        if(toStringMemoized == "")
        {
            StringBuilder sb = new StringBuilder();
            for(int y = 0; y < Value.GetLength(0); y++)
            {
                for(int x = 0; x < Value.GetLength(1); x++)
                {
                    sb.Append(Value[y, x]);
                }
                sb.AppendLine();
            }
            toStringMemoized = sb.ToString();
        }
        return toStringMemoized;
    }

    public bool Equals(RoomSetup other)
    {
        return this.ToString() == other.ToString();
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