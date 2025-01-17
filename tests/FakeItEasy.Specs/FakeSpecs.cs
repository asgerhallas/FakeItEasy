namespace FakeItEasy.Specs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FakeItEasy.Core;
    using FluentAssertions;
    using Xbehave;

    public static class FakeSpecs
    {
        public interface IFoo
        {
            void AMethod();

            void AnotherMethod();

            void AnotherMethod(string text);

            object AMethodReturningAnObject();
        }

        [Scenario]
        public static void NonGenericCallsSuccess(
            IFoo fake,
            IEnumerable<ICompletedFakeObjectCall> completedCalls)
        {
            "Given a Fake"
                .x(() => fake = A.Fake<IFoo>());

            "And I make several calls to the Fake"
                .x(() =>
                {
                    fake.AMethod();
                    fake.AnotherMethod();
                    fake.AnotherMethod("houseboat");
                });

            "When I use the static Fake class to get the calls made on the Fake"
                .x(() => completedCalls = Fake.GetCalls(fake));

            "Then the calls made to the Fake will be returned"
                .x(() =>
                    completedCalls.Select(call => call.Method.Name)
                        .Should()
                        .Equal("AMethod", "AnotherMethod", "AnotherMethod"));
        }

        [Scenario]
        public static void MatchingCallsWithNoMatches(
            IFoo fake,
            IEnumerable<ICompletedFakeObjectCall> completedCalls,
            IEnumerable<ICompletedFakeObjectCall> matchedCalls)
        {
            "Given a Fake"
                .x(() => fake = A.Fake<IFoo>());

            "And I make several calls to the Fake"
                .x(() =>
                {
                    fake.AMethod();
                    fake.AnotherMethod("houseboat");
                });

            "And I use the static Fake class to get the calls made on the Fake"
                .x(() => completedCalls = Fake.GetCalls(fake));

            "When I use Matching to find calls to a method with no matches"
                .x(() => matchedCalls = completedCalls.Matching<IFoo>(c => c.AnotherMethod("hovercraft")));

            "Then it finds no calls"
                .x(() => matchedCalls.Should().BeEmpty());
        }

        [Scenario]
        public static void MatchingCallsWithMatches(
            IFoo fake,
            IEnumerable<ICompletedFakeObjectCall> completedCalls,
            IEnumerable<ICompletedFakeObjectCall> matchedCalls)
        {
            "Given a Fake"
                .x(() => fake = A.Fake<IFoo>());

            "And I make several calls to the Fake"
                .x(() =>
                {
                    fake.AMethod();
                    fake.AnotherMethod();
                    fake.AnotherMethod("houseboat");
                });

            "And I use the static Fake class to get the calls made on the Fake"
                .x(() => completedCalls = Fake.GetCalls(fake));

            "When I use Matching to find calls to a method with a match"
                .x(() => matchedCalls = completedCalls.Matching<IFoo>(c => c.AnotherMethod("houseboat")));

            "Then it finds the matching call"
                .x(() => matchedCalls.Select(c => c.Method.Name).Should().Equal("AnotherMethod"));
        }

        [Scenario]
        public static void ClearRecordedCalls(IFoo fake)
        {
            "Given a Fake"
                .x(() => fake = A.Fake<IFoo>());

            "And I make several calls to the Fake"
                .x(() =>
                {
                    fake.AMethod();
                    fake.AnotherMethod();
                    fake.AnotherMethod("houseboat");
                });

            "When I clear the recorded calls"
                .x(() => Fake.ClearRecordedCalls(fake));

            "Then the recorded call list is empty"
                .x(() => Fake.GetCalls(fake).Should().BeEmpty());
        }

        [Scenario]
        public static void ClearConfiguration(IFoo fake, bool configuredBehaviorWasUsed)
        {
            "Given a Fake"
                .x(() => fake = A.Fake<IFoo>());

            "And I configure a method call on the Fake"
                .x(() => A.CallTo(() => fake.AMethod()).Invokes(() => configuredBehaviorWasUsed = true));

            "When I clear the configuration"
                .x(() => Fake.ClearConfiguration(fake));

            "And I execute the previously-configured method"
                .x(() => fake.AMethod());

            "Then previously-configured behavior is not executed"
                .x(() => configuredBehaviorWasUsed.Should().BeFalse());
        }

        [Scenario]
        public static void RecordedCallHasReturnValue(
            IFoo fake,
            object returnValue,
            ICompletedFakeObjectCall recordedCall)
        {
            "Given a fake"
                .x(() => fake = A.Fake<IFoo>());

            "When I call a method on the fake"
                .x(() => returnValue = fake.AMethodReturningAnObject());

            "And I retrieve the call from the recorded calls"
                .x(() => recordedCall = Fake.GetCalls(fake).Single());

            "Then the recorded call's return value should be the value that was actually returned"
                .x(() => recordedCall.ReturnValue.Should().Be(returnValue));
        }

        [Scenario]
        public static void RecordedDelegateCallHasReturnValue(
            Func<object> fake,
            object returnValue,
            ICompletedFakeObjectCall recordedCall)
        {
            "Given a fake delegate"
                .x(() => fake = A.Fake<Func<object>>());

            "When I invoke the fake delegate"
                .x(() => returnValue = fake());

            "And I retrieve the call from the recorded calls"
                .x(() => recordedCall = Fake.GetCalls(fake).Single());

            "Then the recorded call's return value should be the value that was actually returned"
                .x(() => recordedCall.ReturnValue.Should().Be(returnValue));
        }
    }
}
