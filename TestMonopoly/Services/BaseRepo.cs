using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMonopoly.Services
{
    public abstract class BaseRepo<T> where T : BaseShelfObject
    {
        protected BoxesDBEntities db = new BoxesDBEntities();

        public abstract List<T> Initialize();

        public T Add(T item, DbContext dbContext)
        {
            if (item == null)
                throw new ArgumentNullException();

            dbContext.Set<T>().Add(item); // Используем переданный контекст
            dbContext.SaveChanges();
            return item;
        }

        public T Get(int id)
        {
            var items = Initialize();
            return items.Find(p => GetId(p) == id);
        }

        public List<T> GetAll()
        {
            return Initialize();
        }

        public void Remove(int id)
        {
            var itemToDelete = db.Set<T>().Find(id);
            if (itemToDelete != null)
            {
                db.Set<T>().Remove(itemToDelete);
                db.SaveChanges();
            }
        }

        public T Update(T shelfObject)
        {
            if (shelfObject == null)
                throw new ArgumentNullException();

            var items = Initialize();
            int index = items.FindIndex(p => GetId(p) == GetId(shelfObject));
            if (index == -1)
                throw new Exception();

            var itemToUpdate = db.Set<T>().Find(GetId(shelfObject));
            if (itemToUpdate == null)
                throw new Exception();

            itemToUpdate = shelfObject;
            db.Entry(itemToUpdate).State = EntityState.Modified;
            db.SaveChanges();

            return shelfObject;
        }

        protected abstract int GetId(T shelfObject);
    }
}
