using System;
using System.Threading.Tasks;

public static class TaskExtensions
{
	/// <summary>
	/// Blocks until condition is true or timeout occurs.
	/// </summary>
	/// <param name="condition">The condition that will perpetuate the block.</param>
	/// <param name="frequency">The frequency at which the condition will be check, in milliseconds.</param>
	/// <param name="timeout">Timeout in milliseconds.</param>
	/// <exception cref="TimeoutException"></exception>
	/// <returns></returns>
	public static async Task WaitUntil(Func<bool> condition, int frequency = 25, int timeout = -1)
	{
		var waitTask = Task.Run(async () =>
		{
			while (!condition())
			{
				await Task.Delay(frequency);
			}
		});

		if (waitTask != await Task.WhenAny(waitTask, Task.Delay(timeout)))
		{
			throw new TimeoutException();
		}
	}
}