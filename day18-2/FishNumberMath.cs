public static class FishNumberMath
{
    public static FishNumber Add(FishNumber a, FishNumber b)
    {
        var result = new FishNumber(a, b);
        result.Reduce();

        return result;
    }
}