using System.Text;
using System.Text.RegularExpressions;

string[] lines = File.ReadAllLines("input.txt");

var initialPolymer = lines.First();

var instructions = lines
    .Skip(2)
    .Select(x => Regex.Match(x, @"([A-Z])([A-Z]) -> ([A-Z])"))
    .ToDictionary(
        x => (x.Groups[1].Value[0], x.Groups[2].Value[0]),
        x => x.Groups[3].Value[0]
    );

int steps = 40;

var polymerPairs = instructions.ToDictionary(x => x.Key, x => (ulong)0);


for(int charIndex = 0; charIndex < initialPolymer.Length - 1; charIndex++)
{
    polymerPairs[(initialPolymer[charIndex], initialPolymer[charIndex+1])]++;
}

for(int i = 0; i < steps; i++)
{
    var newPolymerPairs = instructions.ToDictionary(x => x.Key, x => (ulong)0);

    foreach(var kv in polymerPairs)
    {
        char newChar = instructions[kv.Key];
        newPolymerPairs[(kv.Key.Item1, newChar)] += kv.Value;
        newPolymerPairs[(newChar, kv.Key.Item2)] += kv.Value;
    }

    polymerPairs = newPolymerPairs;
}

var charCounts = new Dictionary<char, ulong>();

foreach(var kv in polymerPairs)
{
    if(!charCounts.ContainsKey(kv.Key.Item1)) charCounts.Add(kv.Key.Item1, (ulong) 0);

    charCounts[kv.Key.Item1] += kv.Value;
}

if(!charCounts.ContainsKey(initialPolymer.Last())) charCounts.Add(initialPolymer.Last(), (ulong) 0);

charCounts[initialPolymer.Last()]++;

var max = charCounts.Values.Max();
var min = charCounts.Values.Min();

Console.WriteLine(max - min);
