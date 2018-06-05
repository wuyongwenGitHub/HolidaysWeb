
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 05/30/2018 19:29:19
-- Generated from EDMX file: F:\Myproject\hday\WebSite\HolidayWeb\Holidays.Model\Entites\HolidaysEntites.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [HolidaysWebMSSQL];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_UserRole_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserRole] DROP CONSTRAINT [FK_UserRole_User];
GO
IF OBJECT_ID(N'[dbo].[FK_UserRole_Role]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserRole] DROP CONSTRAINT [FK_UserRole_Role];
GO
IF OBJECT_ID(N'[dbo].[FK_PermissionRole_Permission]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PermissionRole] DROP CONSTRAINT [FK_PermissionRole_Permission];
GO
IF OBJECT_ID(N'[dbo].[FK_PermissionRole_Role]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PermissionRole] DROP CONSTRAINT [FK_PermissionRole_Role];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[AdminUser]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AdminUser];
GO
IF OBJECT_ID(N'[dbo].[CarInfo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CarInfo];
GO
IF OBJECT_ID(N'[dbo].[HomeData]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HomeData];
GO
IF OBJECT_ID(N'[dbo].[HouseConfigure]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HouseConfigure];
GO
IF OBJECT_ID(N'[dbo].[HouseEvaluate]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HouseEvaluate];
GO
IF OBJECT_ID(N'[dbo].[HouseImg]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HouseImg];
GO
IF OBJECT_ID(N'[dbo].[HouseInfo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HouseInfo];
GO
IF OBJECT_ID(N'[dbo].[OrderInfo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[OrderInfo];
GO
IF OBJECT_ID(N'[dbo].[OrderItem]', 'U') IS NOT NULL
    DROP TABLE [dbo].[OrderItem];
GO
IF OBJECT_ID(N'[dbo].[ProductCategory]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductCategory];
GO
IF OBJECT_ID(N'[dbo].[ProductInfo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductInfo];
GO
IF OBJECT_ID(N'[dbo].[Region]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Region];
GO
IF OBJECT_ID(N'[dbo].[SpotInfo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SpotInfo];
GO
IF OBJECT_ID(N'[dbo].[SystemConfig]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SystemConfig];
GO
IF OBJECT_ID(N'[dbo].[TenantInfo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TenantInfo];
GO
IF OBJECT_ID(N'[dbo].[UserAccount]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserAccount];
GO
IF OBJECT_ID(N'[dbo].[UserFavorite]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserFavorite];
GO
IF OBJECT_ID(N'[dbo].[UserInfo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserInfo];
GO
IF OBJECT_ID(N'[dbo].[UserInfoCertificate]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserInfoCertificate];
GO
IF OBJECT_ID(N'[dbo].[UserInfoExt]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserInfoExt];
GO
IF OBJECT_ID(N'[dbo].[UserOrderRecord]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserOrderRecord];
GO
IF OBJECT_ID(N'[dbo].[HouseInfoView]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HouseInfoView];
GO
IF OBJECT_ID(N'[dbo].[OrderInfoView]', 'U') IS NOT NULL
    DROP TABLE [dbo].[OrderInfoView];
GO
IF OBJECT_ID(N'[dbo].[UserFavoriteView]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserFavoriteView];
GO
IF OBJECT_ID(N'[dbo].[UserInfoCertificateView]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserInfoCertificateView];
GO
IF OBJECT_ID(N'[dbo].[UserInfoExtView]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserInfoExtView];
GO
IF OBJECT_ID(N'[dbo].[UserInfoView]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserInfoView];
GO
IF OBJECT_ID(N'[dbo].[ShopCategory]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ShopCategory];
GO
IF OBJECT_ID(N'[dbo].[ShopInfo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ShopInfo];
GO
IF OBJECT_ID(N'[dbo].[ShopInfoCertificate]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ShopInfoCertificate];
GO
IF OBJECT_ID(N'[dbo].[ShopInfoCertificateView]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ShopInfoCertificateView];
GO
IF OBJECT_ID(N'[dbo].[HouseComment]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HouseComment];
GO
IF OBJECT_ID(N'[dbo].[HouseEvaluateView]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HouseEvaluateView];
GO
IF OBJECT_ID(N'[dbo].[SysLog]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SysLog];
GO
IF OBJECT_ID(N'[dbo].[T_User]', 'U') IS NOT NULL
    DROP TABLE [dbo].[T_User];
GO
IF OBJECT_ID(N'[dbo].[T_Role]', 'U') IS NOT NULL
    DROP TABLE [dbo].[T_Role];
GO
IF OBJECT_ID(N'[dbo].[T_Permission]', 'U') IS NOT NULL
    DROP TABLE [dbo].[T_Permission];
GO
IF OBJECT_ID(N'[dbo].[ShopToDayPriceSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ShopToDayPriceSet];
GO
IF OBJECT_ID(N'[dbo].[UserRole]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserRole];
GO
IF OBJECT_ID(N'[dbo].[PermissionRole]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PermissionRole];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'AdminUser'
CREATE TABLE [dbo].[AdminUser] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [LoginAccount] varchar(40)  NOT NULL,
    [LoginPassword] varchar(40)  NOT NULL,
    [State] int  NOT NULL,
    [Type] int  NOT NULL,
    [CreateTime] datetime  NOT NULL
);
GO

-- Creating table 'CarInfo'
CREATE TABLE [dbo].[CarInfo] (
    [ID] bigint IDENTITY(1,1) NOT NULL,
    [CarName] nvarchar(50)  NULL,
    [Linkman] nvarchar(20)  NULL,
    [Phone1] nvarchar(50)  NULL,
    [Phone2] nvarchar(50)  NULL,
    [Address] nvarchar(200)  NULL,
    [CarImg] nvarchar(255)  NULL,
    [ProvinceId] int  NULL,
    [CityId] int  NULL
);
GO

-- Creating table 'HomeData'
CREATE TABLE [dbo].[HomeData] (
    [ID] bigint IDENTITY(1,1) NOT NULL,
    [Type] int  NOT NULL,
    [Title] nvarchar(200)  NOT NULL,
    [Url] nvarchar(500)  NULL,
    [ImgUrl] nvarchar(255)  NOT NULL
);
GO

-- Creating table 'HouseConfigure'
CREATE TABLE [dbo].[HouseConfigure] (
    [ID] bigint IDENTITY(1,1) NOT NULL,
    [Type] int  NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [Icon] nvarchar(100)  NULL,
    [Sort] int  NOT NULL,
    [CreateTime] datetime  NOT NULL
);
GO

-- Creating table 'HouseEvaluate'
CREATE TABLE [dbo].[HouseEvaluate] (
    [ID] bigint IDENTITY(1,1) NOT NULL,
    [HouseInfoID] bigint  NOT NULL,
    [UserInfoID] bigint  NOT NULL,
    [UserPhone] nvarchar(20)  NOT NULL,
    [CleanScore] int  NOT NULL,
    [LocationScore] int  NOT NULL,
    [EnvironmentScore] int  NOT NULL,
    [ServiceScore] int  NOT NULL,
    [PerformanceScore] int  NOT NULL,
    [AverageScore] float  NOT NULL,
    [EvaluateContent] varchar(max)  NOT NULL,
    [CreateTime] datetime  NOT NULL,
    [State] int  NOT NULL,
    [HouseTitle] nvarchar(20)  NOT NULL,
    [Username] nvarchar(40)  NOT NULL,
    [UserImg] nvarchar(200)  NULL
);
GO

-- Creating table 'HouseImg'
CREATE TABLE [dbo].[HouseImg] (
    [ID] bigint IDENTITY(1,1) NOT NULL,
    [HouseInfoID] bigint  NOT NULL,
    [ImgUrl] nvarchar(255)  NOT NULL,
    [CreateTime] datetime  NOT NULL
);
GO

-- Creating table 'HouseInfo'
CREATE TABLE [dbo].[HouseInfo] (
    [ID] bigint IDENTITY(1,1) NOT NULL,
    [UserInfoID] bigint  NOT NULL,
    [HouseTitle] nvarchar(100)  NOT NULL,
    [ProvinceId] int  NULL,
    [CityId] int  NULL,
    [CountyId] int  NULL,
    [DecorativeStyle] int  NOT NULL,
    [LeaseType] int  NOT NULL,
    [BedroomNum] int  NOT NULL,
    [LivingRoomNum] int  NOT NULL,
    [ToiletNum] int  NOT NULL,
    [Price] decimal(12,2)  NOT NULL,
    [IsTrusteeship] bit  NOT NULL,
    [IsSell] bit  NOT NULL,
    [SellPrice] decimal(12,2)  NULL,
    [IsCooking] bit  NOT NULL,
    [IsPet] bit  NOT NULL,
    [StayPersonNum] int  NOT NULL,
    [HouseSize] float  NOT NULL,
    [Facilities] varchar(max)  NULL,
    [Address] nvarchar(200)  NOT NULL,
    [About] varchar(max)  NULL,
    [Rules] varchar(max)  NULL,
    [ChargesNotes] varchar(max)  NULL,
    [BaseInfo] varchar(max)  NULL,
    [IsReals] bit  NOT NULL,
    [CreateTime] datetime  NOT NULL,
    [State] int  NOT NULL,
    [BuyNum] int  NOT NULL,
    [EvaluateNum] int  NOT NULL,
    [EvaluateAvgScore] int  NOT NULL,
    [ShopId] bigint  NULL,
    [ShopName] nvarchar(255)  NULL,
    [IsEmpty] int  NULL,
    [IsBack] int  NULL,
    [IsCancel] int  NULL
);
GO

-- Creating table 'OrderInfo'
CREATE TABLE [dbo].[OrderInfo] (
    [ID] bigint IDENTITY(1,1) NOT NULL,
    [OrderNo] varchar(20)  NOT NULL,
    [OtherNo] varchar(100)  NULL,
    [LandlordID] bigint  NOT NULL,
    [LandlordName] nvarchar(20)  NOT NULL,
    [LandlordPhone] nvarchar(20)  NOT NULL,
    [UserInfoID] bigint  NOT NULL,
    [Username] nvarchar(20)  NOT NULL,
    [UserPhone] nvarchar(20)  NOT NULL,
    [HouseInfoID] bigint  NOT NULL,
    [HouseTitle] nvarchar(100)  NOT NULL,
    [Price] decimal(10,2)  NOT NULL,
    [BuyCount] int  NOT NULL,
    [DownPayment] decimal(10,2)  NOT NULL,
    [TotalPrice] decimal(10,2)  NOT NULL,
    [BalancePayment] decimal(10,2)  NOT NULL,
    [PlatformRoyalty] decimal(10,2)  NOT NULL,
    [PayType] int  NULL,
    [PersonNum] int  NOT NULL,
    [IsFullPrice] bit  NULL,
    [StartDate] datetime  NOT NULL,
    [EndDate] datetime  NOT NULL,
    [State] int  NOT NULL,
    [CreateTime] datetime  NOT NULL,
    [IsEvaluate] bit  NOT NULL,
    [CouponCode] nvarchar(200)  NULL
);
GO

-- Creating table 'OrderItem'
CREATE TABLE [dbo].[OrderItem] (
    [ID] bigint IDENTITY(1,1) NOT NULL,
    [OrderNo] varchar(20)  NOT NULL,
    [OtherNo] varchar(100)  NULL,
    [OrderId] bigint  NOT NULL,
    [Price] decimal(10,2)  NOT NULL,
    [BalancePayment] decimal(10,2)  NOT NULL,
    [State] int  NOT NULL,
    [CreateTime] datetime  NOT NULL,
    [PayTime] datetime  NULL
);
GO

-- Creating table 'ProductCategory'
CREATE TABLE [dbo].[ProductCategory] (
    [ID] bigint IDENTITY(1,1) NOT NULL,
    [CategoryName] nvarchar(20)  NOT NULL,
    [Sort] int  NOT NULL,
    [CreateTime] datetime  NOT NULL
);
GO

-- Creating table 'ProductInfo'
CREATE TABLE [dbo].[ProductInfo] (
    [ID] bigint IDENTITY(1,1) NOT NULL,
    [ProductName] nvarchar(100)  NOT NULL,
    [CategoryID] bigint  NOT NULL,
    [CategoryName] nvarchar(20)  NOT NULL,
    [Price] nvarchar(40)  NOT NULL,
    [IsHomeShow] bit  NOT NULL,
    [Sort] int  NOT NULL,
    [CreateTime] datetime  NOT NULL,
    [ProductImg] nvarchar(255)  NULL,
    [HomeImg] nvarchar(255)  NULL,
    [ProvinceId] int  NULL,
    [CityId] int  NULL
);
GO

-- Creating table 'Region'
CREATE TABLE [dbo].[Region] (
    [Id] int  NOT NULL,
    [Name] nvarchar(40)  NULL,
    [ParentId] int  NULL,
    [ShortName] nvarchar(40)  NULL,
    [LevelType] int  NULL,
    [CityCode] varchar(20)  NULL,
    [ZipCode] varchar(20)  NULL,
    [MergerName] nvarchar(100)  NULL,
    [Lng] float  NULL,
    [Lat] float  NULL,
    [Pinyin] varchar(100)  NULL
);
GO

-- Creating table 'SpotInfo'
CREATE TABLE [dbo].[SpotInfo] (
    [ID] bigint IDENTITY(1,1) NOT NULL,
    [ScenicSpotName] nvarchar(200)  NOT NULL,
    [ScenicSpotDesc] varchar(max)  NULL,
    [ScenicSpotImgs] varchar(max)  NOT NULL,
    [IsHomeShow] bit  NOT NULL,
    [Sort] int  NOT NULL,
    [ProvinceId] int  NULL,
    [CityId] int  NULL,
    [CreateTime] datetime  NOT NULL,
    [LinkSpotId] bigint  NULL,
    [LinkSpotName] nvarchar(200)  NULL,
    [Links] varchar(max)  NULL
);
GO

-- Creating table 'SystemConfig'
CREATE TABLE [dbo].[SystemConfig] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Type] int  NOT NULL,
    [Name] nvarchar(20)  NOT NULL,
    [Value] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'TenantInfo'
CREATE TABLE [dbo].[TenantInfo] (
    [ID] bigint IDENTITY(1,1) NOT NULL,
    [OrderID] bigint  NOT NULL,
    [CertificateType] int  NOT NULL,
    [CertificateNo] nvarchar(50)  NOT NULL,
    [Gender] int  NOT NULL,
    [Nation] nvarchar(50)  NOT NULL,
    [CreateTime] datetime  NOT NULL
);
GO

-- Creating table 'UserAccount'
CREATE TABLE [dbo].[UserAccount] (
    [ID] bigint IDENTITY(1,1) NOT NULL,
    [PhoneAccount] varchar(50)  NULL,
    [WeixinUnionid] varchar(100)  NULL,
    [State] int  NOT NULL,
    [CreateTime] datetime  NOT NULL,
    [UserToken] varchar(40)  NULL
);
GO

-- Creating table 'UserFavorite'
CREATE TABLE [dbo].[UserFavorite] (
    [ID] bigint IDENTITY(1,1) NOT NULL,
    [UserInfoID] bigint  NOT NULL,
    [HouseInfoID] bigint  NOT NULL,
    [CreateTime] datetime  NOT NULL
);
GO

-- Creating table 'UserInfo'
CREATE TABLE [dbo].[UserInfo] (
    [ID] bigint IDENTITY(1,1) NOT NULL,
    [AccountID] bigint  NOT NULL,
    [Img] nvarchar(255)  NULL,
    [Nikename] nvarchar(20)  NULL,
    [Username] nvarchar(20)  NULL,
    [Gender] int  NULL,
    [Email] varchar(100)  NULL,
    [PhoneNo] nvarchar(20)  NULL,
    [PhoneNo2] nvarchar(20)  NULL,
    [IDCardNo] varchar(50)  NULL,
    [UserType] int  NOT NULL,
    [IsRealName] int  NULL,
    [CreateTime] datetime  NOT NULL,
    [LoginAccount] varchar(255)  NULL,
    [LoginPwd] varchar(255)  NULL
);
GO

-- Creating table 'UserInfoCertificate'
CREATE TABLE [dbo].[UserInfoCertificate] (
    [ID] bigint IDENTITY(1,1) NOT NULL,
    [UserInfoID] bigint  NOT NULL,
    [State] int  NOT NULL,
    [FailReason] nvarchar(100)  NULL,
    [CreateTime] datetime  NOT NULL
);
GO

-- Creating table 'UserInfoExt'
CREATE TABLE [dbo].[UserInfoExt] (
    [ID] bigint IDENTITY(1,1) NOT NULL,
    [IsCertification] int  NOT NULL,
    [UserInfoID] bigint  NOT NULL,
    [IDCardImg1] nvarchar(255)  NULL,
    [IDCardImg2] nvarchar(255)  NULL,
    [HouseAddress] nvarchar(200)  NULL,
    [Housecertificate] nvarchar(255)  NULL,
    [WeixinAccount] nvarchar(40)  NULL,
    [AlipayAccount] nvarchar(40)  NULL
);
GO

-- Creating table 'UserOrderRecord'
CREATE TABLE [dbo].[UserOrderRecord] (
    [ID] nchar(10)  NOT NULL
);
GO

-- Creating table 'HouseInfoView'
CREATE TABLE [dbo].[HouseInfoView] (
    [ID] bigint  NOT NULL,
    [UserInfoID] bigint  NOT NULL,
    [HouseTitle] nvarchar(100)  NOT NULL,
    [DecorativeStyle] int  NOT NULL,
    [LeaseType] int  NOT NULL,
    [BedroomNum] int  NOT NULL,
    [LivingRoomNum] int  NOT NULL,
    [ToiletNum] int  NOT NULL,
    [Price] decimal(12,2)  NOT NULL,
    [IsTrusteeship] bit  NOT NULL,
    [IsSell] bit  NOT NULL,
    [SellPrice] decimal(12,2)  NULL,
    [IsCooking] bit  NOT NULL,
    [IsPet] bit  NOT NULL,
    [StayPersonNum] int  NOT NULL,
    [HouseSize] float  NOT NULL,
    [Facilities] varchar(max)  NULL,
    [Address] nvarchar(200)  NOT NULL,
    [About] varchar(max)  NULL,
    [Rules] varchar(max)  NULL,
    [ChargesNotes] varchar(max)  NULL,
    [BaseInfo] varchar(max)  NULL,
    [IsReals] bit  NOT NULL,
    [CreateTime] datetime  NOT NULL,
    [State] int  NOT NULL,
    [BuyNum] int  NOT NULL,
    [EvaluateNum] int  NOT NULL,
    [EvaluateAvgScore] int  NOT NULL,
    [AccountID] bigint  NOT NULL,
    [Img] nvarchar(255)  NULL,
    [Nikename] nvarchar(20)  NULL,
    [Username] nvarchar(20)  NULL,
    [Gender] int  NULL,
    [PhoneNo] nvarchar(20)  NULL,
    [Email] varchar(100)  NULL,
    [PhoneNo2] nvarchar(20)  NULL,
    [IDCardNo] varchar(50)  NULL,
    [UserType] int  NOT NULL,
    [IsRealName] int  NULL,
    [ProvinceId] int  NULL,
    [CityId] int  NULL,
    [CountyId] int  NULL,
    [ShopId] bigint  NULL,
    [ShopName] nvarchar(255)  NULL,
    [IsEmpty] int  NULL,
    [IsBack] int  NULL,
    [IsCancel] int  NULL
);
GO

-- Creating table 'OrderInfoView'
CREATE TABLE [dbo].[OrderInfoView] (
    [Address] nvarchar(200)  NULL,
    [ID] bigint  NOT NULL,
    [OrderNo] varchar(20)  NOT NULL,
    [OtherNo] varchar(100)  NULL,
    [LandlordID] bigint  NOT NULL,
    [LandlordName] nvarchar(20)  NOT NULL,
    [LandlordPhone] nvarchar(20)  NOT NULL,
    [UserInfoID] bigint  NOT NULL,
    [Username] nvarchar(20)  NOT NULL,
    [UserPhone] nvarchar(20)  NOT NULL,
    [HouseInfoID] bigint  NOT NULL,
    [HouseTitle] nvarchar(100)  NOT NULL,
    [Price] decimal(10,2)  NOT NULL,
    [BuyCount] int  NOT NULL,
    [DownPayment] decimal(10,2)  NOT NULL,
    [TotalPrice] decimal(10,2)  NOT NULL,
    [BalancePayment] decimal(10,2)  NOT NULL,
    [PlatformRoyalty] decimal(10,2)  NOT NULL,
    [PayType] int  NULL,
    [PersonNum] int  NOT NULL,
    [IsFullPrice] bit  NULL,
    [StartDate] datetime  NOT NULL,
    [EndDate] datetime  NOT NULL,
    [State] int  NOT NULL,
    [CreateTime] datetime  NOT NULL,
    [IsEvaluate] bit  NOT NULL,
    [ShopId] bigint  NULL,
    [ShopName] nvarchar(255)  NULL
);
GO

-- Creating table 'UserFavoriteView'
CREATE TABLE [dbo].[UserFavoriteView] (
    [ID] bigint  NOT NULL,
    [UserInfoID] bigint  NOT NULL,
    [HouseInfoID] bigint  NOT NULL,
    [CreateTime] datetime  NOT NULL,
    [HouseTitle] nvarchar(100)  NOT NULL,
    [Username] nvarchar(20)  NULL,
    [PhoneNo] nvarchar(20)  NULL
);
GO

-- Creating table 'UserInfoCertificateView'
CREATE TABLE [dbo].[UserInfoCertificateView] (
    [ID] bigint  NOT NULL,
    [UserInfoID] bigint  NOT NULL,
    [State] int  NOT NULL,
    [FailReason] nvarchar(100)  NULL,
    [CreateTime] datetime  NOT NULL,
    [Username] nvarchar(20)  NULL,
    [Gender] int  NULL,
    [PhoneNo] nvarchar(20)  NULL,
    [IDCardNo] varchar(50)  NULL,
    [AccountID] bigint  NOT NULL,
    [Nikename] nvarchar(20)  NULL,
    [Email] varchar(100)  NULL,
    [PhoneNo2] nvarchar(20)  NULL,
    [UserType] int  NOT NULL,
    [IsRealName] int  NULL,
    [IsCertification] int  NOT NULL,
    [IDCardImg1] nvarchar(255)  NULL,
    [IDCardImg2] nvarchar(255)  NULL,
    [HouseAddress] nvarchar(200)  NULL,
    [Housecertificate] nvarchar(255)  NULL,
    [Img] nvarchar(255)  NULL,
    [WeixinAccount] nvarchar(40)  NULL,
    [AlipayAccount] nvarchar(40)  NULL
);
GO

-- Creating table 'UserInfoExtView'
CREATE TABLE [dbo].[UserInfoExtView] (
    [IDCardImg1] nvarchar(255)  NULL,
    [IDCardImg2] nvarchar(255)  NULL,
    [HouseAddress] nvarchar(200)  NULL,
    [Housecertificate] nvarchar(255)  NULL,
    [ID] bigint  NOT NULL,
    [AccountID] bigint  NOT NULL,
    [Nikename] nvarchar(20)  NULL,
    [Username] nvarchar(20)  NULL,
    [Gender] int  NULL,
    [Email] varchar(100)  NULL,
    [PhoneNo] nvarchar(20)  NULL,
    [PhoneNo2] nvarchar(20)  NULL,
    [IDCardNo] varchar(50)  NULL,
    [UserType] int  NOT NULL,
    [IsRealName] int  NOT NULL,
    [CreateTime] datetime  NOT NULL,
    [State] int  NOT NULL,
    [Img] nvarchar(255)  NULL,
    [PhoneAccount] varchar(50)  NULL,
    [IsCertification] int  NOT NULL,
    [UserToken] varchar(40)  NULL,
    [AlipayAccount] nvarchar(40)  NULL,
    [WeixinAccount] nvarchar(40)  NULL,
    [WeixinUnionid] varchar(100)  NULL,
    [LoginAccount] varchar(255)  NULL,
    [LoginPwd] varchar(255)  NULL
);
GO

-- Creating table 'UserInfoView'
CREATE TABLE [dbo].[UserInfoView] (
    [ID] bigint  NOT NULL,
    [AccountID] bigint  NOT NULL,
    [Img] nvarchar(255)  NULL,
    [Nikename] nvarchar(20)  NULL,
    [Username] nvarchar(20)  NULL,
    [Gender] int  NULL,
    [Email] varchar(100)  NULL,
    [PhoneNo] nvarchar(20)  NULL,
    [PhoneNo2] nvarchar(20)  NULL,
    [IDCardNo] varchar(50)  NULL,
    [UserType] int  NOT NULL,
    [IsRealName] int  NULL,
    [CreateTime] datetime  NOT NULL,
    [State] int  NOT NULL,
    [PhoneAccount] varchar(50)  NULL,
    [UserToken] varchar(40)  NULL,
    [WeixinUnionid] varchar(100)  NULL,
    [LoginAccount] varchar(255)  NULL,
    [LoginPwd] varchar(255)  NULL,
    [IDCardImg1] nvarchar(255)  NULL,
    [IDCardImg2] nvarchar(255)  NULL,
    [HouseAddress] nvarchar(200)  NULL,
    [Housecertificate] nvarchar(255)  NULL,
    [IsCertification] int  NULL,
    [AlipayAccount] nvarchar(40)  NULL,
    [WeixinAccount] nvarchar(40)  NULL
);
GO

-- Creating table 'ShopCategory'
CREATE TABLE [dbo].[ShopCategory] (
    [ID] bigint IDENTITY(1,1) NOT NULL,
    [CategoryName] nvarchar(255)  NULL,
    [SortBy] int  NULL,
    [CreateTime] datetime  NULL
);
GO

-- Creating table 'ShopInfo'
CREATE TABLE [dbo].[ShopInfo] (
    [ID] bigint IDENTITY(1,1) NOT NULL,
    [ShopName] nvarchar(255)  NULL,
    [Locations] nvarchar(500)  NULL,
    [Scores] decimal(18,2)  NULL,
    [StartPrice] decimal(18,0)  NULL,
    [ShopType] bigint  NULL,
    [Tags] varchar(max)  NULL,
    [UserId] bigint  NULL,
    [SpotId] bigint  NULL,
    [CreateBy] nvarchar(255)  NULL,
    [CreateUserId] bigint  NULL,
    [CreateOn] datetime  NULL,
    [SortBy] bigint  NULL,
    [ShopImgs] varchar(max)  NULL,
    [ShopTypeName] nvarchar(255)  NULL,
    [SpotName] nvarchar(255)  NULL,
    [PhoneNo] nvarchar(255)  NULL,
    [UserName] nvarchar(255)  NULL,
    [DecorativeStyle] nvarchar(255)  NULL,
    [State] int  NULL,
    [IsDel] int  NULL,
    [IsCheck] int  NULL,
    [About] varchar(max)  NULL,
    [Rules] varchar(max)  NULL,
    [ChargesNotes] varchar(max)  NULL
);
GO

-- Creating table 'ShopInfoCertificate'
CREATE TABLE [dbo].[ShopInfoCertificate] (
    [ID] bigint IDENTITY(1,1) NOT NULL,
    [ShopId] bigint  NOT NULL,
    [State] int  NOT NULL,
    [FailReason] varchar(500)  NULL,
    [CreateOn] datetime  NOT NULL
);
GO

-- Creating table 'ShopInfoCertificateView'
CREATE TABLE [dbo].[ShopInfoCertificateView] (
    [ShopName] nvarchar(255)  NULL,
    [Locations] nvarchar(500)  NULL,
    [Scores] decimal(18,2)  NULL,
    [StartPrice] decimal(18,0)  NULL,
    [ShopType] bigint  NULL,
    [Tags] varchar(max)  NULL,
    [UserId] bigint  NULL,
    [SpotId] bigint  NULL,
    [CreateBy] nvarchar(255)  NULL,
    [CreateUserId] bigint  NULL,
    [SortBy] bigint  NULL,
    [ShopImgs] varchar(max)  NULL,
    [ShopTypeName] nvarchar(255)  NULL,
    [SpotName] nvarchar(255)  NULL,
    [PhoneNo] nvarchar(255)  NULL,
    [UserName] nvarchar(255)  NULL,
    [DecorativeStyle] nvarchar(255)  NULL,
    [IsCheck] int  NULL,
    [About] varchar(max)  NULL,
    [Rules] varchar(max)  NULL,
    [ChargesNotes] varchar(max)  NULL,
    [ID] bigint  NOT NULL,
    [ShopId] bigint  NOT NULL,
    [State] int  NOT NULL,
    [FailReason] varchar(500)  NULL,
    [CreateOn] datetime  NOT NULL
);
GO

-- Creating table 'HouseComment'
CREATE TABLE [dbo].[HouseComment] (
    [ID] bigint IDENTITY(1,1) NOT NULL,
    [HouseInfoId] bigint  NOT NULL,
    [EvaluateId] bigint  NOT NULL,
    [ToUserId] bigint  NOT NULL,
    [ToUserName] nvarchar(100)  NULL,
    [FromUserId] bigint  NOT NULL,
    [FromUserName] nvarchar(100)  NULL,
    [CreateOn] datetime  NULL,
    [Comments] varchar(max)  NULL
);
GO

-- Creating table 'HouseEvaluateView'
CREATE TABLE [dbo].[HouseEvaluateView] (
    [ID] bigint  NOT NULL,
    [HouseInfoID] bigint  NOT NULL,
    [UserInfoID] bigint  NOT NULL,
    [UserPhone] nvarchar(20)  NOT NULL,
    [CleanScore] int  NOT NULL,
    [LocationScore] int  NOT NULL,
    [EnvironmentScore] int  NOT NULL,
    [ServiceScore] int  NOT NULL,
    [PerformanceScore] int  NOT NULL,
    [AverageScore] float  NOT NULL,
    [EvaluateContent] varchar(max)  NOT NULL,
    [CreateTime] datetime  NOT NULL,
    [State] int  NOT NULL,
    [Username] nvarchar(40)  NOT NULL,
    [UserImg] nvarchar(200)  NULL,
    [HouseTitle] nvarchar(100)  NULL,
    [ProvinceId] int  NULL,
    [CityId] int  NULL,
    [CountyId] int  NULL,
    [DecorativeStyle] int  NULL,
    [LeaseType] int  NULL,
    [BedroomNum] int  NULL,
    [LivingRoomNum] int  NULL,
    [ToiletNum] int  NULL,
    [Price] decimal(12,2)  NULL,
    [IsTrusteeship] bit  NULL,
    [IsSell] bit  NULL,
    [SellPrice] decimal(12,2)  NULL,
    [IsCooking] bit  NULL,
    [IsPet] bit  NULL,
    [StayPersonNum] int  NULL,
    [HouseSize] float  NULL,
    [Facilities] varchar(max)  NULL,
    [Address] nvarchar(200)  NULL,
    [About] varchar(max)  NULL,
    [Rules] varchar(max)  NULL,
    [ChargesNotes] varchar(max)  NULL,
    [BaseInfo] varchar(max)  NULL,
    [IsReals] bit  NULL,
    [BuyNum] int  NULL,
    [EvaluateNum] int  NULL,
    [EvaluateAvgScore] int  NULL,
    [ShopId] bigint  NULL,
    [ShopName] nvarchar(255)  NULL,
    [Comments] varchar(max)  NULL,
    [CreateOn] datetime  NULL,
    [FromUserId] bigint  NULL,
    [FromUserName] nvarchar(100)  NULL
);
GO

-- Creating table 'SysLog'
CREATE TABLE [dbo].[SysLog] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [ErrorMsg] varchar(max)  NULL,
    [CreateOn] datetime  NULL
);
GO

-- Creating table 'T_User'
CREATE TABLE [dbo].[T_User] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [LoginName] nvarchar(max)  NOT NULL,
    [Password] nvarchar(max)  NOT NULL,
    [UserRealName] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [CreateTime] datetime  NULL,
    [IsDeleted] bit  NOT NULL,
    [Email] nvarchar(max)  NULL
);
GO

-- Creating table 'T_Role'
CREATE TABLE [dbo].[T_Role] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [CreateTime] datetime  NULL,
    [IsDeleted] bit  NOT NULL
);
GO

-- Creating table 'T_Permission'
CREATE TABLE [dbo].[T_Permission] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [Type] bigint  NOT NULL,
    [CreateTime] datetime  NOT NULL,
    [ParentId] bigint  NOT NULL,
    [IsDeleted] bit  NOT NULL,
    [Url] nvarchar(max)  NULL
);
GO

-- Creating table 'ShopToDayPriceSet'
CREATE TABLE [dbo].[ShopToDayPriceSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ShopId] int  NOT NULL,
    [other] nvarchar(max)  NULL,
    [price] decimal(18,0)  NOT NULL,
    [statu] int  NOT NULL,
    [date] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'UserRole'
CREATE TABLE [dbo].[UserRole] (
    [User_Id] bigint  NOT NULL,
    [Role_Id] bigint  NOT NULL
);
GO

-- Creating table 'PermissionRole'
CREATE TABLE [dbo].[PermissionRole] (
    [Permission_Id] bigint  NOT NULL,
    [Role_Id] bigint  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ID] in table 'AdminUser'
ALTER TABLE [dbo].[AdminUser]
ADD CONSTRAINT [PK_AdminUser]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'CarInfo'
ALTER TABLE [dbo].[CarInfo]
ADD CONSTRAINT [PK_CarInfo]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HomeData'
ALTER TABLE [dbo].[HomeData]
ADD CONSTRAINT [PK_HomeData]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HouseConfigure'
ALTER TABLE [dbo].[HouseConfigure]
ADD CONSTRAINT [PK_HouseConfigure]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HouseEvaluate'
ALTER TABLE [dbo].[HouseEvaluate]
ADD CONSTRAINT [PK_HouseEvaluate]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HouseImg'
ALTER TABLE [dbo].[HouseImg]
ADD CONSTRAINT [PK_HouseImg]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HouseInfo'
ALTER TABLE [dbo].[HouseInfo]
ADD CONSTRAINT [PK_HouseInfo]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'OrderInfo'
ALTER TABLE [dbo].[OrderInfo]
ADD CONSTRAINT [PK_OrderInfo]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'OrderItem'
ALTER TABLE [dbo].[OrderItem]
ADD CONSTRAINT [PK_OrderItem]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ProductCategory'
ALTER TABLE [dbo].[ProductCategory]
ADD CONSTRAINT [PK_ProductCategory]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ProductInfo'
ALTER TABLE [dbo].[ProductInfo]
ADD CONSTRAINT [PK_ProductInfo]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [Id] in table 'Region'
ALTER TABLE [dbo].[Region]
ADD CONSTRAINT [PK_Region]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [ID] in table 'SpotInfo'
ALTER TABLE [dbo].[SpotInfo]
ADD CONSTRAINT [PK_SpotInfo]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'SystemConfig'
ALTER TABLE [dbo].[SystemConfig]
ADD CONSTRAINT [PK_SystemConfig]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'TenantInfo'
ALTER TABLE [dbo].[TenantInfo]
ADD CONSTRAINT [PK_TenantInfo]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'UserAccount'
ALTER TABLE [dbo].[UserAccount]
ADD CONSTRAINT [PK_UserAccount]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'UserFavorite'
ALTER TABLE [dbo].[UserFavorite]
ADD CONSTRAINT [PK_UserFavorite]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'UserInfo'
ALTER TABLE [dbo].[UserInfo]
ADD CONSTRAINT [PK_UserInfo]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'UserInfoCertificate'
ALTER TABLE [dbo].[UserInfoCertificate]
ADD CONSTRAINT [PK_UserInfoCertificate]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'UserInfoExt'
ALTER TABLE [dbo].[UserInfoExt]
ADD CONSTRAINT [PK_UserInfoExt]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'UserOrderRecord'
ALTER TABLE [dbo].[UserOrderRecord]
ADD CONSTRAINT [PK_UserOrderRecord]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID], [UserInfoID], [HouseTitle], [DecorativeStyle], [LeaseType], [BedroomNum], [LivingRoomNum], [ToiletNum], [Price], [IsTrusteeship], [IsSell], [IsCooking], [IsPet], [StayPersonNum], [HouseSize], [Address], [IsReals], [CreateTime], [State], [BuyNum], [EvaluateNum], [EvaluateAvgScore], [AccountID], [UserType] in table 'HouseInfoView'
ALTER TABLE [dbo].[HouseInfoView]
ADD CONSTRAINT [PK_HouseInfoView]
    PRIMARY KEY CLUSTERED ([ID], [UserInfoID], [HouseTitle], [DecorativeStyle], [LeaseType], [BedroomNum], [LivingRoomNum], [ToiletNum], [Price], [IsTrusteeship], [IsSell], [IsCooking], [IsPet], [StayPersonNum], [HouseSize], [Address], [IsReals], [CreateTime], [State], [BuyNum], [EvaluateNum], [EvaluateAvgScore], [AccountID], [UserType] ASC);
GO

-- Creating primary key on [ID], [OrderNo], [LandlordID], [LandlordName], [LandlordPhone], [UserInfoID], [Username], [UserPhone], [HouseInfoID], [HouseTitle], [Price], [BuyCount], [DownPayment], [TotalPrice], [BalancePayment], [PlatformRoyalty], [PersonNum], [StartDate], [EndDate], [State], [CreateTime], [IsEvaluate] in table 'OrderInfoView'
ALTER TABLE [dbo].[OrderInfoView]
ADD CONSTRAINT [PK_OrderInfoView]
    PRIMARY KEY CLUSTERED ([ID], [OrderNo], [LandlordID], [LandlordName], [LandlordPhone], [UserInfoID], [Username], [UserPhone], [HouseInfoID], [HouseTitle], [Price], [BuyCount], [DownPayment], [TotalPrice], [BalancePayment], [PlatformRoyalty], [PersonNum], [StartDate], [EndDate], [State], [CreateTime], [IsEvaluate] ASC);
GO

-- Creating primary key on [ID], [UserInfoID], [HouseInfoID], [CreateTime], [HouseTitle] in table 'UserFavoriteView'
ALTER TABLE [dbo].[UserFavoriteView]
ADD CONSTRAINT [PK_UserFavoriteView]
    PRIMARY KEY CLUSTERED ([ID], [UserInfoID], [HouseInfoID], [CreateTime], [HouseTitle] ASC);
GO

-- Creating primary key on [ID], [UserInfoID], [State], [CreateTime], [AccountID], [UserType], [IsCertification] in table 'UserInfoCertificateView'
ALTER TABLE [dbo].[UserInfoCertificateView]
ADD CONSTRAINT [PK_UserInfoCertificateView]
    PRIMARY KEY CLUSTERED ([ID], [UserInfoID], [State], [CreateTime], [AccountID], [UserType], [IsCertification] ASC);
GO

-- Creating primary key on [ID], [AccountID], [UserType], [CreateTime], [State], [IsCertification] in table 'UserInfoExtView'
ALTER TABLE [dbo].[UserInfoExtView]
ADD CONSTRAINT [PK_UserInfoExtView]
    PRIMARY KEY CLUSTERED ([ID], [AccountID], [UserType], [CreateTime], [State], [IsCertification] ASC);
GO

-- Creating primary key on [ID], [AccountID], [UserType], [CreateTime], [State] in table 'UserInfoView'
ALTER TABLE [dbo].[UserInfoView]
ADD CONSTRAINT [PK_UserInfoView]
    PRIMARY KEY CLUSTERED ([ID], [AccountID], [UserType], [CreateTime], [State] ASC);
GO

-- Creating primary key on [ID] in table 'ShopCategory'
ALTER TABLE [dbo].[ShopCategory]
ADD CONSTRAINT [PK_ShopCategory]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ShopInfo'
ALTER TABLE [dbo].[ShopInfo]
ADD CONSTRAINT [PK_ShopInfo]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ShopInfoCertificate'
ALTER TABLE [dbo].[ShopInfoCertificate]
ADD CONSTRAINT [PK_ShopInfoCertificate]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID], [ShopId], [State], [CreateOn] in table 'ShopInfoCertificateView'
ALTER TABLE [dbo].[ShopInfoCertificateView]
ADD CONSTRAINT [PK_ShopInfoCertificateView]
    PRIMARY KEY CLUSTERED ([ID], [ShopId], [State], [CreateOn] ASC);
GO

-- Creating primary key on [ID] in table 'HouseComment'
ALTER TABLE [dbo].[HouseComment]
ADD CONSTRAINT [PK_HouseComment]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID], [HouseInfoID], [UserInfoID], [UserPhone], [CleanScore], [LocationScore], [EnvironmentScore], [ServiceScore], [PerformanceScore], [AverageScore], [EvaluateContent], [CreateTime], [State], [Username] in table 'HouseEvaluateView'
ALTER TABLE [dbo].[HouseEvaluateView]
ADD CONSTRAINT [PK_HouseEvaluateView]
    PRIMARY KEY CLUSTERED ([ID], [HouseInfoID], [UserInfoID], [UserPhone], [CleanScore], [LocationScore], [EnvironmentScore], [ServiceScore], [PerformanceScore], [AverageScore], [EvaluateContent], [CreateTime], [State], [Username] ASC);
GO

-- Creating primary key on [Id] in table 'SysLog'
ALTER TABLE [dbo].[SysLog]
ADD CONSTRAINT [PK_SysLog]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'T_User'
ALTER TABLE [dbo].[T_User]
ADD CONSTRAINT [PK_T_User]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'T_Role'
ALTER TABLE [dbo].[T_Role]
ADD CONSTRAINT [PK_T_Role]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'T_Permission'
ALTER TABLE [dbo].[T_Permission]
ADD CONSTRAINT [PK_T_Permission]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ShopToDayPriceSet'
ALTER TABLE [dbo].[ShopToDayPriceSet]
ADD CONSTRAINT [PK_ShopToDayPriceSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [User_Id], [Role_Id] in table 'UserRole'
ALTER TABLE [dbo].[UserRole]
ADD CONSTRAINT [PK_UserRole]
    PRIMARY KEY CLUSTERED ([User_Id], [Role_Id] ASC);
GO

-- Creating primary key on [Permission_Id], [Role_Id] in table 'PermissionRole'
ALTER TABLE [dbo].[PermissionRole]
ADD CONSTRAINT [PK_PermissionRole]
    PRIMARY KEY CLUSTERED ([Permission_Id], [Role_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [User_Id] in table 'UserRole'
ALTER TABLE [dbo].[UserRole]
ADD CONSTRAINT [FK_UserRole_User]
    FOREIGN KEY ([User_Id])
    REFERENCES [dbo].[T_User]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Role_Id] in table 'UserRole'
ALTER TABLE [dbo].[UserRole]
ADD CONSTRAINT [FK_UserRole_Role]
    FOREIGN KEY ([Role_Id])
    REFERENCES [dbo].[T_Role]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserRole_Role'
CREATE INDEX [IX_FK_UserRole_Role]
ON [dbo].[UserRole]
    ([Role_Id]);
GO

-- Creating foreign key on [Permission_Id] in table 'PermissionRole'
ALTER TABLE [dbo].[PermissionRole]
ADD CONSTRAINT [FK_PermissionRole_Permission]
    FOREIGN KEY ([Permission_Id])
    REFERENCES [dbo].[T_Permission]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Role_Id] in table 'PermissionRole'
ALTER TABLE [dbo].[PermissionRole]
ADD CONSTRAINT [FK_PermissionRole_Role]
    FOREIGN KEY ([Role_Id])
    REFERENCES [dbo].[T_Role]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PermissionRole_Role'
CREATE INDEX [IX_FK_PermissionRole_Role]
ON [dbo].[PermissionRole]
    ([Role_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------