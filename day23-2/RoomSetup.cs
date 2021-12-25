public record struct RoomSetup(char[,] Value, int correctBonus = 10000) : IEquatable<RoomSetup>
{
    private int heuristicMemoization = -1;
    private bool isHeuristicMemoized = false;

    public int Heuristic
    {
        get
        {
            if(isHeuristicMemoized) return heuristicMemoization;

            int evaluation = 0;

            for(int y = 0; y < Value.GetLength(0); y++)
            {
                for(int x = 0; x < Value.GetLength(1); x++)
                {
                    if(!"ABCD".Contains(InputData.TargetSetup.Value[y, x])) continue;

                    if(Value[y, x] == InputData.TargetSetup.Value[y, x])
                    {
                        // is at right spot

                        bool isNotCovering = true;
                        var that = this;
                        for(int stackPosition = y; stackPosition < Value.GetLength(0); stackPosition++)
                        {
                            if("ABCD".Where(n => n != that.Value[y, x]).Contains(that.Value[stackPosition, x]))
                            {
                                isNotCovering = false;
                                break;
                            }
                        }
                        if(isNotCovering)
                        {
                            evaluation -= correctBonus;
                        }
                    } else if(Value[y, x] != '.')
                    {
                        evaluation += 1000;
                    }
                }
            }
            heuristicMemoization = evaluation;
            isHeuristicMemoized = true;
            return evaluation;
        }
    }



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