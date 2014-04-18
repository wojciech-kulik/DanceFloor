using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationServices
{
    public class SongsService : ISongsService
    {

        public IReadOnlyCollection<ISong> GetAllSongs()
        {
            var songs = new List<ISong>();

            //TODO: replace it
            for (int i = 0; i < 20; i++)
            {
                var song = new Song()
                {
                    Title = "Sdsadsadsadsas sda sadsa a",
                    Artist = "Shakira" + i.ToString(),
                    BackgroundPath = "../Images/game_background.jpg",
                    FilePath = @"Utwory\Billy Talent - Diamond on a Landmine.mp" + i.ToString()
                };
                songs.Add(song);
            }
            return songs;
        }
    }
}
