using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piscolapp
{
    class Album
    {
        private string name;
        private DateTime created_at;
        private List<Picture> pictures;

        public Album(string name, DateTime created_at, List<Picture> pictures = null)
        {
            this.name = name;
            this.created_at = created_at;
            this.pictures = pictures ?? new List<Picture>();
        }

        public string getName()
        {
            return name;
        }
    }
}
