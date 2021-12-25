using System.Text.RegularExpressions;

string[] lines = File.ReadAllLines("input.txt");

Cuboid bounds = new Cuboid(-50, -50, -50, 50, 50, 50);

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
    })
    .Where(x => x.cuboid.IsWithinBounds(bounds))
    ;

// var maxNumberOfOverlaps = parsed
//     .SelectMany(x => parsed, (a, b) => (a, b))
//     .Where(x => x.a != x.b && x.a.cuboid.IsOverlapping(x.b.cuboid))
//     .GroupBy(x => x.a)
//     .Max(x => x.Count());

// Console.WriteLine(maxNumberOfOverlaps);

HashSet<Cuboid> activeCuboids = new HashSet<Cuboid>();

foreach(var cuboidEntry in parsed.Take(20))
{
    Console.WriteLine("New entry " + cuboidEntry);
    if(cuboidEntry.isOn)
    {
        // cut out existing on regions
        List<Cuboid> elementsToAdd = new List<Cuboid>() { cuboidEntry.cuboid };
        foreach(var activeCuboid in activeCuboids)
        {
            bool didCut = false;
            List<Cuboid> elementsToAddAfterCut = new List<Cuboid>();
            foreach(var elementToAdd in elementsToAdd)
            {
                if(activeCuboid.IsOverlapping(elementToAdd))
                {
                    elementsToAddAfterCut.AddRange(elementToAdd.Subtract(activeCuboid));
                    didCut = true;
                }
            }
            if(didCut) elementsToAdd = elementsToAddAfterCut;
        }

        // add them to active cuboids
        foreach(var elementToAdd in elementsToAdd)
        {
            Console.WriteLine("Got remaining element " + elementToAdd);
            activeCuboids.Add(elementToAdd);
        }
    }
    else
    {
        // Cut shape out of active cuboids
        List<Cuboid> elementsToRemove = new List<Cuboid>();
        List<Cuboid> elementsToAdd = new List<Cuboid>();
        foreach(var activeCuboid in activeCuboids)
        {
            if(activeCuboid.IsOverlapping(cuboidEntry.cuboid))
            {
                elementsToAdd.AddRange(activeCuboid.Subtract(cuboidEntry.cuboid));
                elementsToRemove.Add(activeCuboid);
            }
        }
        foreach(var elementToRemove in elementsToRemove)
        {
            activeCuboids.Remove(elementToRemove);
        }
        foreach(var elementToAdd in elementsToAdd)
        {
            activeCuboids.Add(elementToAdd);
        }
    }
    Console.WriteLine();
}

ulong onCubes = activeCuboids.Aggregate(0UL, (a, b) => a + b.Size);

Console.WriteLine(onCubes);