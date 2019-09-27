using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piscolapp
{
    class Album
    {
        private int Id;
        private string name;
        private DateTime created_at;
        private List<int> pictures;

        public Album(int Id, string name, DateTime created_at, List<int> pictures)
        {
            this.Id = Id;
            this.name = name;
            this.created_at = created_at;
            this.pictures = pictures;
        }

        public string getName()
        {
            return name;
        }

        public int getId()
        {
            return Id;
        }

        public List<int> getPictures()
        {
            return this.pictures;
        }
    }
}
