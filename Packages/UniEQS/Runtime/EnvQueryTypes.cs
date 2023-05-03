namespace UniEQS
{
    public enum EnvQueryGeneratorType
    {
        OnCircle = 0,
        SimpleGrid = 1
    }

    public enum EnvQueryTestType
    {
        Unknown = 0,
        Distance = 1,
        Dot = 2,
    }

    public enum EnvQueryTestScoringEquation
    {
        Linear = 10,
        InverseLinear = 11,
        Square = 20,
        HalfSine = 30,
        InverseHalfSine = 31,
        HalfSineSquared = 32,
        InverseHalfSineSquared = 34,
        SigmoidLike = 40,
        InverseSigmoidLike = 41
    }
}