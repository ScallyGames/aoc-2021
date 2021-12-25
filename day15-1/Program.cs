string[] lines = File.ReadAllLines("input.txt");

int[,] dangers = new int[lines.Length, lines[0].Length];

// Generate danger map
for(int y = 0; y < dangers.GetLength(0); y++)
{
    for(int x = 0; x < dangers.GetLength(1); x++)
    {
        dangers[y, x] = int.Parse("" + lines[y][x]);
    }
}

int currentMinDanger = int.MaxValue;


// Pathfinding
PriorityQueue<(int x, int y, Stack<(int x, int y)> path), int> openList = new PriorityQueue<(int x, int y, Stack<(int x, int y)> path), int>();

var targetNode = (lines.Length - 1, lines[0].Length - 1);

openList.Enqueue((0, 0, new Stack<(int x, int y)>()), 0);

Dictionary<(int x, int y), int> reachability = new Dictionary<(int x, int y), int>();

Print(dangers, new Stack<(int x, int y)>());

while(openList.Count > 0)
{
    var nextNodeEntry = openList.Dequeue();
    var nextNode = (nextNodeEntry.x, nextNodeEntry.y);
    var currentDanger = nextNodeEntry.path.Reverse().Skip(1).Select(n => dangers[n.y, n.x]).Sum();

    // Console.ForegroundColor = ConsoleColor.Black;
    // Console.SetCursorPosition(0, lines.Length + 1);
    // Console.WriteLine("                                        ");
    // Console.SetCursorPosition(0, lines.Length + 1);
    // Console.WriteLine("Attempting " + nextNode.x + ", " +nextNode.y);

    if(nextNodeEntry.path.Contains((nextNode.x, nextNode.y))) continue;
    if(
        nextNode.x < 0 || 
        nextNode.x >= lines[0].Length || 
        nextNode.y < 0 ||
        nextNode.y >= lines.Length
    ) continue;

    nextNodeEntry.path.Push(nextNode);
    currentDanger += dangers[nextNode.y, nextNode.x];

    if(reachability.ContainsKey(nextNode) && reachability[nextNode] <= currentDanger) continue;
    
    reachability[nextNode] = currentDanger;
    
    Print(dangers, nextNodeEntry.path);

    if(currentDanger > currentMinDanger)
    {
        continue;
    }

    if(nextNode == targetNode)
    {
        currentMinDanger = currentDanger;
        continue;
    }

    openList.Enqueue((nextNode.x, nextNode.y - 1, new Stack<(int x, int y)>(nextNodeEntry.path.Reverse())), currentDanger);
    openList.Enqueue((nextNode.x - 1, nextNode.y, new Stack<(int x, int y)>(nextNodeEntry.path.Reverse())), currentDanger);
    openList.Enqueue((nextNode.x, nextNode.y + 1, new Stack<(int x, int y)>(nextNodeEntry.path.Reverse())), currentDanger);
    openList.Enqueue((nextNode.x + 1, nextNode.y, new Stack<(int x, int y)>(nextNodeEntry.path.Reverse())), currentDanger);
}


// Console.SetCursorPosition(0, lines.Length + 1);
// Console.WriteLine("                                        ");
// Console.SetCursorPosition(0, lines.Length + 1);
Console.WriteLine(currentMinDanger);

void Print(int[,] dangers, Stack<(int x, int y)> path)
{
    Console.SetCursorPosition(0, 0);

    for(int y = 0; y < dangers.GetLength(0); y++)
    {
        for(int x = 0; x < dangers.GetLength(1); x++)
        {
            if(path.Contains((x, y)))
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Black;
            }

            Console.Write(dangers[y, x]);
        }
        Console.WriteLine();
    }

    Console.WriteLine();
}
