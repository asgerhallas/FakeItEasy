namespace FakeItEasy.Tests.ArgumentConstraintManagerExtensions
{
    using System.Collections.Generic;
    using FakeItEasy.Tests.TestHelpers;
    using Xunit;

    public class NullableIsNotNullTests : ArgumentConstraintTestBase<int?>
    {
        protected override string ExpectedDescription => "NOT NULL";

        public static IEnumerable<object[]> ValidValues()
        {
            return TestCases.FromObject(0, -1, 42);
        }

        public static IEnumerable<object[]> InvalidValues()
        {
            return TestCases.FromObject((object)null);
        }

        [Theory]
        [MemberData(nameof(InvalidValues))]
        public override void IsValid_should_return_false_for_invalid_values(object invalidValue)
        {
            base.IsValid_should_return_false_for_invalid_values(invalidValue);
        }

        [Theory]
        [MemberData(nameof(ValidValues))]
        public override void IsValid_should_return_true_for_valid_values(object validValue)
        {
            base.IsValid_should_return_true_for_valid_values(validValue);
        }

        protected override void CreateConstraint(INegatableArgumentConstraintManager<int?> scope) => scope.IsNotNull();
    }
}
