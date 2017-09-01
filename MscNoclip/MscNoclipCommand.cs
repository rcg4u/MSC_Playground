using MSCLoader;

namespace MscNoclip
{
    public class MscNoclipCommand : ConsoleCommand
	{
		// What the player has to type into the console to execute your commnad
		public override string Name { get { return "Noclip"; } }

		// The help that's displayed for your command when typing help
		public override string Help { get { return "NoclipMod by haverdaden (DD) | Check RacingDeparment for help"; } }

		// The function that's called when the command is ran
		public override void Run(string[] args)
		{
			ModConsole.Print(args);
		    ModConsole.Print("NoclipMod by haverdaden (DD) | Check RacingDeparment for help");
		}
	}
}
