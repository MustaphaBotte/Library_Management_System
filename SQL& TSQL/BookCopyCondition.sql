
CREATE TABLE dbo.BookCopyCondition (
    ConditionID   TINYINT      NOT NULL PRIMARY KEY,
    ConditionName NVARCHAR(15) NOT NULL UNIQUE
);

INSERT INTO dbo.BookCopyCondition (ConditionID, ConditionName) VALUES
(1, N'New'),
(2, N'LikeNew'),
(3, N'VeryGood'),
(4, N'Good'),
(5, N'Fair'),
(6, N'Poor');
