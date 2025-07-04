using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestMonopoly.Services;

namespace TestMonopoly
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var listBox = new ListBox
            {
                Location = new Point(10, 10),
                Size = new Size(this.Width - 30, this.Height - 60),
                ScrollAlwaysVisible = true,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            this.Controls.Add(listBox);

            var pallets = Pallets.GetAll();

            var mountPallets = pallets
                .GroupBy(p => p.GetExpirationDate()?.Month)
                .OrderBy(g => g.Key) // Сортируем группы по месяцам
                .Select(g => new
                {
                    Month = g.Key,
                    Pallets = g.OrderBy(p => p.GetWeight()) // Сортируем по весу внутри группы
                });

            var top3Pallets = Pallets.TakeThreeWithMaxExpirationDate(pallets);

            listBox.Items.Add("=== Три паллеты с наибольшим сроком годности, " +
                "отсортированные по объёму ===");

            foreach (var pallet in top3Pallets)
            {
                string text = ListText(pallet);

                listBox.Items.Add(text);
            }

            // Добавляем пустую строку между группами
            listBox.Items.Add("");

            foreach (var monthGroup in mountPallets)
            {
                // Добавляем заголовок месяца
                listBox.Items.Add($"=== Месяц {monthGroup.Month?.ToString() ?? "Нет данных"} ===");

                foreach (var pallet in monthGroup.Pallets)
                {
                    string text = ListText(pallet);

                    listBox.Items.Add(text);
                }

                // Добавляем пустую строку между группами
                listBox.Items.Add("");
            }
        }

        string ListText(Pallets pallet)
        {
            var boxes = pallet.GetAllBoxes();
            string boxIDs = string.Join(", ",
                boxes.Select(b => $"{b.ID} ({b.GetExpirationDate()?.ToString("dd.MM.yyyy")})"));

            return $"Паллета #{pallet.ID}, " +
                        $"годен до {pallet.GetExpirationDate()?.ToString("dd.MM.yyyy")}, " +
                        $"вес: {pallet.GetWeight()} кг., " +
                        $"объём: {pallet.GetVolume()} м(куб), " +
                        $"{pallet.Boxes.Count} коробка(и): {boxIDs}";
        }
    }
}
