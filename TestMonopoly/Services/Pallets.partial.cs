using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.LinkLabel;

namespace TestMonopoly.Services
{
    /// <summary>
    /// Класс паллет, содержащий в себе методы создания паллеты, сортировки, проверки коробок и добавления коробок в паллету.
    /// </summary>
    public partial class Pallets : BaseShelfObject
    {
        public static readonly int PalletWeight = 30;

        [NotMapped] // Указывает, что поле не хранится в БД
        public DateTime? ExpirationDate =>
        Boxes?.Where(b => b.ProductionDate != null)
              .Min(b => b.ProductionDate);

        /// <summary>
        /// Конструктор объекта класса паллет с параметрами.
        /// </summary>
        /// <param name="width">длина</param>
        /// <param name="height">ширина</param>
        /// <param name="depth">глубина</param>
        public Pallets(double width, double height, double depth)
        {
            Width = width;
            Height = height;
            Depth = depth;
        }

        /// <summary>
        /// Получение списка всех паллет из базы данных.
        /// </summary>
        /// <returns>Список паллет.</returns>
        public static List<Pallets> GetAll()
        {
            PalletsRepo repo = new PalletsRepo();
            var pallets = repo.GetAll();

            return pallets;
        }

        /// <summary>
        /// Сортировка списка паллет по весу.
        /// </summary>
        /// <param name="pallets">Неотсортированный список паллет.</param>
        /// <returns>Оотсортированный по весу список паллет.</returns>
        private static List<Pallets> OrderByWeight(List<Pallets> pallets)
        {
            pallets
                .OrderBy(p => p.GetWeight()).ToList();

            return pallets;
        }

        /// <summary>
        /// Получение трех паллет с наибольшим сроком годности, отсортированных по объёму.
        /// </summary>
        /// <param name="pallets">Неотсортированный список паллет.</param>
        /// <returns>Три паллеты с коробкой наибольшего срока годности, отсортированные по объёму.</returns>
        public static List<Pallets> TakeThreeWithMaxExpirationDate(List<Pallets> pallets)
        {
            var palletsTop3 = pallets
                .Where(p => p.Boxes.Count > 0)
                .OrderByDescending(p => p.GetMaxExpirationDate())
                .Take(3);

            return palletsTop3.OrderBy(p => p.GetVolume()).ToList();
        }

        /// <summary>
        /// Получение срока годности паллеты.
        /// </summary>
        /// <returns>Срок годности паллеты (срок годности коробки с наименьшим сроком).</returns>
        public override DateTime? GetExpirationDate()
        {
            if (Boxes == null || !Boxes.Any())
                return null;

            return this.Boxes
                .Where(b => b.ProductionDate != null)
                .OrderBy(b => b.ProductionDate)
                .FirstOrDefault().GetExpirationDate();
        }

        /// <summary>
        /// Получение наибольшего срока годности коробки в паллете.
        /// </summary>
        /// <returns>Срок годности коробки с наибольшим сроком.</returns>
        public DateTime? GetMaxExpirationDate()
        {
            if (Boxes == null || !Boxes.Any())
                // Задание минимальной даты.
                return Convert.ToDateTime("10.10.1910");

            return this.Boxes
                .Where(b => b.ProductionDate != null)
                .OrderByDescending(b => b.ProductionDate)
                .FirstOrDefault().GetExpirationDate();
        }

        /// <summary>
        /// Получение объема паллеты по объемам коробок на ней.
        /// </summary>
        /// <returns>Объем паллеты с коробками.</returns>
        public override double GetVolume()
        {
            double boxesVolume = 0;
            double palletVolume = Width * Height * Depth;

            if (Boxes?.Any() == true) 
            {
                boxesVolume = Boxes.Sum(b => b.GetVolume());
            }

            return Math.Round(
                (boxesVolume + palletVolume) / 1000000.0, 3, 
                MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// Получение веса паллеты по весу коробок на ней.
        /// </summary>
        /// <returns>Вес паллеты.</returns>
        public double GetWeight()
        {
            double boxesWeight = 0;

            if (Boxes?.Any() == true)
            {
                boxesWeight = Boxes.Sum(b => b.Weight);
            }

            return boxesWeight + PalletWeight;
        }

        /// <summary>
        /// Добавление коробки на паллету с проверкой вместимости.
        /// </summary>
        /// <param name="box">коробка для добавления</param>
        /// <exception cref="InvalidOperationException">Уведомление о том, что коробка не поместилась в паллету.</exception>
        public void AddBox(Boxes box)
        {
            // Проверка возможности добавления.
            if (box.Width > this.Width || box.Depth > this.Depth)
                throw new InvalidOperationException("Коробка не помещается на паллету");

            // Установка связи.
            box.Pallet = this.ID; 
            this.Boxes.Add(box);
        }

        /// <summary>
        /// Создание коробки и помещение ее в паллету.
        /// </summary>
        /// <param name="width">длина</param>
        /// <param name="height">ширина</param>
        /// <param name="depth">глубина</param>
        /// <param name="weight">вес</param>
        /// <returns>Коробка.</returns>
        public Boxes CreateAndAddBox(double width, double height, double depth, double weight)
        {
            var box = new Boxes(width, height, depth, weight, DateTime.Today);
            this.AddBox(box);
            return box;
        }

        /// <summary>
        /// Проверка, помещаются ли все коробки в паллету.
        /// </summary>
        /// <returns>Булево значение (true - если помещаются).</returns>
        public bool CheckBoxes()
        {
            bool allFits = true;

            foreach(Boxes box in this.Boxes)
            {
                allFits = allFits 
                    && box.Depth < this.Depth
                    && box.Width < this.Width; 
            }

            return allFits;
        }

        /// <summary>
        /// Получение списка всех коробок в паллете.
        /// </summary>
        /// <returns>Список коробок в этой паллете.</returns>
        public List<Boxes> GetAllBoxes()
        {
            return this.Boxes.ToList<Boxes>();
        }
    }
}
