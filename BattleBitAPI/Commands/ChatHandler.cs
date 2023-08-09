using BattleBitAPI.Common;
using BattleBitAPI.Server;
using System.Text;

namespace BattleBitAPI.Commands
{
    public class ChatCommandHandler {
        private Dictionary<string, Action<string[], object[]>> Commands { get; set; }

        public ChatCommandHandler() {
            this.Commands = new Dictionary<string, Action<string[], object[]>>();
        }

        public void RegisterCommand(string name, Action<string[], object[]> callback) {
            Commands[name.ToLower()] = callback ?? throw new ArgumentNullException(nameof(callback), "Callback parameter cannot be null.");
        }

        public void ExecuteCommand(string input, object[] metaData)
        {
            var player = (Player)metaData[0];
            var chatChannel = (ChatChannel)metaData[1];

            string[] commandSlice = input.Split(' ');

            if (commandSlice.Length == 0) {
                player.Message("Invalid command.");
                return;
            }

            string commandName = commandSlice[0].Substring(0).ToLower();
            string[] args = commandSlice.Skip(1).ToArray();

            if (Commands.TryGetValue(commandName, out Action<string[], object[]> callback)) {
                callback(args, metaData);
            } else {
                player.Message("Invalid command.");
            }
        }
    }
}