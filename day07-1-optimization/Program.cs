int[] positions = File.ReadAllLines("input.txt")[0].Split(',').Select(x => int.Parse(x)).ToArray();

Console.WriteLine(positions.Max());


// int gatheringMin = 0;
// int gatheringMax = positions.Max();

// while(gatheringMax - gatheringMin > 0)
// {
//     Console.WriteLine($"Interval ({gatheringMin}, {gatheringMax})");

//     int gatheringCandidate = (gatheringMax + gatheringMin) / 2;
    
//     int gatheringCandidateLeft = (gatheringMin + gatheringCandidate) / 2;
//     int gatheringCandidateRight = (gatheringMax + gatheringCandidate) / 2;

//     int costLeft = GetTotalCost(positions, gatheringCandidateLeft);
//     int costRight = GetTotalCost(positions, gatheringCandidateRight);

//     if(costLeft <= costRight)
//     {
//         gatheringMax = gatheringCandidate;
//     }
//     else
//     {
//         gatheringMin = gatheringCandidate;
//     }
// }

// Console.WriteLine(GetTotalCost(positions, gatheringMin));


int[] sortedPositions = positions.OrderBy(x => x).ToArray();

int median = sortedPositions[sortedPositions.Length / 2];

Console.WriteLine(GetTotalCost(positions, median));

int GetTotalCost(int[] positions, int gatheringPosition)
{
    return positions.Sum(x => CostFunction(Math.Abs(x - gatheringPosition)));
}

int CostFunction(int distance)
{
    return distance;
}

// O(log(max(X)) * n)