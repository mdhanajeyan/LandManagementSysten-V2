
INSERT INTO [dbo].[Role]
           ([Name])
     VALUES
           ('Admin')
		   ,('Accounts')
		   ,('Legal')
GO
select * from [Role]



INSERT INTO [dbo].[DocumentType]
           ([DocumentTypeName]
           ,[IsDocumentTypeActive])
     VALUES
           ( 'Advance' , 1),
		   ( 'GPA' , 1),
		   ( 'Agreement To Sell' , 1),
		   ( 'Addendum To Agreement' , 1),
		   ( 'Sale Agreement' , 1)
GO

select * from DocumentType


INSERT INTO [dbo].[ScreenList]
           ([ModuleId]
           ,[ScreenName]
           ,[ScreenId])
     VALUES
           (1, 'Company', 1),
           (1, 'Vendor', 2),
		   (1, 'Bank', 3),
		   (1, 'Cash', 4),
		   (1, 'ExpenseHead', 5),
		   (1, 'Taluk', 6),
		   (1, 'Hobli', 7),
		   (1, 'Village', 8),
		   (1, 'PropChecklistMaster', 9),
		   (1, 'PropertyType', 10),
		   (2, 'Property', 11),
		   (2, 'PropertyCheckList', 12),
		   (2, 'PropertyDeals', 13),
		   (2, 'MergeProperties', 14),
		   (3, 'Payments', 15),
		   (3, 'FundTransfer', 16),
		   (3, 'Receipt', 17),
		   (3, 'SaleDeal', 18),
		   (4, 'UserInfo', 19),
		   (4, 'UserRole', 20),
		   (4, 'Role', 21),
		   (4, 'RolePermission', 22),
		   (4,'ViewLogs', 23),
		   (5,'CurrentMonthPaymentReport',24),
		   (5,'PropertyChecklistReport',25),
		   (5,'PartyStatementOfAccountReport',26),
		   (5,'PostDatedCheckReport',27),
           (5,'DealReport',28),
		   (1,'Party',29)
GO
Select * from [ScreenList]

GO

INSERT INTO [dbo].[UserInfo]
           ([UserName]
           ,[loginName]
           ,[UserPassword]
           ,[Email]
           ,[MobileNo]
           ,[IsActive]
           ,[IsAdmin]
)
     VALUES
           ('admin','User Admin','admin123', 'admin@admin.com', '9986626260', 1, 1),
		   ('accounts','User Accounts','accounts123', 'accounts@accounts.com', '9986626260', 1, 1),
		   ('legal','User Legal','legal123', 'legal@legal.com', '9986626260', 1, 1)
GO


INSERT INTO [dbo].[UserRole]
           ([UserInfoId]
           ,[RoleId]
           )
     VALUES
            (1,1)
		   ,(2,2)
		   ,(3,3)
GO



INSERT INTO [dbo].[RolePermission]
           ([RoleInfoId]
           ,[ScreenId]
           ,[OptionId])
     VALUES
            (2,1,1)
		   ,(2,2,1)
		   ,(2,3,1)
		   ,(2,4,1)
		   ,(2,5,1)
		   ,(2,6,1)
		   ,(2,7,1)
		   ,(2,8,1)
		   ,(2,9,1)
		   ,(2,10,1)
		   ,(2,11,1)
		   ,(2,12,1)
		   ,(2,13,1)
		   ,(2,14,1)
		   ,(2,15,1)
		   ,(2,16,1)
		   ,(2,17,1)
		   ,(2,18,1)
		   ,(2,19,1)
		   ,(2,20,1)
		   ,(2,21,1)
		   ,(2,22,1)
		   ,(2,23,1)
		   ,(2,24,1)
		   ,(2,25,1)
		   ,(2,26,1)
		   ,(2,27,1)
		   ,(2,28,1)
		   
GO
INSERT INTO [dbo].[AccountType]
           ([AccountTypeName])
     VALUES
           ('Saving')
		   ,('Current')
GO

