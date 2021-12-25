string[] lines = File.ReadAllLines("input.txt");

int[,] heightmap = new int[lines.Length, lines[0].Length];


for(int y = 0; y < heightmap.GetLength(0); y++)
{
    for(int x = 0; x < heightmap.GetLength(1); x++)
    {
        heightmap[y, x] = int.Parse(lines[y][x] + "");
    }
}

bool[,] kernel = new [,] {
    { false,  true, false},
    {  true, false,  true},
    { false,  true, false},
};

int totalDanger = 0;

for(int y = 0; y < heightmap.GetLength(0); y++)
{
    for(int x = 0; x < heightmap.GetLength(1); x++)
    {
        if(isMinimum(x, y, heightmap, kernel))
        {
            totalDanger += heightmap[y, x] + 1;
        }
    }
}

Console.WriteLine(totalDanger);

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