record State
{
    private State? previous = null;
    public State? Previous
    {
        get
        {
            return this.previous;
        }
        set
        {
            if(this.previous == value) return;

            this.previous = value;
            this.totalCostMemoized = null;
        }
    }

    private int? totalCostMemoized = null;

    public int TotalCost
    {
        get
        {
            if(totalCostMemoized == null)
            {
                int cost = 0;
                State? current = this;
                while(current != null)
                {
                    cost += current.Cost;
                    current = current.Previous;
                }
                totalCostMemoized = cost;
            }
            return (int)totalCostMemoized;
        }
    }

    public RoomSetup RoomSetup { get; private set; }
    public int Cost { get; private set; }

    public State(RoomSetup roomSetup, int cost, State? previous)
    {
        this.RoomSetup = roomSetup;
        this.Cost = cost;
        this.previous = previous;
    }
}