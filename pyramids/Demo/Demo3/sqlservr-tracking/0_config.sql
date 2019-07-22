EXEC sp_configure 'remote admin connections', 0;
EXEC sp_configure 'user connections', 8;  
GO

RECONFIGURE;
GO
