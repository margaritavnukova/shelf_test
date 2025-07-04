using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMonopoly.Services
{
    /// <summary>
    /// Родительский абстрактный класс для всех объектов на складе.
    /// </summary>
    public abstract class BaseShelfObject
    {
        protected BoxesDBEntities db = new BoxesDBEntities();
        
        /// <summary>
        /// Получение объема коробки или паллеты.
        /// </summary>
        /// <returns>Объем.</returns>
        public abstract double GetVolume();

        /// <summary>
        /// Получение даты истечения срока годности.
        /// </summary>
        /// <returns>Срок годности.</returns>
        public abstract DateTime? GetExpirationDate();
    }
}
