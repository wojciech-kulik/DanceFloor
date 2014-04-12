using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationServices
{
    public class SettingsService : ISettingsService
    {
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
