using System;

public class SizeEatingCondition : IEatingCondition
{
    private int _currentEatingSize;

    public SizeEatingCondition(int startEatingSize)
    {
        _currentEatingSize = startEatingSize;
    }

    public void IncreaseSize()
    {
        _currentEatingSize++;
    }

    public bool CanEat(Eatable eatable)
    {
        var size = eatable.GetComponent<EatableSize>();

        if (size == null)
            throw new ArgumentException($"{nameof(Eatable)} must have {nameof(EatableSize)} to use {nameof(SizeEatingCondition)}");

        return size.Size <= _currentEatingSize;
    }
}