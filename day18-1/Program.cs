using System.Diagnostics;

string[] inputLines = File.ReadAllLines("input.txt");

var parsedFishNumbers = inputLines.Select(x => new FishNumber(x)).ToArray();

var resultingFishNumber = parsedFishNumbers.Aggregate((a, b) => FishNumberMath.Add(a, b));

Console.WriteLine(resultingFishNumber);
Console.WriteLine(resultingFishNumber.GetMagnitude());

