using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ApplicationServices
{
    public class SettingsService : ISettingsService
    {       
        public event GameKeyEventHandler GameKeyPressed;

        //you need to attach this to your PreviewKeyUp event in application
        List<Key> supportedKeys = new List<Key>() { Key.Left, Key.Down, Key.Up, Key.Right, Key.W, Key.A, Key.S, Key.D };
        public void HandleKeyUp(object sender, KeyEventArgs e)
        {
            if (GameKeyPressed != null && supportedKeys.Contains(e.Key))
                GameKeyPressed(sender, ControlHelper.KeyToPlayerID(e.Key), ControlHelper.KeyToPlayerAction(e.Key));
        }

        public IDictionary<PlayerAction, string> GetControlSettings(PlayerID player)
        {
            //TODO: implement
            return new Dictionary<PlayerAction, string>();
        }

        public void SetControlSetting(PlayerID player, PlayerAction action)
        {
            //TODO: implement
            //intercept key
        }
    }
}
