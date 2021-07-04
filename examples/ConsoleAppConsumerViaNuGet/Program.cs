using System;
using AnyOfTypes;

namespace ConsoleAppConsumer
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			X(42);
			X("test");
			Console.WriteLine(new string('-', 50));
		}

		private static void X(AnyOf<int, string> value)
		{
			Console.WriteLine("ToString " + value.ToString());
			Console.WriteLine("IsUndefined " + value.IsUndefined);
			Console.WriteLine("IsFirst " + value.IsFirst);
			Console.WriteLine("IsSecond " + value.IsSecond);

			switch (value.CurrentType)
			{
				case AnyOfType.First:
					Console.WriteLine("AnyOfType = First with value " + value.First);
					break;

				case AnyOfType.Second:
					Console.WriteLine("AnyOfType = Second with value " + value.Second);
					break;

				default:
					Console.WriteLine("???");
					break;
			}
		}
	}
}