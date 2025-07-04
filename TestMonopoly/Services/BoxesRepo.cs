using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMonopoly.Services
{
    /// <summary>
    /// Репозиторий для работы с таблицей коробок в базе данных
    /// </summary>
    public class BoxesRepo : BaseRepo<Boxes>
    {
        public override List<Boxes> Initialize()
        {
            return db.Boxes.ToList<Boxes>();
        }

        protected override int GetId(Boxes shelfObject)
        {
            return shelfObject.ID;
        }
    }
}
