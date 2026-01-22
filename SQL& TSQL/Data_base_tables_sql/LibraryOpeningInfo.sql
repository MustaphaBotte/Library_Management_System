create table LibraryOpeningInfo
(
LibraryID int not null,
WeekDay int not null unique check(WeekDay>0 and WeekDay<=7),
IsOpen bit not null,
OpeningHour  time(0) null,
ClosinggHour time(0) null,

foreign key (LibraryID) references Library(LibraryID),
primary key (LibraryID,WeekDay)
)

