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

int steps = 10;

var currentPolymer = initialPolymer;

for(int i = 0; i < steps; i++)
{
    StringBuilder sb = new StringBuilder();
    for(int charIndex = 0; charIndex < currentPolymer.Length - 1; charIndex++)
    {
        sb.Append(currentPolymer[charIndex]);
        sb.Append(instructions[(currentPolymer[charIndex], currentPolymer[charIndex+1])]);
    }
    sb.Append(currentPolymer[currentPolymer.Length - 1]);

    currentPolymer = sb.ToString();
}

var groups = currentPolymer.GroupBy(x => x).Select(x => x.Count()).ToArray();

var max = groups.Max();
var min = groups.Min();

Console.WriteLine(max - min);