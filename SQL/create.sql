create table Pallets 
(
	ID int identity(1,1) primary key not null,
	Width float not null,
	Height float not null,
	Depth float not null
)


create table Boxes
(
	ID int identity(1,1) primary key not null,
	Width float not null,
	Height float not null,
	Depth float not null,
	Weight float,
	ProductionDate DateTime,
	Pallet int foreign key references Pallets(ID)
)