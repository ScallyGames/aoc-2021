using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;

string[] lines = File.ReadAllLines("input.txt");


var coordinates = lines.TakeWhile(x => x.Length > 0).Select<string, (int x, int y)>(x => 
{
    var numbers = x.Split(',').Select(num => int.Parse(num)).ToArray();
    return (numbers[0], numbers[1]);
}).ToArray();

var extents = coordinates.Aggregate<(int x, int y), (int x, int y)>((0, 0), (a, b) => (Math.Max(a.x, b.x), Math.Max(a.y, b.y)));
extents = (extents.x + 1, extents.y + 1);

Console.WriteLine($"Extents: {extents.x}, {extents.y}");
var sheet = new Bitmap(extents.x, extents.y);

foreach(var coordinate in coordinates)
{
    sheet.SetPixel(coordinate.x, coordinate.y, Color.Black);
}


var instructions = lines.SkipWhile(x => x.Length > 0).Skip(1).TakeWhile(x => x.Length > 0).ToArray();

var instructionData = instructions.Select(x => Regex.Match(x, @"fold along ([x|y])=(\d+)")).ToArray();

for(int y = 0; y < extents.y; y++)
{
    for(int x = 0; x < extents.x; x++)
    {
        if(
            instructionData.Where(x => x.Groups[1].Value == "x").Select(x => int.Parse(x.Groups[2].Value)).Contains(x) ||
            instructionData.Where(x => x.Groups[1].Value == "y").Select(x => int.Parse(x.Groups[2].Value)).Contains(y)
        )
        {
            sheet.SetPixel(x, y, Color.Red);
        }
    }
}

sheet.Save("input.bmp");

// Console.WriteLine();
// // print();

// void print()
// {
//     StringBuilder sb = new StringBuilder();
//     for(int y = 0; y < extents.y; y++)
//     {
//         for(int x = 0; x < extents.x; x++)
//         {
//             sb.Append(sheet[y, x] ? "X" : ".");
//         }
//         sb.AppendLine();
//     }
//     Console.WriteLine(sb.ToString());
// }