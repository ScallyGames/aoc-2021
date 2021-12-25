// puzzle input
char[,] layout = new char[,]
{
    { '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#' },
    { '#', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '#' },
    { '#', '#', '#', 'B', '#', 'A', '#', 'A', '#', 'D', '#', '#', '#' },
    { ' ', ' ', '#', 'D', '#', 'C', '#', 'B', '#', 'A', '#', ' ', ' ' },
    { ' ', ' ', '#', 'D', '#', 'B', '#', 'A', '#', 'C', '#', ' ', ' ' },
    { ' ', ' ', '#', 'D', '#', 'C', '#', 'B', '#', 'C', '#', ' ', ' ' },
    { ' ', ' ', '#', '#', '#', '#', '#', '#', '#', '#', '#', ' ', ' ' },
};

// // test input
// char[,] layout = new char[,]
// {
//     { '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#' },
//     { '#', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '#' },
//     { '#', '#', '#', 'B', '#', 'C', '#', 'B', '#', 'D', '#', '#', '#' },
//     { ' ', ' ', '#', 'A', '#', 'D', '#', 'C', '#', 'A', '#', ' ', ' ' },
//     { ' ', ' ', '#', '#', '#', '#', '#', '#', '#', '#', '#', ' ', ' ' },
// };



Vector2[] possibleMoves = new Vector2[] {
    new Vector2(0, -1),
    new Vector2(0, +1),
    new Vector2(+1, 0),
    new Vector2(-1, 0),
};

State initial = new State(new RoomSetup(layout), 0);


HashSet<RoomSetup> checkedSetups = new HashSet<RoomSetup>();
PriorityQueue<State, State> openList = new PriorityQueue<State, State>(Comparer<State>.Create((a, b) => (a.Cost + a.RoomSetup.Heuristic) - (b.Cost + b.RoomSetup.Heuristic)));
openList.Enqueue(initial, initial);

int numberOfSteps = 0;
int currentMinimum = int.MaxValue;
State bestState;

while(openList.Count > 0)
{
    var currentState = openList.Dequeue();

    if(checkedSetups.Contains(currentState.RoomSetup)) continue;
    checkedSetups.Add(currentState.RoomSetup);

    if(currentState.Cost >= currentMinimum) continue;

    if(currentState.RoomSetup == InputData.TargetSetup)
    {
        currentMinimum = currentState.Cost;
        bestState = currentState;

        if(currentState.RoomSetup.correctBonus > 10)
        {
            PriorityQueue<State, State> newQueue = new PriorityQueue<State, State>(Comparer<State>.Create((a, b) => (a.Cost + a.RoomSetup.Heuristic) - (b.Cost + b.RoomSetup.Heuristic)));
            while(openList.Count > 0)
            {
                var old = openList.Dequeue();
                var rescaledState = new State(new RoomSetup(old.RoomSetup.Value, old.RoomSetup.correctBonus / 10), old.Cost);
                newQueue.Enqueue(rescaledState, rescaledState);
            }
            openList = newQueue;
        }
        Console.WriteLine(openList.Count + " remaining");
        Console.WriteLine("Found new minimum");
        continue;
    }

    numberOfSteps++;

    if(currentMinimum != int.MaxValue && numberOfSteps % 100000 == 0)
    {
        // Console.WriteLine(currentMinimum - currentState.Cost);
        Console.WriteLine(openList.Count + " remaining");
    }

    // PrintRoomSetup(currentState.RoomSetup);

    // find options
    for(int y = 0; y < currentState.RoomSetup.Value.GetLength(0); y++)
    {
        for(int x = 0; x < currentState.RoomSetup.Value.GetLength(1); x++)
        {
            char charToMove = currentState.RoomSetup.Value[y, x];
            if(!"ABCD".Contains(charToMove)) continue;

            if(
                InputData.TargetSetup.Value[y, x] == charToMove && 
                currentState.RoomSetup.Value[2, x] == charToMove && 
                currentState.RoomSetup.Value[3, x] == charToMove
            )
            {
                continue;
            } 

            Vector2 sourcePosition = new Vector2(x, y);
            
            HashSet<Vector2> done = new HashSet<Vector2>();
            Queue<(Vector2 position, int cost)> openMoves = new Queue<(Vector2 position, int cost)>();
            openMoves.Enqueue((new Vector2(x, y), 0));
            done.Add(new Vector2(x, y));

            while(openMoves.Any())
            {
                var currentMove = openMoves.Dequeue();
                foreach(var move in possibleMoves)
                {
                    Vector2 newPosition = new Vector2(currentMove.position.X + move.X, currentMove.position.Y + move.Y);
                    if(currentState.RoomSetup.Value[newPosition.Y, newPosition.X] != '.') continue;
                    if(done.Contains(newPosition)) continue;
                    
                    int newCost = currentMove.cost + GetCost(charToMove);
                    openMoves.Enqueue((newPosition, newCost));
                    done.Add(newPosition);

                    if(IsInCorridor(newPosition) && IsInCorridor(sourcePosition)) continue;

                    if(!IsInCorridor(newPosition) && !IsValidTargetRoom(newPosition, currentState.RoomSetup.Value, charToMove)) continue;

                    if(newPosition.X == sourcePosition.X) continue;

                    // Generate new state
                    var newRoomLayout = (char[,])currentState.RoomSetup.Value.Clone();
                    newRoomLayout[sourcePosition.Y, sourcePosition.X] = '.';
                    newRoomLayout[newPosition.Y, newPosition.X] = charToMove;
                    State newState = new State(new RoomSetup(newRoomLayout, currentState.RoomSetup.correctBonus), currentState.Cost + newCost);
                    if(IsValidState(newState))
                    {
                        openList.Enqueue(newState, newState);
                    }
                }
            }
        }
    }
}

Console.WriteLine(currentMinimum);
Console.WriteLine($"Took {numberOfSteps} steps");

int GetCost(char character) => character switch { 'A' => 1, 'B' => 10, 'C' => 100, 'D' => 1000, _ => throw new Exception("unexpected value") };

bool IsInCorridor(Vector2 position) => position.Y == 1;

bool IsValidState(State state) => state.RoomSetup.Value[1, 3] == '.' && state.RoomSetup.Value[1, 5] == '.' && state.RoomSetup.Value[1, 7] == '.' && state.RoomSetup.Value[1, 9] == '.';

bool IsValidTargetRoom(Vector2 targetPosition, char[,] roomSetup, char charToMove)
{
    if(roomSetup[targetPosition.Y, targetPosition.X] != '.') return false;
    if(InputData.TargetSetup.Value[targetPosition.Y, targetPosition.X] != charToMove) return false;
    if("ABCD".Where(x => x != charToMove).Contains(InputData.TargetSetup.Value[2, targetPosition.X])) return false;
    if("ABCD".Where(x => x != charToMove).Contains(InputData.TargetSetup.Value[3, targetPosition.X])) return false;
    if("ABCD".Where(x => x != charToMove).Contains(InputData.TargetSetup.Value[4, targetPosition.X])) return false;
    if("ABCD".Where(x => x != charToMove).Contains(InputData.TargetSetup.Value[5, targetPosition.X])) return false;

    for(int i = targetPosition.Y + 1; i < roomSetup.GetLength(0); i++)
    {
        if(roomSetup[i, targetPosition.X] == '.') return false;
    }

    return true;    
}

void PrintRoomSetup(RoomSetup roomSetup)
{
    for(int y = 0; y < roomSetup.Value.GetLength(0); y++)
    {
        for(int x = 0; x < roomSetup.Value.GetLength(1); x++)
        {
            Console.Write(roomSetup.Value[y, x]);
        }
        Console.WriteLine();
    }
    Console.WriteLine();
}
