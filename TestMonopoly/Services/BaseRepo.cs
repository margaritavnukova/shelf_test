using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMonopoly.Services
{
    /// <summary>
    /// Родительский абстрактный класс репозитория для работы с базой данных.
    /// </summary>
    /// <typeparam name="T">Тип сущности из базы данных, определяемый при создании репозитория-наследника.</typeparam>
    public abstract class BaseRepo<T> where T : BaseShelfObject
    {
        protected BoxesDBEntities db = new BoxesDBEntities();

        /// <summary>
        /// Получение всех записей из базы данных.
        /// </summary>
        /// <returns>Список всех объектов из базы.</returns>
        public abstract List<T> Initialize();

        /// <summary>
        /// Добавление объекта в базу данных.
        /// </summary>
        /// <param name="item">объект</param>
        /// <param name="dbContext">контекст базы данных</param>
        /// <returns>Добавленный объект.</returns>
        /// <exception cref="ArgumentNullException">Исключение при передаче пустого параметра.</exception>
        public T Add(T item, DbContext dbContext)
        {
            if (item == null)
                throw new ArgumentNullException();

            // Используем переданный контекст
            dbContext.Set<T>().Add(item); 
            dbContext.SaveChanges();
            return item;
        }

        /// <summary>
        /// Получение объекта из базы данных по идентификатору.
        /// </summary>
        /// <param name="id">идентификатор</param>
        /// <returns>Объект с заданным идентификатором.</returns>
        public T Get(int id)
        {
            var items = Initialize();
            return items.Find(p => GetId(p) == id);
        }

        /// <summary>
        /// Получение списка всех объектов из таблицы.
        /// </summary>
        /// <returns>Список всех объектов.</returns>
        public List<T> GetAll()
        {
            return Initialize();
        }

        /// <summary>
        /// Удаление объекта из базы данных по идентификатору.
        /// </summary>
        /// <param name="id">идентификатор</param>
        public void Remove(int id)
        {
            var itemToDelete = db.Set<T>().Find(id);
            if (itemToDelete != null)
            {
                db.Set<T>().Remove(itemToDelete);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Редактирование объекта базы данных.
        /// </summary>
        /// <param name="shelfObject">объект, который нужно обновить</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Исключение при передаче пустого объекта.</exception>
        /// <exception cref="Exception">Исключение при ошибке редиктирования.</exception>
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

            db.Entry(itemToUpdate).State = EntityState.Modified;
            db.SaveChanges();

            return shelfObject;
        }

        /// <summary>
        /// Получение идентификатора объекта.
        /// </summary>
        /// <param name="shelfObject">объект</param>
        /// <returns>Идентификатор.</returns>
        protected abstract int GetId(T shelfObject);
    }
}
