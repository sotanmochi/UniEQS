namespace UniEQS
{
    public abstract class EnvQueryTestBase
    {
        public bool Enabled = true;
        public float ScaleFactor = 1.0f;
        public EnvQueryTestScoringEquation ScoringEquation = EnvQueryTestScoringEquation.Linear;
    }
}