
CREATE TABLE dbo.GeneralStatus (
    StatusID     INT           NOT NULL PRIMARY KEY,
    StatusName   NVARCHAR(15)  NOT NULL UNIQUE,
);

INSERT INTO dbo.GeneralStatus (StatusID, StatusName) VALUES
(1, N'Pending'),
(2, N'Active'),
(3, N'Cancelled'),
(4, N'Expired');



