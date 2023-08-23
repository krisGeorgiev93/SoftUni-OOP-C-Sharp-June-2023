using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Handball.Models
{
    public class CenterBack : Player
    {
        public CenterBack(string name) : base(name, 4) { }

        public override void IncreaseRating()
        {
            this.rating += 1;
            if (this.rating > 10)
            {
                this.rating = 10;
            }
        }

        public override void DecreaseRating()
        {
            this.rating -= 1;
            if (this.rating < 1)
            {
                this.rating = 1;
            }
        }
    }
}
