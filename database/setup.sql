CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) NOT NULL,
    `ProductVersion` varchar(32) NOT NULL,
    PRIMARY KEY (`MigrationId`)
);

START TRANSACTION;

CREATE TABLE `AspNetRoles` (
    `Id` nvarchar(450) NOT NULL,
    `Name` nvarchar(256) NULL,
    `NormalizedName` nvarchar(256) NULL,
    `ConcurrencyStamp` nvarchar(max) NULL,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `AspNetUsers` (
    `Id` nvarchar(450) NOT NULL,
    `FirstName` nvarchar(max) NULL,
    `LastName` nvarchar(max) NULL,
    `JobTitle` nvarchar(max) NULL,
    `UserName` nvarchar(256) NULL,
    `NormalizedUserName` nvarchar(256) NULL,
    `Email` nvarchar(256) NULL,
    `NormalizedEmail` nvarchar(256) NULL,
    `EmailConfirmed` bit NOT NULL,
    `PasswordHash` nvarchar(max) NULL,
    `SecurityStamp` nvarchar(max) NULL,
    `ConcurrencyStamp` nvarchar(max) NULL,
    `PhoneNumber` nvarchar(max) NULL,
    `PhoneNumberConfirmed` bit NOT NULL,
    `TwoFactorEnabled` bit NOT NULL,
    `LockoutEnd` datetimeoffset NULL,
    `LockoutEnabled` bit NOT NULL,
    `AccessFailedCount` int NOT NULL,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `Dashboards` (
    `DashboardID` int NOT NULL,
    `UserName` nvarchar(max) NULL,
    `DashboardText` nvarchar(max) NULL,
    PRIMARY KEY (`DashboardID`)
);

CREATE TABLE `Groups` (
    `GroupID` int NOT NULL,
    `GroupName` int NOT NULL,
    PRIMARY KEY (`GroupID`)
);

CREATE TABLE `Observations` (
    `ObservationID` int NOT NULL,
    `UserName` nvarchar(max) NULL,
    `StudentID` int NULL,
    `StudentName` nvarchar(max) NULL,
    `Description` nvarchar(max) NULL,
    `Severity` int NOT NULL,
    `ObservationDate` datetime2 NOT NULL,
    `Action` nvarchar(max) NULL,
    `Event` nvarchar(max) NULL,
    PRIMARY KEY (`ObservationID`)
);

CREATE TABLE `Reports` (
    `ReportID` int NOT NULL,
    `ReportName` nvarchar(max) NULL,
    `Query` nvarchar(max) NULL,
    PRIMARY KEY (`ReportID`)
);

CREATE TABLE `Students` (
    `StudentID` int NOT NULL,
    `LastName` nvarchar(max) NULL,
    `FirstName` nvarchar(max) NULL,
    `MiddleName` nvarchar(max) NULL,
    `DOB` datetime2 NOT NULL,
    `ExternalID` int NOT NULL,
    PRIMARY KEY (`StudentID`)
);

CREATE TABLE `Supporters` (
    `SupporterID` int NOT NULL,
    `GroupID` int NOT NULL,
    `UserName` nvarchar(max) NULL,
    PRIMARY KEY (`SupporterID`)
);

CREATE TABLE `AspNetRoleClaims` (
    `Id` int NOT NULL,
    `RoleId` nvarchar(450) NOT NULL,
    `ClaimType` nvarchar(max) NULL,
    `ClaimValue` nvarchar(max) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_AspNetRoleClaims_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `AspNetUserClaims` (
    `Id` int NOT NULL,
    `UserId` nvarchar(450) NOT NULL,
    `ClaimType` nvarchar(max) NULL,
    `ClaimValue` nvarchar(max) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_AspNetUserClaims_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `AspNetUserLogins` (
    `LoginProvider` nvarchar(450) NOT NULL,
    `ProviderKey` nvarchar(450) NOT NULL,
    `ProviderDisplayName` nvarchar(max) NULL,
    `UserId` nvarchar(450) NOT NULL,
    PRIMARY KEY (`LoginProvider`, `ProviderKey`),
    CONSTRAINT `FK_AspNetUserLogins_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `AspNetUserRoles` (
    `UserId` nvarchar(450) NOT NULL,
    `RoleId` nvarchar(450) NOT NULL,
    PRIMARY KEY (`UserId`, `RoleId`),
    CONSTRAINT `FK_AspNetUserRoles_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_AspNetUserRoles_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `AspNetUserTokens` (
    `UserId` nvarchar(450) NOT NULL,
    `LoginProvider` nvarchar(450) NOT NULL,
    `Name` nvarchar(450) NOT NULL,
    `Value` nvarchar(max) NULL,
    PRIMARY KEY (`UserId`, `LoginProvider`, `Name`),
    CONSTRAINT `FK_AspNetUserTokens_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `Delegations` (
    `DelegationID` int NOT NULL,
    `GroupID` int NOT NULL,
    `StudentID` int NOT NULL,
    PRIMARY KEY (`DelegationID`),
    CONSTRAINT `FK_Delegations_Groups_GroupID` FOREIGN KEY (`GroupID`) REFERENCES `Groups` (`GroupID`) ON DELETE CASCADE,
    CONSTRAINT `FK_Delegations_Students_StudentID` FOREIGN KEY (`StudentID`) REFERENCES `Students` (`StudentID`) ON DELETE CASCADE
);

CREATE INDEX `IX_AspNetRoleClaims_RoleId` ON `AspNetRoleClaims` (`RoleId`);

CREATE UNIQUE INDEX `RoleNameIndex` ON `AspNetRoles` (`NormalizedName`);

CREATE INDEX `IX_AspNetUserClaims_UserId` ON `AspNetUserClaims` (`UserId`);

CREATE INDEX `IX_AspNetUserLogins_UserId` ON `AspNetUserLogins` (`UserId`);

CREATE INDEX `IX_AspNetUserRoles_RoleId` ON `AspNetUserRoles` (`RoleId`);

CREATE INDEX `EmailIndex` ON `AspNetUsers` (`NormalizedEmail`);

CREATE UNIQUE INDEX `UserNameIndex` ON `AspNetUsers` (`NormalizedUserName`);

CREATE INDEX `IX_Delegations_GroupID` ON `Delegations` (`GroupID`);

CREATE INDEX `IX_Delegations_StudentID` ON `Delegations` (`StudentID`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20210914031224_Initial', '5.0.9');

COMMIT;

