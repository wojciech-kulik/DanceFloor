using Caliburn.Micro;
using Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ApplicationServices
{
    public class SettingsService : ISettingsService
    {
        List<Key> gameKeys = new List<Key>() { Key.Left, Key.Down, Key.Up, Key.Right, Key.W, Key.A, Key.S, Key.D };
        List<Key> navigationKeys = new List<Key>() { Key.Escape, Key.Enter };

        IEventAggregator _eventAggregator;

        public SettingsService(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        //you need to attach this to your PreviewKeyUp event in application        
        public void HandleKeyUp(object sender, KeyEventArgs e)
        {
            if (navigationKeys.Contains(e.Key))
            {
                GameAction gameAction = GameAction.Resume;
                switch(e.Key)
                {
                    case Key.Escape:
                        gameAction = GameAction.Pause;
                        break;
                    case Key.Enter:
                        gameAction = GameAction.Resume;
                        break;
                }

                _eventAggregator.Publish(new GameActionEvent
                {
                    GameAction = gameAction
                });
                return;
            }

            if (gameKeys.Contains(e.Key))
            {
                _eventAggregator.Publish(new GameKeyEvent
                {
                    PlayerId = ControlHelper.KeyToPlayerID(e.Key),
                    PlayerAction = ControlHelper.KeyToPlayerAction(e.Key)
                });
            }
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
