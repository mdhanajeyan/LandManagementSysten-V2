-- disable referential integrity
EXEC sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL' 
GO 

EXEC sp_MSForEachTable 'DELETE FROM ?' 
GO 

EXEC sp_MSForEachTable 'DBCC CHECKIDENT(''?'', RESEED, 0)'

-- enable referential integrity again 
EXEC sp_MSForEachTable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL' 
GO


