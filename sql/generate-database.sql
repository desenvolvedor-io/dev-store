USE [master]
GO
/****** Object:  Database [DSBilling]    Script Date: 01/10/2021 21:23:12 ******/
CREATE DATABASE [DSBilling]
GO

USE [DSBilling]
GO

CREATE TABLE [dbo].[Payments](
	[Id] [uniqueidentifier] NOT NULL,
	[OrderId] [uniqueidentifier] NOT NULL,
	[PaymentType] [int] NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_Payments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Transactions](
	[Id] [uniqueidentifier] NOT NULL,
	[AuthorizationCode] [varchar](100) NULL,
	[CreditCardCompany] [varchar](100) NULL,
	[TransactionDate] [datetime2](7) NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[TransactionCost] [decimal](18, 2) NOT NULL,
	[TransactionStatus] [int] NOT NULL,
	[TID] [varchar](100) NULL,
	[NSU] [varchar](100) NULL,
	[PaymentId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Transactions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Transactions_PaymentId] ON [dbo].[Transactions]
(
	[PaymentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Transactions]  WITH CHECK ADD  CONSTRAINT [FK_Transactions_Payments_PaymentId] FOREIGN KEY([PaymentId])
REFERENCES [dbo].[Payments] ([Id])
GO
ALTER TABLE [dbo].[Transactions] CHECK CONSTRAINT [FK_Transactions_Payments_PaymentId]
GO

USE [master]
GO
ALTER DATABASE [DSBilling] SET  READ_WRITE 
GO



/****** Object:  Database [DSCatalog]    Script Date: 01/10/2021 21:26:03 ******/
CREATE DATABASE [DSCatalog]
GO

USE [DSCatalog]
GO

CREATE TABLE [dbo].[Products](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [varchar](250) NOT NULL,
	[Description] [varchar](500) NOT NULL,
	[Active] [bit] NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[DateAdded] [datetime2](7) NOT NULL,
	[Image] [varchar](250) NOT NULL,
	[Stock] [int] NOT NULL,
 CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

USE [master]
GO

ALTER DATABASE [DSCatalog] SET  READ_WRITE 
GO


/****** Object:  Database [DSCustomers]    Script Date: 01/10/2021 21:27:00 ******/
CREATE DATABASE [DSCustomers]
GO

USE [DSCustomers]
GO

CREATE TABLE [dbo].[Addresses](
	[Id] [uniqueidentifier] NOT NULL,
	[StreetAddress] [varchar](200) NOT NULL,
	[BuildingNumber] [varchar](50) NOT NULL,
	[SecondaryAddress] [varchar](250) NULL,
	[Neighborhood] [varchar](100) NOT NULL,
	[ZipCode] [varchar](20) NOT NULL,
	[City] [varchar](100) NOT NULL,
	[State] [varchar](50) NOT NULL,
	[CustomerId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Addresses] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [varchar](200) NOT NULL,
	[Email] [varchar](254) NULL,
	[SocialNumber] [varchar](50) NOT NULL,
	[Deleted] [bit] NOT NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_Addresses_CustomerId] ON [dbo].[Addresses]
(
	[CustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Addresses]  WITH CHECK ADD  CONSTRAINT [FK_Addresses_Customer_CustomerId] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
GO
ALTER TABLE [dbo].[Addresses] CHECK CONSTRAINT [FK_Addresses_Customer_CustomerId]
GO

USE [master]
GO
ALTER DATABASE [DSCustomers] SET  READ_WRITE 
GO


/****** Object:  Database [DSOrders]    Script Date: 01/10/2021 21:28:07 ******/
CREATE DATABASE [DSOrders]
GO

USE [DSOrders]
GO

CREATE SEQUENCE [dbo].[MySequence] 
 AS [int]
 START WITH 1000
 INCREMENT BY 1
 MINVALUE -2147483648
 MAXVALUE 2147483647
 CACHE 
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderItems](
	[Id] [uniqueidentifier] NOT NULL,
	[OrderId] [uniqueidentifier] NOT NULL,
	[ProductId] [uniqueidentifier] NOT NULL,
	[ProductName] [varchar](250) NOT NULL,
	[Quantity] [int] NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[ProductImage] [varchar](100) NULL,
 CONSTRAINT [PK_OrderItems] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[Id] [uniqueidentifier] NOT NULL,
	[Code] [int] NOT NULL,
	[CustomerId] [uniqueidentifier] NOT NULL,
	[VoucherId] [uniqueidentifier] NULL,
	[HasVoucher] [bit] NOT NULL,
	[Discount] [decimal](18, 2) NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[DateAdded] [datetime2](7) NOT NULL,
	[OrderStatus] [int] NOT NULL,
	[StreetAddress] [varchar](100) NULL,
	[BuildingNumber] [varchar](100) NULL,
	[SecondaryAddress] [varchar](100) NULL,
	[Neighborhood] [varchar](100) NULL,
	[ZipCode] [varchar](100) NULL,
	[City] [varchar](100) NULL,
	[State] [varchar](100) NULL,
 CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vouchers](
	[Id] [uniqueidentifier] NOT NULL,
	[Code] [varchar](100) NOT NULL,
	[Percentage] [decimal](18, 2) NULL,
	[Discount] [decimal](18, 2) NULL,
	[Quantity] [int] NOT NULL,
	[DiscountType] [int] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UsedAt] [datetime2](7) NULL,
	[ExpirationDate] [datetime2](7) NOT NULL,
	[Active] [bit] NOT NULL,
	[Used] [bit] NOT NULL,
 CONSTRAINT [PK_Vouchers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_OrderItems_OrderId] ON [dbo].[OrderItems]
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Orders_VoucherId] ON [dbo].[Orders]
(
	[VoucherId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Orders] ADD  DEFAULT (NEXT VALUE FOR [MySequence]) FOR [Code]
GO
ALTER TABLE [dbo].[OrderItems]  WITH CHECK ADD  CONSTRAINT [FK_OrderItems_Orders_OrderId] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Orders] ([Id])
GO
ALTER TABLE [dbo].[OrderItems] CHECK CONSTRAINT [FK_OrderItems_Orders_OrderId]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Vouchers_VoucherId] FOREIGN KEY([VoucherId])
REFERENCES [dbo].[Vouchers] ([Id])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Vouchers_VoucherId]
GO

USE [master]
GO
ALTER DATABASE [DSOrders] SET  READ_WRITE 
GO


/****** Object:  Database [DSShoppingCart]    Script Date: 01/10/2021 21:29:26 ******/
CREATE DATABASE [DSShoppingCart]
GO

USE [DSShoppingCart]
GO

CREATE TABLE [dbo].[CartItems](
	[Id] [uniqueidentifier] NOT NULL,
	[ProductId] [uniqueidentifier] NOT NULL,
	[Name] [varchar](100) NULL,
	[Quantity] [int] NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[Image] [varchar](100) NULL,
	[ShoppingCartId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_CartItems] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerShoppingCart](
	[Id] [uniqueidentifier] NOT NULL,
	[CustomerId] [uniqueidentifier] NOT NULL,
	[Total] [decimal](18, 2) NOT NULL,
	[HasVoucher] [bit] NOT NULL,
	[Discount] [decimal](18, 2) NOT NULL,
	[Voucher_Percentage] [decimal](18, 2) NULL,
	[Voucher_Discount] [decimal](18, 2) NULL,
	[Voucher_Code] [varchar](50) NULL,
	[Voucher_DiscountType] [int] NULL,
 CONSTRAINT [PK_CustomerShoppingCart] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_CartItems_ShoppingCartId] ON [dbo].[CartItems]
(
	[ShoppingCartId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IDX_Customer] ON [dbo].[CustomerShoppingCart]
(
	[CustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CartItems]  WITH CHECK ADD  CONSTRAINT [FK_CartItems_CustomerShoppingCart_ShoppingCartId] FOREIGN KEY([ShoppingCartId])
REFERENCES [dbo].[CustomerShoppingCart] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CartItems] CHECK CONSTRAINT [FK_CartItems_CustomerShoppingCart_ShoppingCartId]
GO

USE [master]
GO

ALTER DATABASE [DSShoppingCart] SET  READ_WRITE 
GO


/****** Object:  Database [DSUsers]    Script Date: 01/10/2021 21:30:23 ******/
CREATE DATABASE [DSUsers]

GO
USE [DSUsers]
GO

CREATE TABLE [dbo].[AspNetRoleClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
	[UserId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](450) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](450) NOT NULL,
	[UserName] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[Email] [nvarchar](256) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserTokens](
	[UserId] [nvarchar](450) NOT NULL,
	[LoginProvider] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](128) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RefreshTokens](
	[Id] [uniqueidentifier] NOT NULL,
	[Username] [nvarchar](max) NULL,
	[Token] [uniqueidentifier] NOT NULL,
	[ExpirationDate] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_RefreshTokens] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SecurityKeys](
	[Id] [uniqueidentifier] NOT NULL,
	[Parameters] [nvarchar](max) NULL,
	[KeyId] [nvarchar](max) NULL,
	[Type] [nvarchar](max) NULL,
	[JwsAlgorithm] [nvarchar](max) NULL,
	[JweAlgorithm] [nvarchar](max) NULL,
	[JweEncryption] [nvarchar](max) NULL,
	[CreationDate] [datetime2](7) NOT NULL,
	[JwkType] [int] NOT NULL,
	[IsRevoked] [bit] NOT NULL,
 CONSTRAINT [PK_SecurityKeys] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO

CREATE NONCLUSTERED INDEX [IX_AspNetRoleClaims_RoleId] ON [dbo].[AspNetRoleClaims]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO

CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex] ON [dbo].[AspNetRoles]
(
	[NormalizedName] ASC
)
WHERE ([NormalizedName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO

CREATE NONCLUSTERED INDEX [IX_AspNetUserClaims_UserId] ON [dbo].[AspNetUserClaims]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO

CREATE NONCLUSTERED INDEX [IX_AspNetUserLogins_UserId] ON [dbo].[AspNetUserLogins]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO

CREATE NONCLUSTERED INDEX [IX_AspNetUserRoles_RoleId] ON [dbo].[AspNetUserRoles]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO

CREATE NONCLUSTERED INDEX [EmailIndex] ON [dbo].[AspNetUsers]
(
	[NormalizedEmail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO

CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex] ON [dbo].[AspNetUsers]
(
	[NormalizedUserName] ASC
)
WHERE ([NormalizedUserName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AspNetRoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetRoleClaims] CHECK CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserTokens]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserTokens] CHECK CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]
GO
USE [master]
GO
ALTER DATABASE [DSUsers] SET  READ_WRITE 
GO

USE [DSCatalog]
GO
INSERT [dbo].[Products] ([Id], [Name], [Description], [Active], [Price], [DateAdded], [Image], [Stock]) VALUES (N'ba861293-ded9-4639-ab8f-0395bf15ddca', N'Mug Wonder Woman', N'Porcelain mug with high-strength thermal printing.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2021-10-01T21:33:45.8373449' AS DateTime2), N'caneca-Wonder Woman.jpg', 10)
GO
INSERT [dbo].[Products] ([Id], [Name], [Description], [Active], [Price], [DateAdded], [Image], [Stock]) VALUES (N'51e47fc9-d1ec-4d3d-b8be-0e2f0aec878c', N'Mug Rick and Morty', N'Porcelain mug with high-strength thermal printing.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2021-10-01T21:33:45.8373316' AS DateTime2), N'caneca-Rick and Morty.jpg', 5)
GO
INSERT [dbo].[Products] ([Id], [Name], [Description], [Active], [Price], [DateAdded], [Image], [Stock]) VALUES (N'd61f5340-bc48-4ddd-b9e8-13b4efbce1fe', N'T-Shirt Maestria', N'T-Shirt 100% cotton, high durable to wash and high temperatures washing.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2021-10-01T21:33:45.8371484' AS DateTime2), N'Maestria.webp', 10)
GO
INSERT [dbo].[Products] ([Id], [Name], [Description], [Active], [Price], [DateAdded], [Image], [Stock]) VALUES (N'dd296d6d-1de4-4668-9973-14d99ade799f', N'T-Shirt Software Developer', N'T-Shirt 100% cotton, high durable to wash and high temperatures washing.', 1, CAST(100.00 AS Decimal(18, 2)), CAST(N'2021-10-01T21:33:45.8374889' AS DateTime2), N'camiseta1.jpg', 10)
GO
INSERT [dbo].[Products] ([Id], [Name], [Description], [Active], [Price], [DateAdded], [Image], [Stock]) VALUES (N'86cbe472-b0af-4583-969d-1eb0dd54a556', N'T-Shirt 4 Head White', N'T-Shirt 100% cotton, high durable to wash and high temperatures washing.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2021-10-01T21:33:45.8364739' AS DateTime2), N'Branca 4head.webp', 5)
GO
INSERT [dbo].[Products] ([Id], [Name], [Description], [Active], [Price], [DateAdded], [Image], [Stock]) VALUES (N'53c04d7d-92dd-4a89-9ba6-22e8770e38c0', N'Mug Nightmare', N'Porcelain mug with high-strength thermal printing.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2021-10-01T21:33:45.8373125' AS DateTime2), N'caneca-Nightmare.jpg', 10)
GO
INSERT [dbo].[Products] ([Id], [Name], [Description], [Active], [Price], [DateAdded], [Image], [Stock]) VALUES (N'f5770d7b-5e93-4d27-8567-3c0cd8b8defb', N'T-Shirt Debugar Black', N'T-Shirt 100% cotton, high durable to wash and high temperatures washing.', 1, CAST(75.00 AS Decimal(18, 2)), CAST(N'2021-10-01T21:33:45.8374453' AS DateTime2), N'camiseta4.jpg', 10)
GO
INSERT [dbo].[Products] ([Id], [Name], [Description], [Active], [Price], [DateAdded], [Image], [Stock]) VALUES (N'b87bfc63-0690-411f-886b-4b292fad6ba1', N'T-Shirt Yoda', N'T-Shirt 100% cotton, high durable to wash and high temperatures washing.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2021-10-01T21:33:45.8372072' AS DateTime2), N'Yoda.webp', 10)
GO
INSERT [dbo].[Products] ([Id], [Name], [Description], [Active], [Price], [DateAdded], [Image], [Stock]) VALUES (N'a21043db-4096-4e1d-bcbc-4bc6b847347b', N'Mug No Coffee No Code', N'Porcelain mug with high-strength thermal printing.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2021-10-01T21:33:45.8373550' AS DateTime2), N'caneca4.jpg', 10)
GO
INSERT [dbo].[Products] ([Id], [Name], [Description], [Active], [Price], [DateAdded], [Image], [Stock]) VALUES (N'df35647f-d7fe-40ab-b1d7-4f756f29de4f', N'Mug Batman Black', N'Porcelain mug with high-strength thermal printing.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2021-10-01T21:33:45.8373895' AS DateTime2), N'caneca-Batman.jpg', 8)
GO
INSERT [dbo].[Products] ([Id], [Name], [Description], [Active], [Price], [DateAdded], [Image], [Stock]) VALUES (N'147a1255-dff6-45e7-be5a-5af0f2cc1b79', N'T-Shirt My Yoda', N'T-Shirt 100% cotton, high durable to wash and high temperatures washing.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2021-10-01T21:33:45.8371753' AS DateTime2), N'My Yoda.webp', 10)
GO
INSERT [dbo].[Products] ([Id], [Name], [Description], [Active], [Price], [DateAdded], [Image], [Stock]) VALUES (N'2e744010-9d7d-4293-8db5-602e5f1dddbb', N'T-Shirt Pato', N'T-Shirt 100% cotton, high durable to wash and high temperatures washing.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2021-10-01T21:33:45.8371856' AS DateTime2), N'Pato.webp', 10)
GO
INSERT [dbo].[Products] ([Id], [Name], [Description], [Active], [Price], [DateAdded], [Image], [Stock]) VALUES (N'4dc4a409-12e2-45d7-92f9-6bfd6c2ab930', N'T-Shirt Code Life Gray', N'T-Shirt 100% cotton, high durable to wash and high temperatures washing.', 1, CAST(99.00 AS Decimal(18, 2)), CAST(N'2021-10-01T21:33:45.8374549' AS DateTime2), N'camiseta3.jpg', 3)
GO
INSERT [dbo].[Products] ([Id], [Name], [Description], [Active], [Price], [DateAdded], [Image], [Stock]) VALUES (N'4c6eb181-3896-402f-8c10-6e047d0f7907', N'T-Shirt Xavier School', N'T-Shirt 100% cotton, high durable to wash and high temperatures washing.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2021-10-01T21:33:45.8371972' AS DateTime2), N'Xaviers School.webp', 10)
GO
INSERT [dbo].[Products] ([Id], [Name], [Description], [Active], [Price], [DateAdded], [Image], [Stock]) VALUES (N'27a9d6c7-41fa-49ae-8215-77e6b0810734', N'Mug Turn Coffee into Code', N'Porcelain mug with high-strength thermal printing.', 1, CAST(20.00 AS Decimal(18, 2)), CAST(N'2021-10-01T21:33:45.8375032' AS DateTime2), N'caneca3.jpg', 10)
GO
INSERT [dbo].[Products] ([Id], [Name], [Description], [Active], [Price], [DateAdded], [Image], [Stock]) VALUES (N'66ef4617-37c2-4fd0-a5fc-7cc5c210399a', N'Mug Vegeta', N'Porcelain mug with high-strength thermal printing.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2021-10-01T21:33:45.8373765' AS DateTime2), N'caneca1-Vegeta.jpg', 10)
GO
INSERT [dbo].[Products] ([Id], [Name], [Description], [Active], [Price], [DateAdded], [Image], [Stock]) VALUES (N'70217a50-68c2-441c-b2c4-7e0c91e73be6', N'T-Shirt Kappa', N'T-Shirt 100% cotton, high durable to wash and high temperatures washing.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2021-10-01T21:33:45.8371257' AS DateTime2), N'Kappa.webp', 10)
GO
INSERT [dbo].[Products] ([Id], [Name], [Description], [Active], [Price], [DateAdded], [Image], [Stock]) VALUES (N'9de438f3-bece-4a08-b429-8043cb52a979', N'T-Shirt 4 Head', N'T-Shirt 100% cotton, high durable to wash and high temperatures washing.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2021-10-01T21:33:45.7686079' AS DateTime2), N'4head.webp', 5)
GO
INSERT [dbo].[Products] ([Id], [Name], [Description], [Active], [Price], [DateAdded], [Image], [Stock]) VALUES (N'7d102ad0-b6a4-46ad-8518-8fe8cc899837', N'T-Shirt MacGyver', N'T-Shirt 100% cotton, high durable to wash and high temperatures washing.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2021-10-01T21:33:45.8371381' AS DateTime2), N'MacGyver.webp', 10)
GO
INSERT [dbo].[Products] ([Id], [Name], [Description], [Active], [Price], [DateAdded], [Image], [Stock]) VALUES (N'079a4fbf-adcc-4574-b89e-92de224ef047', N'T-Shirt Rick And Morty', N'T-Shirt 100% cotton, high durable to wash and high temperatures washing.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2021-10-01T21:33:45.8372424' AS DateTime2), N'Rick And Morty.webp', 5)
GO
INSERT [dbo].[Products] ([Id], [Name], [Description], [Active], [Price], [DateAdded], [Image], [Stock]) VALUES (N'8be38d66-9448-4bcc-9f77-93e693f6a1f8', N'Mug Big Bang Theory', N'Porcelain mug with high-strength thermal printing.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2021-10-01T21:33:45.8374010' AS DateTime2), N'caneca-bbt.webp', 0)
GO
INSERT [dbo].[Products] ([Id], [Name], [Description], [Active], [Price], [DateAdded], [Image], [Stock]) VALUES (N'b2eff88b-a4c5-4ca3-83b2-93f9489160c7', N'Mug Geeks', N'Porcelain mug with high-strength thermal printing.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2021-10-01T21:33:45.8374212' AS DateTime2), N'caneca-Geeks.jpg', 10)
GO
INSERT [dbo].[Products] ([Id], [Name], [Description], [Active], [Price], [DateAdded], [Image], [Stock]) VALUES (N'903827e3-2dd4-4add-9fdc-a72f537a48ef', N'T-Shirt Tilt White', N'T-Shirt 100% cotton, high durable to wash and high temperatures washing.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2021-10-01T21:33:45.8366622' AS DateTime2), N'Branco Tiltado.webp', 10)
GO
INSERT [dbo].[Products] ([Id], [Name], [Description], [Active], [Price], [DateAdded], [Image], [Stock]) VALUES (N'ac38ad47-c463-4f65-9d1b-af0738a7579e', N'T-Shirt Code Life Black', N'T-Shirt 100% cotton, high durable to wash and high temperatures washing.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2021-10-01T21:33:45.8371639' AS DateTime2), N'camiseta2.jpg', 10)
GO
INSERT [dbo].[Products] ([Id], [Name], [Description], [Active], [Price], [DateAdded], [Image], [Stock]) VALUES (N'39608034-d9aa-4a9b-bdcc-b110e0ca7d61', N'T-Shirt Rick And Morty 2', N'T-Shirt 100% cotton, high durable to wash and high temperatures washing.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2021-10-01T21:33:45.8372317' AS DateTime2), N'Rick And Morty Captured.webp', 10)
GO
INSERT [dbo].[Products] ([Id], [Name], [Description], [Active], [Price], [DateAdded], [Image], [Stock]) VALUES (N'df5d5274-ac5d-4001-bd21-b6cb19b1eff5', N'Mug Mushroom', N'Porcelain mug with high-strength thermal printing.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2021-10-01T21:33:45.8374117' AS DateTime2), N'caneca-cogumelo.webp', 10)
GO
INSERT [dbo].[Products] ([Id], [Name], [Description], [Active], [Price], [DateAdded], [Image], [Stock]) VALUES (N'd56c92dd-5138-4261-82cb-bafa84e24c0f', N'Mug Batman', N'Porcelain mug with high-strength thermal printing.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2021-10-01T21:33:45.8373670' AS DateTime2), N'caneca1--batman.jpg', 5)
GO
INSERT [dbo].[Products] ([Id], [Name], [Description], [Active], [Price], [DateAdded], [Image], [Stock]) VALUES (N'2433c7d5-5543-416c-8397-bb7d973f7abf', N'T-Shirt Say My Name', N'T-Shirt 100% cotton, high durable to wash and high temperatures washing.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2021-10-01T21:33:45.8372524' AS DateTime2), N'Say My Name.webp', 10)
GO
INSERT [dbo].[Products] ([Id], [Name], [Description], [Active], [Price], [DateAdded], [Image], [Stock]) VALUES (N'070037a2-c563-4e24-87aa-bcd4b6a91876', N'T-Shirt Tilt', N'T-Shirt 100% cotton, high durable to wash and high temperatures washing.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2021-10-01T21:33:45.8366375' AS DateTime2), N'tiltado.webp', 10)
GO
INSERT [dbo].[Products] ([Id], [Name], [Description], [Active], [Price], [DateAdded], [Image], [Stock]) VALUES (N'84667a5d-2bf6-4c2b-a4d5-c0f6b441e5f8', N'Mug IronMan', N'Porcelain mug with high-strength thermal printing.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2021-10-01T21:33:45.8374309' AS DateTime2), N'caneca-ironman.jpg', 8)
GO
INSERT [dbo].[Products] ([Id], [Name], [Description], [Active], [Price], [DateAdded], [Image], [Stock]) VALUES (N'18a30989-9cf6-435d-8c61-c49826ea03a6', N'T-Shirt Support', N'T-Shirt 100% cotton, high durable to wash and high temperatures washing.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2021-10-01T21:33:45.8372661' AS DateTime2), N'support.webp', 10)
GO
INSERT [dbo].[Products] ([Id], [Name], [Description], [Active], [Price], [DateAdded], [Image], [Stock]) VALUES (N'5d953a57-06d4-40d7-a84f-c864d7083274', N'Mug Joker Wanted', N'Porcelain mug with high-strength thermal printing.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2021-10-01T21:33:45.8372883' AS DateTime2), N'caneca-joker Wanted.jpg', 10)
GO
INSERT [dbo].[Products] ([Id], [Name], [Description], [Active], [Price], [DateAdded], [Image], [Stock]) VALUES (N'09d30aa9-0580-4b08-a3dc-d651936fc53d', N'Mug Ozob', N'Porcelain mug with high-strength thermal printing.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2021-10-01T21:33:45.8373220' AS DateTime2), N'caneca-Ozob.webp', 10)
GO
INSERT [dbo].[Products] ([Id], [Name], [Description], [Active], [Price], [DateAdded], [Image], [Stock]) VALUES (N'f4f2f70c-3df6-46f6-83c9-da53a2053e87', N'T-Shirt Heisenberg', N'T-Shirt 100% cotton, high durable to wash and high temperatures washing.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2021-10-01T21:33:45.8370964' AS DateTime2), N'Heisenberg.webp', 10)
GO
INSERT [dbo].[Products] ([Id], [Name], [Description], [Active], [Price], [DateAdded], [Image], [Stock]) VALUES (N'931bf19d-6292-4309-96cb-e8d133e45e26', N'T-Shirt Quack', N'T-Shirt 100% cotton, high durable to wash and high temperatures washing.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2021-10-01T21:33:45.8372206' AS DateTime2), N'Quack.webp', 10)
GO
INSERT [dbo].[Products] ([Id], [Name], [Description], [Active], [Price], [DateAdded], [Image], [Stock]) VALUES (N'9669789a-f9f7-4fbe-8c4c-ebf50a0aff4b', N'Mug Programmer Code', N'Porcelain mug with high-strength thermal printing.', 1, CAST(15.00 AS Decimal(18, 2)), CAST(N'2021-10-01T21:33:45.8374736' AS DateTime2), N'caneca2.jpg', 10)
GO
INSERT [dbo].[Products] ([Id], [Name], [Description], [Active], [Price], [DateAdded], [Image], [Stock]) VALUES (N'a5fd7e4f-cff6-4de8-8ed3-f38e09d27adb', N'T-Shirt Try Hard', N'T-Shirt 100% cotton, high durable to wash and high temperatures washing.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2021-10-01T21:33:45.8372771' AS DateTime2), N'Tryhard.webp', 10)
GO
INSERT [dbo].[Products] ([Id], [Name], [Description], [Active], [Price], [DateAdded], [Image], [Stock]) VALUES (N'5251c7f6-1a02-4e49-88b4-f6a9c374a020', N'Mug Joker', N'Porcelain mug with high-strength thermal printing.', 1, CAST(50.00 AS Decimal(18, 2)), CAST(N'2021-10-01T21:33:45.8373013' AS DateTime2), N'caneca-Joker.jpg', 5)
GO
INSERT [dbo].[Products] ([Id], [Name], [Description], [Active], [Price], [DateAdded], [Image], [Stock]) VALUES (N'af6f98a1-70e5-45b8-87c4-f8c1bd95972d', N'Mug Star Bugs Coffee', N'Porcelain mug with high-strength thermal printing.', 1, CAST(20.00 AS Decimal(18, 2)), CAST(N'2021-10-01T21:33:45.8374642' AS DateTime2), N'caneca1.jpg', 10)
GO



SELECT 'DATABASE CREATED !!!  :D'