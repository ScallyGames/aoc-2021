int[] positions = File.ReadAllLines("input.txt")[0].Split(',').Select(x => int.Parse(x)).ToArray();

Console.WriteLine(positions.Max());


int gatheringMin = 0;
int gatheringMax = positions.Max();

while(gatheringMax - gatheringMin > 0)
{
    Console.WriteLine($"Interval ({gatheringMin}, {gatheringMax})");

    if(gatheringMax - gatheringMin == 1)
    {

        int costMin = GetTotalCost(positions, gatheringMin);
        int costMax = GetTotalCost(positions, gatheringMax);

        if(costMin < costMax)
        {
            gatheringMax = gatheringMin;
        }
        else
        {
            gatheringMin = gatheringMax;
        }
    }
    else
    {
        int gatheringCandidate = (gatheringMax + gatheringMin) / 2;
        
        int gatheringCandidateLeft = gatheringCandidate - 1;
        int gatheringCandidateRight = gatheringCandidate + 1;

        int costLeft = GetTotalCost(positions, gatheringCandidateLeft);
        int costRight = GetTotalCost(positions, gatheringCandidateRight);

        if(costLeft <= costRight)
        {
            gatheringMax = gatheringCandidate;
        }
        else
        {
            gatheringMin = gatheringCandidate;
        }
    }
}

// O(log(max(X)) * n)

Console.WriteLine(GetTotalCost(positions, gatheringMin));


int GetTotalCost(int[] positions, int gatheringPosition)
{
    return positions.Sum(x => CostFunction(Math.Abs(x - gatheringPosition)));
}

int CostFunction(int distance)
{
    return distance * (distance + 1) / 2;
}