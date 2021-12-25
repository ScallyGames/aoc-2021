string[] lines = File.ReadAllLines("input.txt");

int[,] octopuses = new int[lines.Length, lines[0].Length];


#if NET6_0_WINDOWS_OR_GREATER
if(Console.BufferHeight < lines.Length)
{
    Console.BufferHeight = lines.Length;
}
if(Console.BufferWidth < lines[0].Length)
{
    Console.BufferWidth = lines[0].Length;
}
#endif
Console.OutputEncoding = System.Text.Encoding.GetEncoding(1200);

// fill initial values
for(int y = 0; y < octopuses.GetLength(0); y++)
{
    for(int x = 0; x < octopuses.GetLength(1); x++)
    {
        octopuses[y, x] = int.Parse(lines[y][x] + "");
    }
}


bool[,] kernel = new [,] {
    { true,  true, true},
    { true, false, true},
    { true,  true, true},
};


int totalNumberOfFlashes = 0;


HashSet<(int x, int y)> flashes = new HashSet<(int x, int y)>();
Queue<(int x, int y)> remainingPoints = new Queue<(int x, int y)>();

int currentIteration = 0;
while(true)
{
    currentIteration++;
    doIteration(out bool isSynchronized);

    print(octopuses);
    Thread.Sleep(40);
    
    if(isSynchronized)
    {
        Console.WriteLine(currentIteration);
        break;
    }
}

void doIteration(out bool isSynchronized)
{
    for(int y = 0; y < octopuses.GetLength(0); y++)
    {
        for(int x = 0; x < octopuses.GetLength(1); x++)
        {
            octopuses[y, x]++;
        }
    }

    for(int y = 0; y < octopuses.GetLength(0); y++)
    {
        for(int x = 0; x < octopuses.GetLength(1); x++)
        {
            flashInKernel(x, y, octopuses, kernel);
        }
    }

    // print(octopuses);
    while(remainingPoints.Any())
    {
        var point = remainingPoints.Dequeue();
        flashInKernel(point.x, point.y, octopuses, kernel);
        // print(octopuses);
    }

    totalNumberOfFlashes += flashes.Count;

    isSynchronized = flashes.Count == lines.Length * lines[0].Length;

    flashes.Clear();
    remainingPoints.Clear();
}


void flashInKernel(int x, int y, int[,] map, bool[,] kernel)
{
    if(map[y, x] <= 9) return;
    if(flashes.Contains((x, y))) return;

    flashes.Add((x, y));
    map[y, x] = 0;

    for(int kernelY = 0; kernelY < kernel.GetLength(0); kernelY++)
    {
        int sampleY = y + kernelY - (kernel.GetLength(0) / 2);

        if(!isInBounds(sampleY, map.GetLength(0))) continue;

        for(int kernelX = 0; kernelX < kernel.GetLength(1); kernelX++)
        {
            if(!kernel[kernelY, kernelX]) continue;

            int sampleX = x + kernelX - (kernel.GetLength(1) / 2);

            if(!isInBounds(sampleX, map.GetLength(1))) continue;

            // flash
            if(!flashes.Contains((sampleX, sampleY)))
            {
                map[sampleY, sampleX]++;
                remainingPoints.Enqueue((sampleX, sampleY));
            }
        }
    }
}

bool isInBounds(int coordinate, int bound) => coordinate >= 0 && coordinate < bound;



void print(int[,] map)
{
    Console.SetCursorPosition(0, 0);
    for(int y = 0; y < map.GetLength(0); y++)
    {
        for(int x = 0; x < map.GetLength(1); x++)
        {
            if(map[y, x] != 0)
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write(" ");
                // Console.Write(map[y, x]);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("•");
                // Console.Write(map[y, x]);
            }
        }
        Console.WriteLine();
    }
    Console.WriteLine();
    Console.ResetColor();
}

// void printPixel(int[,] map, int x, int y)
// {
//     Console.SetCursorPosition(x, y);
//     if(map[y, x] <= 9)
//     {
//         // Console.BackgroundColor = ConsoleColor.Black;
//         // Console.Write(" ");
//         Console.Write(map[y, x]);
//     }
//     else
//     {
//         // Console.BackgroundColor = (ConsoleColor)(1 + (map[y, x] * -1) % 13);
//         // Console.Write(" ");
//         Console.Write(map[y, x]);
//     }
// }