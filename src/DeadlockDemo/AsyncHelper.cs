using System;
using System.Threading;
using System.Threading.Tasks;

namespace DeadlockDemo
{
	public static class AsyncHelper
	{
		private static readonly TaskFactory TaskFactory = new(CancellationToken.None, TaskCreationOptions.None, TaskContinuationOptions.None, TaskScheduler.Default);

		public static TResult RunSync<TResult>(Func<Task<TResult>> func)
		{
#pragma warning disable CA2008 // Do not create tasks without passing a TaskScheduler
			return TaskFactory
				.StartNew(func)
#pragma warning restore CA2008 // Do not create tasks without passing a TaskScheduler
				.Unwrap()
				.GetAwaiter()
				.GetResult();
		}

		public static void RunSync(Func<Task> func)
		{
#pragma warning disable CA2008 // Do not create tasks without passing a TaskScheduler
			TaskFactory
				.StartNew(func)
#pragma warning restore CA2008 // Do not create tasks without passing a TaskScheduler
				.Unwrap()
				.GetAwaiter()
				.GetResult();
		}
	}
}
