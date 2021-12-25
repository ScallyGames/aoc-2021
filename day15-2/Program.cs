using SFML.Graphics;
using SFML.Window;

var window = new Window();
window.Run();


class Window
{
    const int size = 1;

    int[,] dangers;
    int currentMinDanger = int.MaxValue;
    Stack<(int x, int y)> cheapestPath = new Stack<(int x, int y)>();

    PriorityQueue<(int x, int y, Stack<(int x, int y)> path), int> openList = new PriorityQueue<(int x, int y, Stack<(int x, int y)> path), int>();
    
    Dictionary<(int x, int y), int> reachability = new Dictionary<(int x, int y), int>();

    (int x, int y) targetNode;

    public void Run()
    {
        string[] lines = File.ReadAllLines("input.txt");

        dangers = new int[lines.Length, lines[0].Length];

        // Generate danger map
        for (int y = 0; y < dangers.GetLength(0); y++)
        {
            for (int x = 0; x < dangers.GetLength(1); x++)
            {
                dangers[y, x] = int.Parse("" + lines[y][x]);
            }
        }

        targetNode = (lines.Length * 5 - 1, lines[0].Length * 5 - 1);

        openList.Enqueue((0, 0, new Stack<(int x, int y)>()), 0);

        Console.WriteLine(dangers.GetLength(1));
        RenderWindow window = new RenderWindow(new VideoMode((uint) dangers.GetLength(1) * 5 * size, (uint) dangers.GetLength(0) * 5 * size), "AoC Day 15");

        window.SetActive();
        window.Closed += new EventHandler(OnClose);

        while(window.IsOpen)
        {
            window.Clear();
            window.DispatchEvents();
            this.OnRenderFrame(window);
            window.Display();
        }
    }

    private void OnClose(object sender, EventArgs e)
    {
        RenderWindow window = (RenderWindow) sender;
        window.Close();
    }

    protected void OnRenderFrame(RenderWindow window)
    {
        bool didRenderFrame = false;

        int frameCount = 600;

        while(!didRenderFrame)
        {
            if (openList.Count > 0)
            {
                var nextNodeEntry = openList.Dequeue();
                var nextNode = (nextNodeEntry.x, nextNodeEntry.y);
                var currentDanger =
                    nextNodeEntry.path
                        .Reverse()
                        .Skip(1)
                        .Select(n => GetCost(dangers, n.x, n.y))
                        .Sum();

                if (nextNodeEntry.path.Contains((nextNode.x, nextNode.y))) continue;
                if (
                    nextNode.x < 0 ||
                    nextNode.x >= dangers.GetLength(1) * 5 ||
                    nextNode.y < 0 ||
                    nextNode.y >= dangers.GetLength(0) * 5
                ) continue;

                nextNodeEntry.path.Push(nextNode);
                currentDanger += GetCost(dangers, nextNode.x, nextNode.y);

                if(reachability.ContainsKey(nextNode) && reachability[nextNode] <= currentDanger) continue;
                
                reachability[nextNode] = currentDanger;

                if(frameCount == 0)
                {
                    this.Render(dangers, nextNodeEntry.path, window);
                    didRenderFrame = true;
                    frameCount = 600;
                }
                frameCount--;

                if (currentDanger > currentMinDanger)
                {
                    continue;
                }

                if (nextNode == targetNode)
                {
                    currentMinDanger = currentDanger;
                    cheapestPath = nextNodeEntry.path;
                    continue;
                }

                openList.Enqueue((nextNode.x, nextNode.y - 1, new Stack<(int x, int y)>(nextNodeEntry.path.Reverse())), currentDanger + GetEstimateCost(nextNode, targetNode));
                openList.Enqueue((nextNode.x - 1, nextNode.y, new Stack<(int x, int y)>(nextNodeEntry.path.Reverse())), currentDanger + GetEstimateCost(nextNode, targetNode));
                openList.Enqueue((nextNode.x, nextNode.y + 1, new Stack<(int x, int y)>(nextNodeEntry.path.Reverse())), currentDanger + GetEstimateCost(nextNode, targetNode));
                openList.Enqueue((nextNode.x + 1, nextNode.y, new Stack<(int x, int y)>(nextNodeEntry.path.Reverse())), currentDanger + GetEstimateCost(nextNode, targetNode));
            }
            else
            {
                Render(dangers, cheapestPath, window, true);
                didRenderFrame = true;
            }
        }
    }

    private void Render(int[,] dangers, Stack<(int x, int y)> path, RenderWindow window, bool isCompletePath = false)
    {
        HashSet<(int x, int y)> pathPixels = new HashSet<(int x, int y)>(path);
        Image image = new Image((uint)dangers.GetLength(1) * 5, (uint)dangers.GetLength(0) * 5);


        for(int y = 0; y < dangers.GetLength(0) * 5; y++)
        {
            for(int x = 0; x < dangers.GetLength(1) * 5; x++)
            {
                var cost = GetCost(dangers, x, y);
                byte greyLevel = (byte)(cost * (255 / 10));
                image.SetPixel((uint)x, (uint)y, pathPixels.Contains((x, y)) ? (isCompletePath ? Color.Green : Color.Red) : new Color(greyLevel, greyLevel, greyLevel));
            }
        }
        window.Draw(new Sprite(new Texture(image)));
    }

    int GetCost(int[,] dangers, int x, int y)
    {
        return ((dangers[y % dangers.GetLength(0), x % dangers.GetLength(1)] + (y / dangers.GetLength(0) + x / dangers.GetLength(1))) - 1) % 9 + 1;
    }

    int GetEstimateCost((int x, int y) currentPosition, (int x, int y) targetPosition)
    {
        return Math.Abs(targetPosition.x - currentPosition.x) + Math.Abs(targetPosition.y - currentPosition.y) * 1;
    }
}

