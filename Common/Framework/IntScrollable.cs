public struct IntScrollable
{
    private int val;
    public int Val
    {
        get => val;
        set 
        {
            val = value;
            if (val > Max)
                val = Min;
            if (val < Min)
                val = Max;
        }
    }
    public int Max;
    public int Min;

    public IntScrollable(int min, int max)
    {
        Min = min;
        Max = max;
        val = min;
    }

    public int SetAndReturn(int value)
    {
        Val = value;
        return Val;
    }
    public int ScrollUp() => ++Val;
    public int ScrollDown() => --Val;
}