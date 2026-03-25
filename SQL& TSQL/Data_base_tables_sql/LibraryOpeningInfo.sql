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



INSERT INTO LibraryOpeningInfo (LibraryID, WeekDay, IsOpen, OpeningHour, ClosinggHour)
VALUES
    (2, 1, 1, '08:00:00', '20:00:00'),  -- Monday
    (2, 2, 1, '08:00:00', '20:00:00'),  -- Tuesday
    (2, 3, 1, '08:00:00', '20:00:00'),  -- Wednesday
    (2, 4, 1, '08:00:00', '20:00:00'),  -- Thursday
    (2, 5, 1, '08:00:00', '17:00:00'),  -- Friday (shorter)
    (2, 6, 1, '09:00:00', '14:00:00'),  -- Saturday (shorter)
    (2, 7, 0, NULL,        NULL);         -- Sunday (closed)
