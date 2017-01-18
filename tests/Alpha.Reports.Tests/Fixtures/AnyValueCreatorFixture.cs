namespace Alpha.Reports.Tests.Fixtures
{
    using Ploeh.AutoFixture;

    public class AnyValueCreatorFixture
    {
        private readonly Fixture anyValueGenerator;

        public AnyValueCreatorFixture()
        {
            anyValueGenerator = new Fixture();
        }

        public T Create<T>()
        {
            return anyValueGenerator.Create<T>();
        }
    }
}
