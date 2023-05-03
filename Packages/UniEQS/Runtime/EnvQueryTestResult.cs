namespace UniEQS
{
    public struct EnvQueryTestResult
    {
        public EnvQueryTestType Type;
        public float Value;
        
        public EnvQueryTestResult(EnvQueryTestType type, float value)
        {
            Type = type;
            Value = value;
        }
    }
}