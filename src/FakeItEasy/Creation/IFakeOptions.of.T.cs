namespace FakeItEasy.Creation
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;

    /// <summary>
    /// Provides options for generating fake object.
    /// </summary>
    /// <typeparam name="T">The type of fake object generated.</typeparam>
    public interface IFakeOptions<T> : IHideObjectMembers where T : class
    {
        /// <summary>
        /// Specifies arguments for the constructor of the faked class.
        /// </summary>
        /// <param name="argumentsForConstructor">The arguments to pass to the constructor of the faked class.</param>
        /// <returns>Options object.</returns>
        IFakeOptions<T> WithArgumentsForConstructor(IEnumerable<object> argumentsForConstructor);

        /// <summary>
        /// Specifies arguments for the constructor of the faked class by giving an expression with the call to
        /// the desired constructor using the arguments to be passed to the constructor.
        /// </summary>
        /// <param name="constructorCall">The constructor call to use when creating a class proxy.</param>
        /// <returns>Options object.</returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is by design when using the Expression-, Action- and Func-types.")]
        IFakeOptions<T> WithArgumentsForConstructor(Expression<Func<T>> constructorCall);

        /// <summary>
        /// Specifies that the fake should delegate calls to the specified instance.
        /// </summary>
        /// <param name="wrappedInstance">The object to delegate calls to.</param>
        /// <returns>Options object.</returns>
        IFakeOptions<T> Wrapping(T wrappedInstance);

        /// <summary>
        /// Specifies that the fake should be created with these additional attributes.
        /// </summary>
        /// <param name="attributes">Expressions that create attributes to add to the proxy.</param>
        /// <returns>Options object.</returns>
        IFakeOptions<T> WithAttributes(params Expression<Func<Attribute>>[] attributes);

        /// <summary>
        /// Sets up the fake to implement the specified interface in addition to the
        /// originally faked class.
        /// </summary>
        /// <param name="interfaceType">The type of interface to implement.</param>
        /// <returns>Options object.</returns>
        /// <exception cref="ArgumentException">The specified type is not an interface.</exception>
        /// <exception cref="ArgumentNullException">The specified type is null.</exception>
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = nameof(Implements), Justification = "Would be a breaking change, might be changed in a later major version.")]
        IFakeOptions<T> Implements(Type interfaceType);

        /// <summary>
        /// Sets up the fake to implement the specified interface in addition to the
        /// originally faked class.
        /// </summary>
        /// <typeparam name="TInterface">The type of interface to implement.</typeparam>
        /// <returns>Options object.</returns>
        /// <exception cref="ArgumentException">The specified type is not an interface.</exception>
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = nameof(Implements), Justification = "Would be a breaking change, might be changed in a later major version.")]
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Used to provide a strongly typed fluent API.")]
        IFakeOptions<T> Implements<TInterface>();

        /// <summary>
        /// Specifies an action that should be run over the fake object for the initial configuration (during the creation of the fake proxy).
        /// </summary>
        /// <param name="action">An action to perform.</param>
        /// <returns>Options object.</returns>
        /// <remarks>
        /// <para>
        /// Note that this method might be called when the fake is not yet fully constructed, so <paramref name="action"/> should
        /// use the fake instance to set up behavior, but not rely on the instance's state.
        /// Also, if FakeItEasy has to try multiple constructors in order
        /// to create the fake (for example, because one or more constructors throw exceptions and must be bypassed),
        /// the <c>action</c> will be called more than once, so it should be side effect-free.
        /// </para>
        /// </remarks>
        IFakeOptions<T> ConfigureFake(Action<T> action);

        /// <summary>
        /// Specifies the name of the fake, by which it will be referred to in error messages.
        /// </summary>
        /// <param name="name">The name of the fake.</param>
        /// <returns>Options object.</returns>
        IFakeOptions<T> Named(string name);
    }
}
