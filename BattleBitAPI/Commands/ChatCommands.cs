using BattleBitAPI.Common;
using BattleBitAPI.Server;
using BattleBitAPI.Storage;
using System.Text;

namespace BattleBitAPI.Commands {
    public class ChatCommands {
        public static void RegisterCommands(ChatCommandHandler handler) {
            handler.RegisterCommand("kick", KickCommand);
        }

        private static void KickCommand(string[] args, object[] metaData) {
            var player = (Player)metaData[0];
            var server = player.GameServer;
            ulong target = server.FindSteamIdByName(args[0], server);
            var dbContext = new GameDbContext();
            var db = new SqlStorage(dbContext);
            var playerStats = db.GetPlayerStatsOf(player.SteamID);

            // check if player is admin
            if (playerStats.Result.Roles != Roles.Admin && playerStats.Result.Roles != Roles.Moderator) {
                player.Message("You do not have permission to use this command.");
                return;
            }

            if (args.Length == 0) {
                player.Message("Invalid command.");
                return;
            }

            string targetName = args[0];
            string reason = args.Length > 1 ? string.Join(" ", args.Skip(1)) : "No reason provided.";

            server.Kick(target, reason);
            player.Message($"Kicked {targetName} for {reason}.");
        }
    }
}