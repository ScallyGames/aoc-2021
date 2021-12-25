using System.Text;
using System.Text.RegularExpressions;

string[] lines = File.ReadAllLines("input-big.txt");


var coordinates = lines.TakeWhile(x => x.Length > 0).Select<string, (int x, int y)>(x => 
{
    var numbers = x.Split(',').Select(num => int.Parse(num)).ToArray();
    return (numbers[0], numbers[1]);
}).ToArray();

var extents = coordinates.Aggregate<(int x, int y), (int x, int y)>((0, 0), (a, b) => (Math.Max(a.x, b.x), Math.Max(a.y, b.y)));
extents = (extents.x + 1, extents.y + 1);

Console.WriteLine($"Extents: {extents.x}, {extents.y}");
var sheet = new bool[extents.y, extents.x];

foreach(var coordinate in coordinates)
{
    sheet[coordinate.y, coordinate.x] = true;
}

var instructions = lines.SkipWhile(x => x.Length > 0).Skip(1).TakeWhile(x => x.Length > 0).ToArray();

foreach(var instruction in instructions)
{
        
    Console.WriteLine(instruction);

    var match = Regex.Match(instruction, @"fold along ([x|y])=(\d+)");

    // print();

    if(match.Groups[1].Value == "x")
    {
        // X (Vertical)
        int foldIndex = int.Parse(match.Groups[2].Value);
        for(int y = 0; y < extents.y; y++)
        {
            for(int x = 0; x < foldIndex; x++)
            {
                var sourceIndex = foldIndex + (foldIndex - x);
                if(sourceIndex >= extents.x) continue;
                sheet[y, x] = sheet[y, x] || sheet[y, sourceIndex];
            }
        }

        extents = (foldIndex, extents.y);
    }
    else
    {
        // Y (Horizontal)
        int foldIndex = int.Parse(match.Groups[2].Value);
        for(int y = 0; y < foldIndex; y++)
        {
            var sourceIndex = foldIndex + (foldIndex - y);
            if(sourceIndex >= extents.y) continue;

            for(int x = 0; x < extents.x; x++)
            {
                sheet[y, x] = sheet[y, x] || sheet[foldIndex + (foldIndex - y), x];
            }
        }
        
        extents = (extents.x, foldIndex);
    }
}

Console.WriteLine();
print();

void print()
{
    StringBuilder sb = new StringBuilder();
    for(int y = 0; y < extents.y; y++)
    {
        for(int x = 0; x < extents.x; x++)
        {
            sb.Append(sheet[y, x] ? "X" : ".");
        }
        sb.AppendLine();
    }
    Console.WriteLine(sb.ToString());
}