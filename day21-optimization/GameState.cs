public record struct GameState(PlayerState Player0State, PlayerState Player1State, int PlayerOnTurn)
{
    public bool IsCompleted => Player0State.HasWon || Player1State.HasWon;
    public int MaxScore => Math.Max(Player0State.CurrentScore, Player1State.CurrentScore);
}