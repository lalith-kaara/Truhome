CREATE TABLE Customer (
    Id SERIAL,  -- Primary key, constraint added separately
    FirstName VARCHAR(100),
    MiddleName VARCHAR(100),
    LastName VARCHAR(100),
    DateOfBirth DATE,
    MobileNumber BIGINT,
    DrivingLicenseNumber VARCHAR(50),
    PassportNumber VARCHAR(50),
    PanNumber VARCHAR(20),
    AadharNumber VARCHAR(20),
    CkycNumber VARCHAR(20),
    VoterId VARCHAR(50),
    FatherFirstName VARCHAR(100),
    FatherMiddleName VARCHAR(100),
    FatherLastName VARCHAR(100),
    HusbandFirstName VARCHAR(100),
    HusbandMiddleName VARCHAR(100),
    HusbandLastName VARCHAR(100)
);

-- Primary Key
ALTER TABLE Customer
ADD CONSTRAINT PK_Customer_Id PRIMARY KEY (Id);


-- Add random data in customer table
INSERT INTO Customer (
    FirstName,
    MiddleName,
    LastName,
    DateOfBirth,
    MobileNumber,
    DrivingLicenseNumber,
    PassportNumber,
    PanNumber,
    AadharNumber,
    CkycNumber,
    VoterId,
    FatherFirstName,
    FatherMiddleName,
    FatherLastName,
    HusbandFirstName,
    HusbandMiddleName,
    HusbandLastName
)
SELECT
    'FirstName' || i,
    'MiddleName' || i,
    'LastName' || i,
    date '1980-01-01' + (random() * 15000)::int, -- Random DOB between 1980 and ~2021
    (7000000000 + round(random() * 999999999))::bigint,
    'DL' || lpad(i::text, 8, '0'),
    'P' || lpad(i::text, 7, '0'),
    'PAN' || lpad(i::text, 7, '0'),
    'AADHAR' || lpad(i::text, 8, '0'),
    'CKYC' || lpad(i::text, 6, '0'),
    'VOTER' || lpad(i::text, 6, '0'),
    'FatherFirst' || i,
    'FatherMiddle' || i,
    'FatherLast' || i,
    'HusbandFirst' || i,
    'HusbandMiddle' || i,
    'HusbandLast' || i
FROM generate_series(1, 200) AS s(i);

-- Customer Request Log Table 

CREATE TABLE CustomerRequestLog (
    ID SERIAL PRIMARY KEY,
    FirstName VARCHAR(255),
    MiddleName VARCHAR(255),
    LastName VARCHAR(255),
    DateOfBirth DATE,
    MobileNumber BIGINT,
    DrivingLicenseNumber VARCHAR(100),
    PassportNumber VARCHAR(100),
    PanNumber VARCHAR(50),
    AadharNumber VARCHAR(100),
    CkycNumber VARCHAR(100),
    VoterId VARCHAR(100),
    FatherFirstName VARCHAR(255),
    FatherMiddleName VARCHAR(255),
    FatherLastName VARCHAR(255),
    HusbandFirstName VARCHAR(255),
    HusbandMiddleName VARCHAR(255),
    HusbandLastName VARCHAR(255),
    SystemOrigin VARCHAR(300),
    CorrelationId VARCHAR(50)
    --XIpAddress (might add request ip address as well later) 
);

-- logs table for system logs.

CREATE TABLE Logs (
    Id SERIAL PRIMARY KEY,
    Level VARCHAR(50) NOT NULL,
    Message TEXT NOT NULL,
    CorrelationId UUID,
    LoggedAt TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);


-- scafolding command:

dotnet ef dbcontext scaffold "Host=gondola.proxy.rlwy.net;Port=27894;Database=truhome;Username=postgres;Password=zjpWJkONqJGCLBkHkhBqJzofJtUvuhgk;SSL Mode=Require;Trust Server Certificate=true" Npgsql.EntityFrameworkCore.PostgreSQL -o Entities -c TruhomeDbContext --context-dir Contexts --force


-- once scaffolding is done, remove connection string from context file and add it to appsettings.json