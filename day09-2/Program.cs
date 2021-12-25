string[] lines = File.ReadAllLines("input.txt");

int[,] heightmap = new int[lines.Length, lines[0].Length];

if(Console.BufferHeight < lines.Length)
{
    Console.BufferHeight = lines.Length;
}
if(Console.BufferWidth < lines[0].Length)
{
    Console.BufferWidth = lines[0].Length;
}

for(int y = 0; y < heightmap.GetLength(0); y++)
{
    for(int x = 0; x < heightmap.GetLength(1); x++)
    {
        heightmap[y, x] = int.Parse(lines[y][x] + "");
    }
}

// print(heightmap);

bool[,] kernel = new [,] {
    { false,  true, false},
    {  true, false,  true},
    { false,  true, false},
};


List<(int x, int y)> lowPoints = new List<(int x, int y)>();

for(int y = 0; y < heightmap.GetLength(0); y++)
{
    for(int x = 0; x < heightmap.GetLength(1); x++)
    {
        if(isMinimum(x, y, heightmap, kernel))
        {
            lowPoints.Add((x, y));
        }
    }
}

print(heightmap);

int groupIndex = -1;

Dictionary<int, int> basins = new Dictionary<int, int>();

foreach(var lowPoint in lowPoints)
{
    basins.Add(groupIndex, 0);

    Queue<(int x, int y)> openList = new Queue<(int x, int y)>();

    openList.Enqueue(lowPoint);

    while(openList.Count > 0)
    {
        var point = openList.Dequeue();

        if(!isInBounds(point.y, heightmap.GetLength(0))) continue;
        if(!isInBounds(point.x, heightmap.GetLength(1))) continue;

        if(heightmap[point.y, point.x] < 0) continue;

        if(heightmap[point.y, point.x] == 9) continue;

        heightmap[point.y, point.x] = groupIndex;
        basins[groupIndex]++;

        openList.Enqueue((point.x - 1, point.y));
        openList.Enqueue((point.x + 1, point.y));
        openList.Enqueue((point.x, point.y - 1));
        openList.Enqueue((point.x, point.y + 1));

        printPixel(heightmap, point.x, point.y);
        Thread.Sleep(20);
    }

    groupIndex--;
}

Console.WriteLine(basins.Values.OrderByDescending(x => x).Take(3).Aggregate(1L, (a, b) => a * b));


bool isMinimum(int x, int y, int[,] map, bool[,] kernel)
{
    for(int kernelY = 0; kernelY < kernel.GetLength(0); kernelY++)
    {
        int sampleY = y + kernelY - (kernel.GetLength(0) / 2);

        if(!isInBounds(sampleY, map.GetLength(0))) continue;

        for(int kernelX = 0; kernelX < kernel.GetLength(1); kernelX++)
        {
            if(!kernel[kernelY, kernelX]) continue;

            int sampleX = x + kernelX - (kernel.GetLength(1) / 2);

            if(!isInBounds(sampleX, map.GetLength(1))) continue;

            if(map[sampleY, sampleX] <= map[y, x]) 
            {
                return false;
            }
        }
    }

    return true;
}

bool isInBounds(int coordinate, int bound) => coordinate >= 0 && coordinate < bound;

void print(int[,] map)
{
    Console.SetCursorPosition(0, 0);
    for(int y = 0; y < map.GetLength(0); y++)
    {
        for(int x = 0; x < map.GetLength(1); x++)
        {
            if(map[y, x] >= 0)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write(" ");
                // Console.Write(map[y, x]);
            }
            else
            {
                Console.BackgroundColor = (ConsoleColor)(1 + (map[y, x] * -1) % 13);
                Console.Write(" ");
                // Console.Write((char)('a' - 1 + (map[y, x] * -1)));   
            }
        }
        Console.WriteLine();
    }
    Console.WriteLine();
    Console.ResetColor();
}

void printPixel(int[,] map, int x, int y)
{
    Console.SetCursorPosition(x, y);
    if(map[y, x] >= 0)
    {
        Console.BackgroundColor = ConsoleColor.Black;
        Console.Write(" ");
    }
    else
    {
        Console.BackgroundColor = (ConsoleColor)(1 + (map[y, x] * -1) % 13);
        Console.Write(" ");
    }
}