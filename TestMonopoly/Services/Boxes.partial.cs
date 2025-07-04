using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMonopoly.Services
{
	public partial class Boxes : BaseShelfObject
	{
		private readonly int LifeSpanInDays = 100;

		public Boxes() { }

		public Boxes(double width, double height, double depth, double weight, DateTime productionDate)
		{
			Width = width;
			Height = height;
			Depth = depth;
			Weight = weight;
			ProductionDate = productionDate;
		}


		public override DateTime? GetExpirationDate()
		{
			if (!ProductionDate.HasValue) return null;

			return ProductionDate.Value.AddDays(LifeSpanInDays);
		}

		public void SetProductionDate(DateTime expirationDate)
		{
			ProductionDate = expirationDate.AddDays(-LifeSpanInDays);
		}

		public override double GetVolume()
		{
			return Width * Height * Depth;
		}
	}
}
