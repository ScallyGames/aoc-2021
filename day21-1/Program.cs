IDie die = new DeterministicDie();

int playerOnTurnIndex = 0;
int[] playerPositions = new int[2] { 7, 4};
// int[] playerPositions = new int[2] { 4, 8};
int[] playerScores = new int[2];

const int winningScore = 1000;
const int numberOfRolls = 3;
const int numberOfFieldsInCircle = 10;

while(true)
{
    int rollResults = 0;
    Console.Write($"Player {playerOnTurnIndex + 1} rolls ");
    for(int i = 0; i < numberOfRolls; i++)
    {
        int dieResult = die.GetRollResult(); 
        rollResults += dieResult;
        Console.Write(dieResult + "+");
    }
    
    playerPositions[playerOnTurnIndex] += rollResults;
    playerPositions[playerOnTurnIndex] = ((playerPositions[playerOnTurnIndex] - 1) % numberOfFieldsInCircle) + 1;
    Console.Write($" reaching field {playerPositions[playerOnTurnIndex]}");

    playerScores[playerOnTurnIndex] += playerPositions[playerOnTurnIndex];


    Console.WriteLine();

    if(playerScores[playerOnTurnIndex] >= winningScore)
    {
        Console.WriteLine($"Player {playerOnTurnIndex + 1} wins.");
        break;
    }
    playerOnTurnIndex = (playerOnTurnIndex + 1) % 2;

}

Console.WriteLine(playerScores[(playerOnTurnIndex + 1) % 2]);
Console.WriteLine(die.GetNumberOfRolls());
Console.WriteLine(playerScores[(playerOnTurnIndex + 1) % 2] * die.GetNumberOfRolls());