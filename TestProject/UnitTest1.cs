using TestMonopoly.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestProject
{
    [TestClass]
    public class BoxesUnitTests
    {
        /// <summary>
        /// Тест на возвращение пустой даты при отсутствии коробок на паллете.
        /// </summary>
        [TestMethod]
        public void GetExpirationDate_WhenNoBoxes_ReturnsNull()
        {
            // Arrange
            var pallet = new Pallets
            {
                // Пустая коллекция
                Boxes = new List<Boxes>()
            };

            // Act
            var result = pallet.GetExpirationDate();

            // Assert
            Assert.IsNull(result, "Должен возвращать null при отсутствии коробок");
        }

        /// <summary>
        /// Тестирование метода GetExpirationDate на прибавление 100 дней к дате производства коробки
        /// </summary>
        [TestMethod]
        public void GetExpirationDate_ForABox()
        {
            // Arrange
            var box = new Boxes
            {
                ProductionDate = new DateTime(2024, 10, 10),
            };

            // Act
            var result = box.GetExpirationDate();

            // Assert
            var assertingResult = new DateTime(2024, 10, 10).AddDays(100);
            Assert.AreEqual(result, assertingResult, "Должны быть равны");
        }
    }

    [TestClass]
    public class PalletServiceTests
    {
        /// <summary>
        /// Тестирование метода сортировки и получения топ3 коробок по дате и объему
        /// </summary>
        [TestMethod]
        public void TakeThreeWithMaxExpirationDate_WhenMoreThanThreePallets_ReturnsTop3SortedByVolume()
        {
            // Arrange
            var pallets = new List<Pallets>
                {
                    CreatePalletWithBoxes(1, new DateTime(2023, 12, 31), 10, 10, 10), // Объем 1000.
                    CreatePalletWithBoxes(2, new DateTime(2024, 01, 15), 5, 5, 5),    // Объем 125.
                    CreatePalletWithBoxes(3, new DateTime(2024, 02, 20), 8, 8, 8),    // Объем 512.
                    CreatePalletWithBoxes(4, new DateTime(2023, 11, 30), 7, 7, 7)     // Объем 343.
                };

            // Act
            var result = Pallets.TakeThreeWithMaxExpirationDate(pallets);

            // Assert
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(2, result[0].ID); // Должен быть первым, т.к. самый маленький объем из топ-3.
            Assert.AreEqual(3, result[1].ID);
            Assert.AreEqual(1, result[2].ID);
        }
        
        /// <summary>
        /// Тестирование метода, который должен возвращать пустой список, если нечего сортировать
        /// </summary>
        [TestMethod]
        public void TakeThreeWithMaxExpirationDate_WhenEmptyList_ReturnsEmptyList()
        {
            // Arrange
            var pallets = new List<Pallets>();

            // Act
            var result = Pallets.TakeThreeWithMaxExpirationDate(pallets);

            // Assert
            Assert.AreEqual(0, result.Count);
        }

        /// <summary>
        /// Тестирование метода топ3, так, чтобы в список не была включена паллета без коробок
        /// </summary>
        [TestMethod]
        public void TakeThreeWithMaxExpirationDate_WhenPalletsWithoutBoxes_ExcludesThem()
        {
            // Arrange
            var pallets = new List<Pallets>
                {
                    new Pallets { ID = 1, Boxes = new List<Boxes>() }, // Без коробок
                    CreatePalletWithBoxes(2, new DateTime(2024, 01, 15)),
                    CreatePalletWithBoxes(3, new DateTime(2024, 02, 20))
                };

            // Act
            var result = Pallets.TakeThreeWithMaxExpirationDate(pallets);

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.IsFalse(result.Any(p => p.ID == 1));
        }

        /// <summary>
        /// Специальный метод создания паллеты с коробками для упрощения тестирования.
        /// </summary>
        /// <param name="id">идентификатор паллеты</param>
        /// <param name="expirationDate">срок годности</param>
        /// <param name="width">длина</param>
        /// <param name="height">ширина</param>
        /// <param name="depth">глубина</param>
        /// <returns>Паллета с коробками.</returns>
        private Pallets CreatePalletWithBoxes(int id, DateTime expirationDate,
            int width = 10, int height = 10, int depth = 10)
        {
            var pallet = new Pallets
            {
                ID = id,
                Width = width,
                Height = height,
                Depth = depth
            };

            pallet.Boxes.Add(new Boxes
            {
                ProductionDate = expirationDate.AddDays(-100) // Дата производства = срок годности - 100 дн.
            });

            return pallet;
        }
    }
}

