using SFML.Graphics;
using SFML.Window;
using System.Diagnostics;

var window = new Window();
window.Run();


class Window
{
    bool isStoringLitPoints = true;
    string[] lines;
    string lookupTable;

    RenderWindow window;
    
    int minX;
    int minY;
    int maxX;
    int maxY;

    HashSet<(int x, int y)> activePoints;

    Stopwatch sw;

    public void Run()
    {
        sw = new Stopwatch();
        sw.Start();

        VideoMode mode = new VideoMode(1000, 1000);
        window = new RenderWindow(mode, "Visualization");

        lines = File.ReadAllLines("input.txt");
        
        lookupTable = lines[0];

        
        string[] imageData = lines.Skip(2).ToArray();


        activePoints = new HashSet<(int x, int y)>();

        for(int y = 0; y < imageData.Length; y++)
        {
            for(int x = 0; x < imageData[y].Length; x++)
            {
                if(imageData[y][x] == '#')
                {
                    activePoints.Add((x, y));
                }
            }
        }


        window.SetActive();
        window.Closed += new EventHandler(OnClose);

        while(window.IsOpen)
        {
            window.Clear();
            window.DispatchEvents();
            if(sw.ElapsedMilliseconds > 10)
            {
                activePoints = GetIterationResult(activePoints);
                activePoints = GetIterationResult(activePoints);
                sw.Restart();
            }
            this.OnRenderFrame(window);
            window.Display();
        }
    }


    HashSet<(int x, int y)> GetIterationResult(HashSet<(int x, int y)> previousState)
    {
        HashSet<(int x, int y)> activePoints = new HashSet<(int x, int y)>();

        minX = previousState.Min(x => x.x);
        minY = previousState.Min(x => x.y);
        maxX = previousState.Max(x => x.x);
        maxY = previousState.Max(x => x.y);

        for(int y = minY - 1; y <= maxY + 1; y++)
        {
            for(int x = minX - 1; x <= maxX + 1; x++)
            {
                if(GetKernelResult(x, y, previousState, (minX, minY, maxX, maxY)) != isStoringLitPoints)
                {
                    activePoints.Add((x, y));
                }
            }
        }

        isStoringLitPoints = !isStoringLitPoints;

        return activePoints;
    }

        
    bool GetKernelResult(int x, int y, HashSet<(int x, int y)> previousState, (int minX, int minY, int maxX, int maxY) bounds)
    {
        int lookupIndex = 0;

        for(int offsetY = 0; offsetY < 3; offsetY++)
        {
            int sampleY = y + offsetY - 1;
            for(int offsetX = 0; offsetX < 3; offsetX++)
            {
                int sampleX = x + offsetX - 1;
                
                if(previousState.Contains((sampleX, sampleY)))
                {
                    lookupIndex |= 1 << (8 - ((offsetY * 3) + offsetX));
                }
            }
        }
        
        if(!isStoringLitPoints)
        {
            lookupIndex = ~lookupIndex;
            lookupIndex = lookupIndex & (512 - 1); // limit to 9 bits
        }

        return lookupTable[lookupIndex] == '#';
    }

    bool IsPaddedWithTrue(int x, int y, (int minX, int minY, int maxX, int maxY) bounds)
    {
        return !isStoringLitPoints && 
            (
                x < bounds.minX ||
                x > bounds.maxX ||
                y < bounds.minY ||
                y > bounds.maxY
            );
    }
    

    private void OnClose(object sender, EventArgs e)
    {
        RenderWindow window = (RenderWindow) sender;
        window.Close();
    }

    protected void OnRenderFrame(RenderWindow window)
    {
        if(window.Size.X <= 0 || window.Size.Y <= 0) return;

        Image image = new Image(window.Size.X, window.Size.Y);

        for(int y = 0; y < image.Size.Y; y++)
        {
            int sampleY = y - (int)window.Size.Y / 2;
            for(int x = 0; x < image.Size.X; x++)
            {
                int sampleX = x - (int)window.Size.X / 2;

                if(this.activePoints.Contains((sampleX, sampleY)))
                {
                    image.SetPixel((uint)x, (uint)y, isStoringLitPoints ? Color.Yellow : Color.Black);
                }
                else
                {
                    image.SetPixel((uint)x, (uint)y, isStoringLitPoints ? Color.Black : Color.Yellow);
                }
                if(IsPaddedWithTrue(sampleX, sampleY, (minX, minY, maxX, maxY)))
                {
                    image.SetPixel((uint)x, (uint)y, Color.Yellow);
                }
            }
        }

        window.Draw(new Sprite(new Texture(image)));
    }
}