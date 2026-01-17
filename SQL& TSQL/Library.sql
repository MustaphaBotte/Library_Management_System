
CREATE TABLE Library
(
    LibraryID INT IDENTITY(1,1) PRIMARY KEY,          -- PK
    LibraryName NVARCHAR(100) NOT NULL,               -- Name of the library
    Email NVARCHAR(255),                  -- Optional email, unique
    Address NVARCHAR(255)  NULL,                   -- Physical address
	CountryID int not null,
    FixPhoneNumber NVARCHAR(15) NOT NULL,             -- Landline phone
    WhatsAppPhoneNumber NVARCHAR(15) NULL,            -- WhatsApp contact
    WebSiteLink NVARCHAR(255) NULL,                   -- Website URL
    OpeningInfoID INT NOT NULL,                       -- FK to OpeningInfo table
    ReservationPolicyID INT NOT NULL,                 -- FK to ReservationPolicy table
    LogoImgPath NVARCHAR(500) NULL,                    -- Path to logo image

    foreign key (ReservationPolicyID) references ReservationPolicy(ReservationPolicyID),
	foreign key (CountryID) references Countries(CountryID)

);

create unique index IX_NonNullLibraryEmail on
Library(Email) where Email is not null
