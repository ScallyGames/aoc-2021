using System.Diagnostics;

string[] lines = File.ReadAllLines("input.txt");

Stopwatch sw = new Stopwatch();
sw.Start();

IEnumerable<KeyValuePair<(Vector3 a, Vector3 b), Vector3>> GetDifferences(IEnumerable<Vector3> beacons, IEnumerable<Vector3> reference)
{
    return beacons
            .SelectMany(x => reference, (a, b) => (a, b))
            .Select(x => new KeyValuePair<(Vector3 a, Vector3 b), Vector3>(x, VectorMath.Difference(x.b, x.a)));
}

var possibleOrientations = new Vector3[]
{
    new Vector3(1, 0, 0),
    new Vector3(0, 1, 0),
    new Vector3(0, 0, 1),
    new Vector3(-1, 0, 0),
    new Vector3(0, -1, 0),
    new Vector3(0, 0, -1),
};

var transformations = new List<int[,]>();

foreach(var forward in possibleOrientations)
{
    foreach(var up in possibleOrientations.Where(x => VectorMath.Dot(forward, x) == 0))
    {
        var right = VectorMath.Cross(up, forward);

        transformations.Add(new int[,] {
            { right.X, right.Y, right.Z },
            { up.X, up.Y, up.Z },
            { forward.X, forward.Y, forward.Z },
        });
    }
}

var scannerBeacons = 
    lines
        .Where(x => x.Contains("---"))
        .ToDictionary(
            scannerStart => scannerStart, 
            scannerStart => {
                return lines
                    .SkipWhile(x => x != scannerStart)
                    .Skip(1)
                    .TakeWhile(x => x != "")
                    .Select(
                        x => x
                            .Split(',')
                            .Select(n => int.Parse(n))
                            .ToArray()
                    )
                    .Select(n => new Vector3() { X = n[0], Y = n[1], Z = n[2] });
            }
        );


HashSet<Vector3> absoluteBeaconPositions = new HashSet<Vector3>(scannerBeacons.First().Value);


var remainingScanners = new Queue<KeyValuePair<string, IEnumerable<Vector3>>>(scannerBeacons.Skip(1));

while(remainingScanners.Any())
{
    var nextScannerForAlignment = remainingScanners.Dequeue();

    //  Console.WriteLine("Trying next scanner");
    // Console.WriteLine(nextScannerForAlignment.Key);

    var didMatch = false;

    var beacons = nextScannerForAlignment.Value;

    foreach (var transformation in transformations)
    {
        var transformedBeacons = beacons.Select(x => VectorMath.Multiply(x, transformation));

        var differencePatterns = GetDifferences(transformedBeacons, absoluteBeaconPositions);

        var offset = differencePatterns
            .GroupBy(x => x.Value)
            .MaxBy(x => x.Count());

        int overlaps = offset!.Count();

        if (overlaps >= 12)
        {
            didMatch = true;

            var beaconsInAbsoluteSpace = transformedBeacons.Select(x => VectorMath.Addition(x, offset.Key));

            foreach(var beaconInAbsoluteSpace in beaconsInAbsoluteSpace)
            {
                absoluteBeaconPositions.Add(beaconInAbsoluteSpace);
            }

            break;
        }
    }

    if (!didMatch)
    {
        remainingScanners.Enqueue(nextScannerForAlignment);
    }
}


Console.WriteLine(absoluteBeaconPositions.Count);

sw.Stop();

Console.WriteLine("Took " + sw.Elapsed + " seconds");