CREATE SYMMETRIC KEY MySymmetricKey
WITH ALGORITHM = AES_128
ENCRYPTION BY PASSWORD = 'MyPassword';


BACKUP CERTIFICATE MySymmetricKey 
TO FILE = 'C:\KeyBackups\MySymmetricKey.key' 
WITH PRIVATE KEY ( 
    FILE = 'C:\KeyBackups\MySymmetricKey.pvk', 
    ENCRYPTION BY PASSWORD = 'MyPrivateKeyPassword'
);


CREATE CERTIFICATE MySymmetricKey 
FROM FILE = 'C:\KeyBackups\MySymmetricKey.key' 
WITH PRIVATE KEY ( 
    FILE = 'C:\KeyBackups\MySymmetricKey.pvk', 
    DECRYPTION BY PASSWORD = 'MyPrivateKeyPassword'
);

SELECT * FROM sys.symmetric_keys WHERE name = 'MySymmetricKey'