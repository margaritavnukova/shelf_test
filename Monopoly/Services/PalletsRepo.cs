using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMonopoly.Services
{
    /// <summary>
    /// Репозиторий для работы с таблицей паллет в базе данных.
    /// </summary>
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
