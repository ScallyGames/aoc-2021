int minX = 138;
int maxX = 184;
int minY = -71;
int maxY = -125;

int totalMaxHeight = int.MinValue;
(int x, int y) coolestVelocity = (int.MinValue, int.MinValue);

for(int y = maxY; y < 1000; y++)
{
    for(int x = 0; x < maxX; x++)
    {
        (int x, int y) currentPosition = (0, 0);
        (int x, int y) initialVelocity = (x, y);
        (int x, int y) currentVelocity = initialVelocity;

        int maxHeight = int.MinValue;
        while(currentPosition.y >= maxY && currentPosition.x <= maxX)
        {
            currentPosition.x += currentVelocity.x;
            currentPosition.y += currentVelocity.y;

            currentVelocity.x += -Math.Sign(currentVelocity.x);
            currentVelocity.y -= 1;

            maxHeight = Math.Max(maxHeight, currentPosition.y);

            if(currentPosition.x >= minX && currentPosition.x <= maxX && currentPosition.y >= maxY && currentPosition.y <= minY)
            {
                if(maxHeight > totalMaxHeight)
                {
                    coolestVelocity = initialVelocity;
                    totalMaxHeight = maxHeight;
                }
                Console.WriteLine(initialVelocity + " is a valid initial velocity reaching " + maxHeight);
            }
        }
    }
}

Console.WriteLine();
Console.WriteLine("Coolest velocity: " + coolestVelocity + " reaching " + totalMaxHeight);