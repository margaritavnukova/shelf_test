using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMonopoly.Services
{
    public abstract class BaseShelfObject
    {
        protected BoxesDBEntities db = new BoxesDBEntities();
        
        public abstract double GetVolume();
        public abstract DateTime? GetExpirationDate();
    }
}
