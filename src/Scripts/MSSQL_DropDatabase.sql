use master; 
alter database DATABASENAME set single_user with rollback immediate; 
drop database DATABASENAME