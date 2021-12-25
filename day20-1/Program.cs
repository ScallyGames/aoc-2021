string[] lines = File.ReadAllLines("input.txt");

string lookupTable = lines[0];

string[] imageData = lines.Skip(2).ToArray();

bool isStoringLitPoints = true;

HashSet<(int x, int y)> activePoints = new HashSet<(int x, int y)>();

for(int y = 0; y < imageData.Length; y++)
{
    for(int x = 0; x < imageData[y].Length; x++)
    {
        if(imageData[y][x] == '#')
        {
            activePoints.Add((x, y));
        }
    }
}

const int numberOfIterations = 2;

for(int i = 0; i < numberOfIterations; i++)
{
    activePoints = GetIterationResult(activePoints);
}

Console.WriteLine(activePoints.Count());


HashSet<(int x, int y)> GetIterationResult(HashSet<(int x, int y)> previousState)
{
    Console.WriteLine("Doing iteration");

    HashSet<(int x, int y)> activePoints = new HashSet<(int x, int y)>();

    var minX = previousState.Min(x => x.x);
    var minY = previousState.Min(x => x.y);
    var maxX = previousState.Max(x => x.x);
    var maxY = previousState.Max(x => x.y);

    Console.WriteLine(string.Join('=', new string[maxX - minX + 4 + 2]));

    for(int y = minY - 1; y <= maxY + 1; y++)
    {
        Console.Write('|');
        for(int x = minX - 1; x <= maxX + 1; x++)
        {
            if(previousState.Contains((x, y)))
            {
                Console.BackgroundColor = isStoringLitPoints ? ConsoleColor.Yellow : ConsoleColor.Black;
            }
            else
            {
                Console.BackgroundColor = isStoringLitPoints ? ConsoleColor.Black : ConsoleColor.Yellow;
            }
            if(IsPaddedWithTrue(x, y, (minX, minY, maxX, maxY)))
            {
                Console.BackgroundColor = ConsoleColor.Yellow;
            }
            Console.Write(' ');

            if(GetKernelResult(x, y, previousState, (minX, minY, maxX, maxY)) != isStoringLitPoints)
            {
                activePoints.Add((x, y));
            }
        }
        Console.ResetColor();
        Console.WriteLine('|');
    }
    
    Console.WriteLine(string.Join('=', new string[maxX - minX + 4 + 2]));

    isStoringLitPoints = !isStoringLitPoints;

    return activePoints;
}

bool GetKernelResult(int x, int y, HashSet<(int x, int y)> previousState, (int minX, int minY, int maxX, int maxY) bounds)
{
    int lookupIndex = 0;

    for(int offsetY = 0; offsetY < 3; offsetY++)
    {
        int sampleY = y + offsetY - 1;
        for(int offsetX = 0; offsetX < 3; offsetX++)
        {
            int sampleX = x + offsetX - 1;
            
            if(previousState.Contains((sampleX, sampleY)))
            {
                lookupIndex |= 1 << (8 - ((offsetY * 3) + offsetX));
            }
        }
    }
    
    if(!isStoringLitPoints)
    {
        lookupIndex = ~lookupIndex;
        lookupIndex = lookupIndex & (512 - 1); // limit to 9 bits
    }

    return lookupTable[lookupIndex] == '#';
}

bool IsPaddedWithTrue(int x, int y, (int minX, int minY, int maxX, int maxY) bounds)
{
    return !isStoringLitPoints && 
        (
            x < bounds.minX ||
            x > bounds.maxX ||
            y < bounds.minY ||
            y > bounds.maxY
        );
}