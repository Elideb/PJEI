using System;
using System.Threading;

namespace PJEI.Libraries {

    // Adapted from code in Reactive Extensions (Rx): https://rx.codeplex.com/

    public interface ICancelable : IDisposable {
        /// <summary>
        /// Gets a value that indicates whether the object is disposed.
        /// </summary>
        bool IsDisposed { get; }
    }

    /// <summary>
    /// Provides a set of static methods for creating Disposables.
    /// </summary>
    public static class Disposable {
        /// <summary>
        /// Gets the disposable that does nothing when disposed.
        /// </summary>
        public static IDisposable Empty {
            get { return DefaultDisposable.Instance; }
        }

        /// <summary>
        /// Creates a disposable object that invokes the specified action when disposed.
        /// </summary>
        /// <param name="dispose">Action to run during the first call to <see cref="IDisposable.Dispose"/>. The action is guaranteed to be run at most once.</param>
        /// <returns>The disposable object that runs the given action upon disposal.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="dispose"/> is null.</exception>
        public static IDisposable Create(Action dispose) {
            if (dispose == null)
                throw new ArgumentNullException("dispose");

            return new AnonymousDisposable(dispose);
        }
    }

    /// <summary>
    /// Represents an Action-based disposable.
    /// </summary>
    internal sealed class AnonymousDisposable : ICancelable {
        private volatile Action _dispose;

        /// <summary>
        /// Constructs a new disposable with the given action used for disposal.
        /// </summary>
        /// <param name="dispose">Disposal action which will be run upon calling Dispose.</param>
        public AnonymousDisposable(Action dispose) {
            System.Diagnostics.Debug.Assert(dispose != null);

            _dispose = dispose;
        }

        /// <summary>
        /// Gets a value that indicates whether the object is disposed.
        /// </summary>
        public bool IsDisposed {
            get { return _dispose == null; }
        }

        /// <summary>
        /// Calls the disposal action if and only if the current instance hasn't been disposed yet.
        /// </summary>
        public void Dispose() {
#pragma warning disable 0420
            var dispose = Interlocked.Exchange(ref _dispose, null);
#pragma warning restore 0420
            if (dispose != null) {
                dispose();
            }
        }
    }

    /// <summary>
    /// Represents a disposable that does nothing on disposal.
    /// </summary>
    internal sealed class DefaultDisposable : IDisposable {
        /// <summary>
        /// Singleton default disposable.
        /// </summary>
        public static readonly DefaultDisposable Instance = new DefaultDisposable();

        private DefaultDisposable() {
        }

        /// <summary>
        /// Does nothing.
        /// </summary>
        public void Dispose() {
            // no op
        }
    }
}
