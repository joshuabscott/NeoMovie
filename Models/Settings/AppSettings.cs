using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NeoMovie.Models.Settings
{
    public class AppSettings
    {
        public NeoMovieSettings NeoMovieSettings { get; set; }

        public TMDBSettings TMDBSettingsSettings { get; set; }
    }
}