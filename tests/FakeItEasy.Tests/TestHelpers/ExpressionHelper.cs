namespace FakeItEasy.Tests.TestHelpers
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using FakeItEasy.Configuration;
    using FakeItEasy.Core;

    internal static class ExpressionHelper
    {
        public static Expression<Action<T>> CreateExpression<T>(Expression<Action<T>> expression)
        {
            return expression;
        }

        public static IInterceptedFakeObjectCall CreateFakeCall<TFake, TReturn>(Expression<Func<TFake, TReturn>> callSpecification)
            where TFake : class
        {
            return CreateFakeCall(A.Fake<TFake>(), callSpecification);
        }

        public static IInterceptedFakeObjectCall CreateFakeCall<TFake>(Expression<Action<TFake>> callSpecification)
            where TFake : class
        {
            return CreateFakeCall(A.Fake<TFake>(), callSpecification);
        }

        private static IInterceptedFakeObjectCall CreateFakeCall<TFake>(TFake fakedObject, LambdaExpression callSpecification)
        {
            var result = A.Fake<IInterceptedFakeObjectCall>();

            A.CallTo(() => result.Method).Returns(GetMethodInfo(callSpecification));
            A.CallTo(() => result.FakedObject).Returns(fakedObject);
            A.CallTo(() => result.Arguments).Returns(CreateArgumentCollection(callSpecification));

            return result;
        }

        private static MethodInfo GetMethodInfo(LambdaExpression callSpecification)
        {
            var methodExpression = callSpecification.Body as MethodCallExpression;
            if (methodExpression is object)
            {
                return methodExpression.Method;
            }

            var memberExpression = (MemberExpression)callSpecification.Body;
            var property = (PropertyInfo)memberExpression.Member;
            return property.GetGetMethod(true);
        }

        private static ArgumentCollection CreateArgumentCollection(LambdaExpression callSpecification)
        {
            var methodCall = callSpecification.Body as MethodCallExpression;

            MethodInfo method;
            object[] arguments;

            if (methodCall is object)
            {
                method = methodCall.Method;
                arguments =
                    (from argument in methodCall.Arguments
                     select argument.Evaluate()).ToArray();
            }
            else
            {
                var propertyCall = (MemberExpression)callSpecification.Body;
                var property = (PropertyInfo)propertyCall.Member;

                method = property.GetGetMethod(true);
                arguments = Array.Empty<object>();
            }

            return new ArgumentCollection(arguments, method);
        }
    }
}
