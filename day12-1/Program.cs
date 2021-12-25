string[] lines = File.ReadAllLines("input.txt");

Dictionary<string, Cave> caves = new Dictionary<string, Cave>();


// Transform connections into cave graph
foreach(var line in lines)
{
    var connection = line.Split('-');

    for(int i = 0; i < connection.Length; i++)
    {
        if(!caves.ContainsKey(connection[i]))
        {
            caves.Add(connection[i], new Cave(connection[i]));
        }
    }

    for(int i = 0; i < connection.Length; i++)
    {
        caves[connection[i]].AdjacentCaves.Add(caves[connection[connection.Length - i - 1]]);
    }
}

var paths = caves["start"].GetAllPathsTo("end", new List<Cave>());

Console.WriteLine(paths.Count);

// foreach(var path in paths)
// {
//     Console.WriteLine(string.Join(',', path.Select(x => x.Name)));
// }