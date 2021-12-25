using System.Diagnostics;

IDie die = new DiracDie();

Stopwatch sw = new Stopwatch();
sw.Start();

GameState initialGameState = new GameState(new PlayerState(7, 0), new PlayerState(4, 0), 0);


Dictionary<GameState, ulong> gameStateOccurances = new Dictionary<GameState, ulong>();
PriorityQueue<GameState, GameState> openGameStates = new PriorityQueue<GameState, GameState>(Comparer<GameState>.Create((a, b) => a.MaxScore - b.MaxScore));

gameStateOccurances.Add(initialGameState, 1);
openGameStates.Enqueue(initialGameState, initialGameState);

while(openGameStates.Count > 0)
{
    var gameStateToHandle = openGameStates.Dequeue();

    // Console.WriteLine(gameStateToHandle.GetHashCode() + " splits into:");

    List<RollResult> rollResults = new List<RollResult>();
    rollResults.Add(new RollResult(0, 1));
    
    for(int i = 0; i < GameSettings.NumberOfRolls; i++)
    {
        var dieResult = die.GetRollResult(); 
        rollResults = rollResults
            .SelectMany<RollResult, int, RollResult>(x => dieResult, (a, b) => new RollResult(a.Roll + b, a.NumberOfTimesRolled))
            .GroupBy(x => x.Roll)
            .Select(x => new RollResult(x.Key, (byte)x.Sum(x => x.NumberOfTimesRolled)))
            .ToList();
    }

    foreach(var roll in rollResults)
    {
        GameState newGameState;
        if(gameStateToHandle.PlayerOnTurn == 0)
        {
            int newPosition = gameStateToHandle.Player0State.CurrentPosition + roll.Roll;
            newPosition = ((newPosition - 1) % GameSettings.NumberOfFieldsInCircle) + 1;
            int newScore = gameStateToHandle.Player0State.CurrentScore + newPosition;
            PlayerState newPlayerState = new PlayerState(newPosition, newScore);
            newGameState = new GameState(newPlayerState, gameStateToHandle.Player1State, (gameStateToHandle.PlayerOnTurn + 1) % 2);
        }
        else
        {
            int newPosition = gameStateToHandle.Player1State.CurrentPosition + roll.Roll;
            newPosition = ((newPosition - 1) % GameSettings.NumberOfFieldsInCircle) + 1;
            int newScore = gameStateToHandle.Player1State.CurrentScore + newPosition;
            PlayerState newPlayerState = new PlayerState(newPosition, newScore);
            newGameState = new GameState(gameStateToHandle.Player0State, newPlayerState, (gameStateToHandle.PlayerOnTurn + 1) % 2);
        }
        
        // Console.Write($"    {newGameState.GetHashCode()}");
        if(!gameStateOccurances.ContainsKey(newGameState))
        {
            // Console.Write(" which has not yet been reached");
            gameStateOccurances.Add(newGameState, 0);

            if(!newGameState.IsCompleted)
            {
                openGameStates.Enqueue(newGameState, newGameState);
            }
        }
        else
        {
            // Console.Write($" which has already been reached {gameStateOccurances[newGameState]} times");
        }
        gameStateOccurances[newGameState] += gameStateOccurances[gameStateToHandle] * roll.NumberOfTimesRolled;
        
        // Console.Write($" and has now been reached {gameStateOccurances[newGameState]} times");
        // Console.WriteLine();
    }
    
    gameStateOccurances.Remove(gameStateToHandle);
}

ulong numberOfGamesWonByPlayer0 = gameStateOccurances.Where(x => x.Key.Player0State.HasWon).Aggregate(0UL, (a, b) => a + b.Value);
ulong numberOfGamesWonByPlayer1 = gameStateOccurances.Where(x => x.Key.Player1State.HasWon).Aggregate(0UL, (a, b) => a + b.Value);

Console.WriteLine(numberOfGamesWonByPlayer0);
Console.WriteLine(numberOfGamesWonByPlayer1);

sw.Stop();
Console.WriteLine("Took " + sw.Elapsed);