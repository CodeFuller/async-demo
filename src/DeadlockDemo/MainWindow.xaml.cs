using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace DeadlockDemo
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			var timer = new DispatcherTimer
			{
				Interval = TimeSpan.FromMilliseconds(10),
			};

			timer.Tick += (_, _) => TimeLabel.Content = DateTime.Now.ToString("HH:mm:ss.fff", CultureInfo.InvariantCulture);
			timer.Start();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			var callMethods = new (RadioButton, Func<string>)[]
			{
				(Method1, WaitTaskSynchronously1),
				(Method2, WaitTaskSynchronously2),
				(Method3, WaitTaskSynchronously3),
				(Method4, WaitTaskSynchronously4),
				(Method5, WaitTaskSynchronously5),
				(Method6, WaitTaskSynchronously6),
			};

			Func<string> call = null;
			foreach (var callMethod in callMethods)
			{
				if (callMethod.Item1.IsChecked == true)
				{
					call = callMethod.Item2;
					break;
				}
			}

			if (call == null)
			{
				throw new InvalidOperationException("Call method is not set");
			}

			var result = call();
		}

		private static string WaitTaskSynchronously1()
		{
			return AsyncMethod().Result;
		}

		private static string WaitTaskSynchronously2()
		{
			return AsyncMethod().GetAwaiter().GetResult();
		}

		private static string WaitTaskSynchronously3()
		{
			return AsyncMethod().ConfigureAwait(false).GetAwaiter().GetResult();
		}

		private static string WaitTaskSynchronously4()
		{
			return Task.Run(() => AsyncMethod()).Result;
		}

		private static string WaitTaskSynchronously5()
		{
			return Task.Run(async () => await AsyncMethod()).Result;
		}

		private static string WaitTaskSynchronously6()
		{
			return AsyncMethodWithConfigureAwait().Result;
		}

		private static async Task<string> AsyncMethod()
		{
			await Task.Delay(TimeSpan.FromMilliseconds(1));

			return "Completed";
		}

		private static async Task<string> AsyncMethodWithConfigureAwait()
		{
			await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);

			return "Completed";
		}
	}
}
