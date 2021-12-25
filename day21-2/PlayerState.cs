public record struct PlayerState(int CurrentPosition, int CurrentScore)
{
    public bool HasWon => CurrentScore >= GameSettings.WinningScore;
}