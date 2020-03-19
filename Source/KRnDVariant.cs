using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRnD
{
    // This class is used to store all relevant variant stats (currently mass only) of a part used to calculate all other stats with
    // incrementel upgrades
    public class KRnDVariant
    {
        public string name;
        public float mass;
        public float origMass;

        public KRnDVariant(string name, float mass)
        {
            this.name = name;
            this.mass = mass;
            origMass = mass;
        }
        public void UpdateMass(float adjust)
        {
            this.mass = origMass * adjust;
        }
    }
}
