using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMonopoly.Services
{
	/// <summary>
	/// Класс, дополняющий автогенерируемый фреймворком код модели Коробки из базы данных
	/// </summary>
	public partial class Boxes : BaseShelfObject
	{
		/// <summary>
		/// Установленный в ТЗ срок годности коробок в днях
		/// </summary>
		private readonly int LifeSpanInDays = 100;

		/// <summary>
		/// Контруктор коробки по умолчанию.
		/// </summary>
		public Boxes() { }

		/// <summary>
		/// Параметризированный контруктор коробки.
		/// </summary>
		/// <param name="width">длина</param>
		/// <param name="height">ширина</param>
		/// <param name="depth">глубина</param>
		/// <param name="weight">вес</param>
		/// <param name="productionDate">дата производства</param>
		public Boxes(double width, double height, double depth, double weight, DateTime productionDate)
		{
			Width = width;
			Height = height;
			Depth = depth;
			Weight = weight;
			ProductionDate = productionDate;
		}

		/// <summary>
		/// Получение даты истечения срока годности коробки по дате производства.
		/// </summary>
		/// <returns>Срок годности.</returns>
		public override DateTime? GetExpirationDate()
		{
			if (!ProductionDate.HasValue) return null;

			return ProductionDate.Value.AddDays(LifeSpanInDays);
		}

        /// <summary>
        /// Установка даты производства по известной дате истечения срока годности коробки.
        /// </summary>
        /// <param name="expirationDate">дата истечения срока годности коробки</param>
        public void SetProductionDate(DateTime expirationDate)
		{
			ProductionDate = expirationDate.AddDays(-LifeSpanInDays);
		}

		/// <summary>
		/// Получение объема коробки.
		/// </summary>
		/// <returns>Объем коробки.</returns>
		public override double GetVolume()
		{
			return Width * Height * Depth;
		}
	}
}
