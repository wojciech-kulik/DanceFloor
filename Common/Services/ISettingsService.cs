using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public interface ISettingsService
    {
        IDictionary<PlayerAction, string> GetControlSettings(PlayerID player);

        void SetControlSetting(PlayerID player, PlayerAction action);
    }
}
