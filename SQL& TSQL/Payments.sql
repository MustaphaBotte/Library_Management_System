CREATE TABLE Payments
(
PaymentID int primary key identity,
FeePolicyID int not null,
PaidAt datetime not null,
PaymentMethod nvarchar(10), -- cash , visa , paypap etc 
Amount decimal (5,2) not null,
BorrowID int not null,
foreign key (BorrowID) references Borrows (BorrowID),
foreign key (FeePolicyID) references FeePolicy (FeePolicyID),
)