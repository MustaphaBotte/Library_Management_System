create table ReservationPolicy
(ReservationPolicyID int primary key identity,
MaxBooksToReserve int ,
MaxDaysToPickTheBook int)


INSERT INTO ReservationPolicy (MaxBooksToReserve, MaxDaysToPickTheBook)
VALUES (3, 3); -- 3 days