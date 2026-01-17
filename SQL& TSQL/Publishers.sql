CREATE TABLE Publisher (
    PublisherID     INT IDENTITY PRIMARY KEY,
    Name            NVARCHAR(200)  NOT NULL,
    Description     NVARCHAR(MAX)  NULL,
    CountryID       INT            NOT NULL,      
    Address         NVARCHAR(400)  NULL,
    CreationDate    DATETIME   NOT NULL DEFAULT GETDATE(),
    WebSiteLink     NVARCHAR(255)  NULL,
    Email           NVARCHAR(254)  NULL,
    PhoneNumber     NVARCHAR(40)   NULL,
    LogoImgPath     NVARCHAR(400)  NULL,
    Status          VARCHAR(20)    NOT NULL  ,
	foreign key(CountryID) references Countries(CountryID)
);
create unique index IDX_Publisher_email on
Publisher(Email) where Email is not null

create unique index IDX_Publisher_PhoneNumber on
Publisher(PhoneNumber) where PhoneNumber is not null