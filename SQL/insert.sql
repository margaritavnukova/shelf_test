﻿INSERT INTO Pallets (Width, Height, Depth)
VALUES
(100, 150, 120),
(120, 150, 120),
(80, 120, 100),
(100, 100, 100),
(120, 120, 120),
(90, 140, 110),
(110, 130, 100),
(95, 125, 105),
(105, 135, 115),
(115, 145, 125),
(85, 115, 95),
(125, 155, 135),
(75, 110, 90),
(130, 160, 140),
(70, 100, 80),
(140, 170, 150),
(65, 95, 75),
(150, 180, 160),
(60, 90, 70),
(160, 190, 170);

INSERT INTO Boxes (Width, Height, Depth, Weight, ProductionDate, Pallet)
VALUES
(30.0, 40.0, 20.0, 5.2, CONVERT(date, '01.07.2025', 104), 1),
(25.0, 35.0, 15.0, 4.1, CONVERT(date, '05.06.2025', 104), 1),
(20.0, 30.0, 10.0, 3.5, CONVERT(date, '10.06.2025', 104), 2),
(35.0, 45.0, 25.0, 6.0, CONVERT(date, '15.01.2025', 104), 2),
(40.0, 50.0, 30.0, 7.2, CONVERT(date, '20.01.2025', 104), 3),
(15.0, 25.0, 5.0, 2.8, CONVERT(date, '25.01.2025', 104), 3),
(45.0, 55.0, 35.0, 8.1, CONVERT(date, '01.02.2025', 104), 4),
(50.0, 60.0, 40.0, 9.5, CONVERT(date, '05.02.2025', 104), 4),
(10.0, 20.0, 5.0, 1.5, CONVERT(date, '10.02.2025', 104), 5),
(55.0, 65.0, 45.0, 10.2, CONVERT(date, '15.02.2025', 104), 5),
(60.0, 70.0, 50.0, 11.8, CONVERT(date, '20.02.2025', 104), 6),
(5.0, 15.0, 5.0, 0.8, CONVERT(date, '25.02.2025', 104), 6),
(65.0, 75.0, 55.0, 12.5, CONVERT(date, '01.03.2025', 104), 7),
(70.0, 80.0, 60.0, 14.0, CONVERT(date, '05.06.2025', 104), 7),
(75.0, 85.0, 65.0, 15.5, CONVERT(date, '10.06.2025', 104), 8),
(80.0, 90.0, 70.0, 17.0, CONVERT(date, '15.03.2025', 104), 8),
(85.0, 95.0, 75.0, 18.5, CONVERT(date, '20.03.2025', 104), 9),
(90.0, 100.0, 80.0, 20.0, CONVERT(date, '25.03.2025', 104), 9),
(95.0, 105.0, 85.0, 21.5, CONVERT(date, '01.04.2025', 104), 10),
(100.0, 110.0, 90.0, 23.0, CONVERT(date, '05.04.2025', 104), 10);