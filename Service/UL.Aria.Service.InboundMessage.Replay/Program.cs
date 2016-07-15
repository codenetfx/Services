namespace UL.Aria.Service.InboundMessage.Replay
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var replay = new Replay();
			replay.Start(args);
		}
	}
}