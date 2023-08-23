using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Handball.Models
{
    public class ForwardWing : Player
    {
        public ForwardWing(string name) : base(name, 5.5) { }

        public override void IncreaseRating()
        {
            this.rating += 1.25;
            if (this.rating > 10)
            {
                this.rating = 10;
            }
        }

        public override void DecreaseRating()
        {
            this.rating -= 0.75;
            if (this.rating < 1)
            {
                this.rating = 1;
            }
        }
    }
}
