using System.Text.RegularExpressions;

string[] lines = File.ReadAllLines("input.txt");

Cuboid bounds = new Cuboid(-50, -50, -50, 50, 50, 50);

bool[,,] reactor = new bool[101, 101, 101];

var parsed = lines
    .Select(x =>
    {
        var match = Regex.Match(x, @"(on|off) x=(?'minX'-?\d+)\.{2}(?'maxX'-?\d+),y=(?'minY'-?\d+)\.{2}(?'maxY'-?\d+),z=(?'minZ'-?\d+)\.{2}(?'maxZ'-?\d+)");
        
        bool isOn = match.Groups[1].Value == "on";
        Cuboid cuboid = new Cuboid(
            int.Parse(match.Groups["minX"].Value),
            int.Parse(match.Groups["minY"].Value),
            int.Parse(match.Groups["minZ"].Value),
            int.Parse(match.Groups["maxX"].Value),
            int.Parse(match.Groups["maxY"].Value),
            int.Parse(match.Groups["maxZ"].Value)
        );

        return (isOn, cuboid);
    });


foreach(var cuboidEntry in parsed.Where(x => x.cuboid.IsWithinBounds(bounds)))
{
    for(int z = cuboidEntry.cuboid.MinZ; z <= cuboidEntry.cuboid.MaxZ; z++)
    {   
        int zIndex = z - bounds.MinZ;
        for(int y = cuboidEntry.cuboid.MinY; y <= cuboidEntry.cuboid.MaxY; y++)
        {
            int yIndex = y - bounds.MinY;
            for(int x = cuboidEntry.cuboid.MinX; x <= cuboidEntry.cuboid.MaxX; x++)
            {
                int xIndex = x - bounds.MinX;

                reactor[zIndex, yIndex, xIndex] = cuboidEntry.isOn;
            }
        }
    }
}

var numberOfCoresOn = reactor.Cast<bool>().Count(x => x == true);

Console.WriteLine(numberOfCoresOn);