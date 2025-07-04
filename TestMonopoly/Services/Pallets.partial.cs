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
    public partial class Pallets : BaseShelfObject
    {
        public static readonly int PalletWeight = 30;

        [NotMapped] // Указывает, что поле не хранится в БД
        public DateTime? ExpirationDate =>
        Boxes?.Where(b => b.ProductionDate != null)
              .Min(b => b.ProductionDate);

        //public Pallets() { }

        public Pallets(double width, double height, double depth)
        {
            Width = width;
            Height = height;
            Depth = depth;
        }

        public static List<Pallets> GetAll()
        {
            PalletsRepo repo = new PalletsRepo();
            var pallets = repo.GetAll();

            return pallets;
        }

        private static List<Pallets> OrderByWeight(List<Pallets> pallets)
        {
            pallets
                .Sort((p1, p2) => p1.GetWeight()
                .CompareTo(p2.GetWeight()));

            return pallets;
        }

        public static List<Pallets> TakeThreeWithMaxExpirationDate(List<Pallets> pallets)
        {
            var palletsTop3 = pallets
                .Where(p => p.Boxes.Count > 0)
                .OrderByDescending(p => p.GetExpirationDate())
                .Take(3);

            return palletsTop3.OrderBy(p => p.GetVolume()).ToList();
        }

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

        public double GetWeight()
        {
            double boxesWeight = 0;

            if (Boxes?.Any() == true)
            {
                boxesWeight = Boxes.Sum(b => b.Weight);
            }

            return boxesWeight + PalletWeight;
        }

        public override DateTime? GetExpirationDate()
        {
            if (Boxes == null || !Boxes.Any())
                return null;

            return this.Boxes
                .Where(b => b.ProductionDate != null)
                .OrderBy(b => b.ProductionDate)
                .FirstOrDefault().GetExpirationDate();
        }

        public DateTime? GetMaxExpirationDate()
        {
            if (Boxes == null || !Boxes.Any())
                return Convert.ToDateTime("10.10.1910");

            return this.Boxes
                .Where(b => b.ProductionDate != null)
                .OrderByDescending(b => b.ProductionDate)
                .FirstOrDefault().GetExpirationDate();
        }

        public void AddBox(Boxes box)
        {
            // Проверка возможности добавления
            if (box.Width > this.Width || box.Depth > this.Depth)
                throw new InvalidOperationException("Коробка не помещается на паллету");

            // Установка связи
            box.Pallet = this.ID; // или box.PalletID = this.ID
            this.Boxes.Add(box);
        }

        public Boxes CreateAndAddBox(double width, double height, double depth, double weight)
        {
            var box = new Boxes(width, height, depth, weight, DateTime.Today);
            this.AddBox(box);
            return box;
        }

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

        public List<Boxes> GetAllBoxes()
        {
            return this.Boxes.ToList<Boxes>();
        }
    }
}
