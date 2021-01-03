USE [PayPoint]
GO

/****** Object:  View [dbo].[vw_QuarterlyData]    Script Date: 3/18/2020 1:14:57 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE VIEW [dbo].[vw_QuarterlyData]
AS
SELECT dbo.Invoices.Reference,dbo.Allocations.AllocationName,dbo.ExpenseTypes.ExpenseName,dbo.Currencies.CurrencyAbbr,dbo.Currencies.CurrencySymbol,dbo.Departments.DepartmentName,dbo.BusinessUnits.UnitName,dbo.Vendors.VendorName,YEAR(dbo.Invoices.CreatedOn) AS SaleYear,
       (CASE WHEN DATEPART(QQ,dbo.Invoices.CreatedOn)=1 THEN '1st QUARTER'
            WHEN DATEPART(QQ,dbo.Invoices.CreatedOn)=2 THEN '2nd QUARTER'
            WHEN DATEPART(QQ,dbo.Invoices.CreatedOn)=3 THEN '3rd QUARTER'
            WHEN DATEPART(QQ,dbo.Invoices.CreatedOn)=4 THEN '4th QUARTER'
                  END) AS Sale_QUARTER,
       MAX(DateName(MOnth,dbo.Invoices.CreatedOn)) AS Sale_MONTH
  FROM dbo.Invoices
  INNER JOIN
         dbo.Vendors ON dbo.Invoices.VendorId = dbo.Vendors.VendorId INNER JOIN
         dbo.Currencies ON dbo.Invoices.CurrencyId = dbo.Currencies.CurrencyId INNER JOIN
		  dbo.BusinessUnits ON dbo.Invoices.UnitId = dbo.BusinessUnits.UnitId INNER JOIN
         dbo.ExpenseTypes ON dbo.Invoices.ExpId= dbo.ExpenseTypes.ExpId INNER JOIN
		  dbo.Allocations ON dbo.Invoices.AllocationId= dbo.Allocations.AllocationId  INNER JOIN
		   dbo.Departments ON dbo.Departments.DepartmentId= dbo.Invoices.DepartmentId
     GROUP BY YEAR(dbo.Invoices.CreatedOn), DATEPART(QQ,dbo.Invoices.CreatedOn),
               DATEPART(MONTH,dbo.Invoices.CreatedOn), dbo.Invoices.Reference, dbo.Allocations.AllocationName,dbo.ExpenseTypes.ExpenseName,dbo.Currencies.CurrencyAbbr,dbo.Currencies.CurrencySymbol,dbo.BusinessUnits.UnitName,dbo.Vendors.VendorName,dbo.Departments.DepartmentName
     
GO


