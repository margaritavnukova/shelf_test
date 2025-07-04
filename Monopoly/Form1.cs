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
    /// <summary>
    /// Основная оконная форма вывода данных.
    /// </summary>
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        ListBox listBox = new ListBox();

        private void Form1_Load(object sender, EventArgs e)
        {
            listBox = new ListBox
            {
                Location = new Point(10, 10),
                Size = new Size(this.Width - 30, this.Height - 60),
                ScrollAlwaysVisible = true,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Top 
                    | AnchorStyles.Left | AnchorStyles.Right
            };
            this.Controls.Add(listBox);

            PrintAllPallets();
        }

        /// <summary>
        /// Вывод всех паллет в список.
        /// </summary>
        void PrintAllPallets()
        {
            listBox.Items.Clear();

            var pallets = Pallets.GetAll();

            var mountPallets = pallets
                .GroupBy(p => p.GetExpirationDate()?.Month)
                // Сортировка группы по месяцам.
                .OrderBy(g => g.Key) 
                .Select(g => new
                {
                    Month = g.Key,
                    // Сортировка по весу внутри группы.
                    Pallets = g.OrderBy(p => p.GetWeight())
                });

            // Получение трех паллет с наибольшим сроком для отдельного вывода.
            var top3Pallets = Pallets.TakeThreeWithMaxExpirationDate(pallets);

            listBox.Items.Add(
                "=== Три паллеты, содержащие коробки " +
                "с наибольшим сроком годности, " +
                "отсортированные по объёму ===");

            foreach (var pallet in top3Pallets)
            {
                string text = ListText(pallet);

                listBox.Items.Add(text);
            }

            foreach (var monthGroup in mountPallets)
            {
                // Добавление пустой строки между группами
                listBox.Items.Add("");

                // Добавление заголовка месяца
                listBox.Items.Add($"=== Месяц {monthGroup.Month?.ToString() ?? "Нет данных"} ===");

                foreach (var pallet in monthGroup.Pallets)
                {
                    string text = ListText(pallet);

                    listBox.Items.Add(text);
                }
            }
        }

        /// <summary>
        /// Заполенение строки информацией о паллете.
        /// </summary>
        /// <param name="pallet">паллета, которую нужно вывести в строке</param>
        /// <returns>Строка с данными о паллете.</returns>
        string ListText(Pallets pallet)
        {
            var boxes = pallet.GetAllBoxes();
            string boxIDs = string.Join(", ",
                boxes.Select(b => $"{b.ID} ({b.GetExpirationDate()?.ToString("dd.MM.yyyy")})"));

            return $"Паллета #{pallet.ID}, " +
                        $"годна до {pallet.GetExpirationDate()?.ToString("dd.MM.yyyy")}, " +
                        $"вес: {pallet.GetWeight()} кг., " +
                        $"объём: {pallet.GetVolume()} м(куб), " +
                        $"{pallet.Boxes.Count} коробка(и): {boxIDs}";
        }
    }
}
