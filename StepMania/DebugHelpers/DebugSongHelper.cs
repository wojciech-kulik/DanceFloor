//#define DEBUG_TIMING
//#define DEBUG_HIT_TIME

using Common;
using GameLayer;
using StepMania.Constants;
using StepMania.ViewModels;
using StepMania.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace StepMania.DebugHelpers
{
    public class DebugSongHelper
    {
        public static Song GenerateSong(int seconds = 300, Difficulty difficulty = Difficulty.Easy)
        {    
            Random r = new Random();

            Sequence sequence = new Sequence();
            sequence.Difficulty = Difficulty.Easy;

            Song result = new Song();
            result.FilePath = @"Utwory\Billy Talent - Diamond on a Landmine.mp3";
            //result.BackgroundPath = @"C:\Users\Wojciech\Documents\Tapety\crestock-499633-1440x900.jpg";
            result.Duration = new TimeSpan(0, 0, seconds);
            result.Sequences.Add(difficulty, sequence);

            for (int i = 2; i < seconds; i++)
            {
                int count = r.Next(1, 3);
                for (int j = 0; j < count; j++)
                {
                    SeqElemType elemType = (SeqElemType)r.Next(0, 4);
                    sequence.AddElement(new SequenceElement() { Type = elemType, Time = new TimeSpan(0, 0, 0, i, r.Next(200, 1000)) });
                }
            }

            return result;
        }

        public static void AddTimeToNotes(UIElementCollection notes, SeqElemType elemType, double top)
        {
            #if DEBUG_TIMING            

            TextBlock tb = new TextBlock() { FontSize = 20, Text = String.Format("{0:N2}", top / GameUIConstants.PixelsPerSecond) };
            Canvas.SetTop(tb, top);
            switch (elemType)
            {
                case SeqElemType.LeftArrow:
                    Canvas.SetLeft(tb, GameUIConstants.LeftArrowX);
                    break;
                case SeqElemType.DownArrow:
                    Canvas.SetLeft(tb, GameUIConstants.DownArrowX);
                    break;
                case SeqElemType.UpArrow:
                    Canvas.SetLeft(tb, GameUIConstants.UpArrowX);
                    break;
                case SeqElemType.RightArrow:
                    Canvas.SetLeft(tb, GameUIConstants.RightArrowX);
                    break;
            }
            notes.Add(tb);

            #endif
        }

        public static void ShowCurrentTimeInsteadPoints(Storyboard animation, IMusicPlayerService musciPlayerService, GameView view)
        {
            #if DEBUG_TIMING

            new Thread(new ThreadStart(() =>
                {
                    while (true)
                    {
                        if (Application.Current == null)
                            return;

                        Application.Current.Dispatcher.BeginInvoke(new System.Action(() =>
                        {
                            var time = musciPlayerService.CurrentTime;
                            view.p1PointsBar.Points = "S: " + time.TotalSeconds.ToString("N2") + " | A: " + animation.GetCurrentTime().TotalSeconds.ToString("N2");
                        }));
                        Thread.Sleep(50);
                    }
                })).Start();

            #endif
        }

        public static void ShowHitTimeDifferenceInsteadPoints(GameView view, Storyboard animation)
        {          
            #if DEBUG_HIT_TIME
            var currentTime = animation.GetCurrentTime();
            var note = view.p1Notes.Children.OfType<Image>().ToList().FirstOrDefault(n => Math.Abs(((ISequenceElement)n.Tag).Time.TotalSeconds - currentTime.TotalSeconds) < 0.2);

            string hitInfo = note != null ? "TAK" : "NIE";

            if (note != null)
                hitInfo += " " + (((ISequenceElement)note.Tag).Time.TotalSeconds - currentTime.TotalSeconds).ToString("N2");
            view.p1PointsBar.Points = hitInfo;
            #endif
        }

        public static bool HandleKeyPressed(GameViewModel model, GameKeyEvent e)
        {
            #if DEBUG_HIT_TIME || DEBUG_TIMING
            if (e.PlayerAction == PlayerAction.Back)
                return false;
            else if (e.PlayerAction == PlayerAction.Enter)
                model.ResumeGame();
            else
                model.PauseGame();

            return true;
            #else
                return false;
            #endif
        }
    }
}
