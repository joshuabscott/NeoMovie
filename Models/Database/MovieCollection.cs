using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NeoMovie.Models.Database
{
    public class MovieCollection // Linking Table for Movies & Collections
    {
        public int Id { get; set; }
        public int CollectionId { get; set; }
        public int MovieId { get; set; }

        public int Order { get; set; }

        public virtual Collection Collection { get; set; }
        public virtual Movie Movie { get; set; }
    }
}