using BattleBitAPI;
using BattleBitAPI.Common;
using BattleBitAPI.Server;
using BattleBitAPI.Storage;
using BattleBitAPI.Commands;
using System.Numerics;

class Program
{
    private static readonly ChatCommandHandler commandHandler = new();
    static void Main(string[] args)
    {
        var listener = new ServerListener<MyPlayer>();
        listener.OnGameServerTick += OnGameServerTick;
        listener.OnPlayerTypedMessage += OnPlayerMessage;
        listener.Start(29294);//Port
        Thread.Sleep(-1);
    }

    private static async Task OnGameServerTick(GameServer server)
    {
        //server.Settings.SpectatorEnabled = !server.Settings.SpectatorEnabled;
        //server.MapRotation.AddToRotation("DustyDew");
        //server.MapRotation.AddToRotation("District");
        //server.GamemodeRotation.AddToRotation("CONQ");
        //server.ForceEndGame();
    }

    private static async Task OnPlayerMessage(Player player, ChatChannel channel, string message) {
        if (message.StartsWith("!")) {
            commandHandler.ExecuteCommand(message.Substring(1), new object[] { player, channel });
        }
    }

}
class MyPlayer : Player
{
    
}
