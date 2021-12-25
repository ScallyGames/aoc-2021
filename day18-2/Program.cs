string[] inputLines = File.ReadAllLines("input.txt");

var parsedFishNumbers = inputLines.Select(x => new FishNumber(x)).ToArray();

var highestMagnitude = parsedFishNumbers
    .SelectMany(a => parsedFishNumbers, (a, b) => (new FishNumber(a.ToString()), new FishNumber(b.ToString())))
    .Where(x => x.Item1.ToString() != x.Item2.ToString())
    .Select(x => 
    {
        var additionResult = FishNumberMath.Add(x.Item1, x.Item2);
        return (x.Item1, x.Item2, additionResult, additionResult.GetMagnitude());
    })
    .MaxBy(x => x.Item4);

Console.WriteLine($"{highestMagnitude.Item1} + {highestMagnitude.Item2} = {highestMagnitude.additionResult} with magnitude of {highestMagnitude.Item4}");

// Console.WriteLine(highestMagnitude);