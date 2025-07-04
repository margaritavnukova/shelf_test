using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMonopoly.Services
{
    public class PalletsRepo : BaseRepo<Pallets>
    {
        public override List<Pallets> Initialize()
        {
            return db.Pallets
                .Include<Pallets, ICollection<Boxes>>(p => p.Boxes)
                .ToList();
        }

        protected override int GetId(Pallets shelfObject)
        {
            return shelfObject.ID;
        }
    }
}
