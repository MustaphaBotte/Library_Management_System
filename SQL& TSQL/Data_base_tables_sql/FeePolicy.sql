create table FeePolicy
(
FeePolicyID int primary key identity,
BookID int not null,
FeeChargePerDay decimal(5,2) default(0.0),
LateFeePerDay decimal(5,2) default(0.0),
DamageFeeRate int  null default(0) , --Percentage 0 to 100 %
FOREIGN KEY (BookID) REFERENCES Books(BookID),

)
