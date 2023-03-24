using mssqlEncryption.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Data.SqlClient;
using System.Data;

namespace mssqlEncryption
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            DatabaseContext context = new DatabaseContext();
            context.Database.SetConnectionString("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=EncryptionDb;Integrated Security=True;");
            context.Database.EnsureCreated();
            //Example1_Password(context)

            Example2_Cer_Insert(context);
            Example2_Cer_Select(context);

            //Example3_Cer_WriteImage(context);
            //Example3_Cer_ReadImage(context);
        }

        public static void Example1_Password(DatabaseContext context)
        {
            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                try
                {
                    var SYMMETRICKEY = "MySymmetricKey";
                    var PASSWORD = "MyPassword";
                    //var sql = String.Format($"OPEN SYMMETRIC KEY {SYMMETRICKEY} DECRYPTION BY CERTIFICATE {1}");
                    var sql = $"OPEN SYMMETRIC KEY {SYMMETRICKEY} DECRYPTION BY PASSWORD = '{PASSWORD}'";
                    context.Database.ExecuteSqlRaw(sql);

                    var CardId = "1252233215152";
                    var FirstName = "abc";
                    var LastName = "def";
                    sql = @$"INSERT INTO [dbo].[Member]
    ([FirstName]
    ,[LastName]
    ,[CardId]
    ,[FirstNameEnc]
    ,[LastNameEnc]
    ,[CardIdEnc]
	,[Delflag])
VALUES
    (N'{FirstName}'
    ,N'{LastName}'
    ,N'{CardId}'
    ,EncryptByKey( Key_GUID('{SYMMETRICKEY}'), CONVERT(varchar,N'{FirstName}') )
    ,EncryptByKey( Key_GUID('{SYMMETRICKEY}'), CONVERT(varchar,N'{LastName}') )
    ,EncryptByKey( Key_GUID('{SYMMETRICKEY}'), CONVERT(varchar,N'{CardId}') )
    ,0)
";

                    context.Database.ExecuteSqlRaw(sql);

                    sql = $"CLOSE SYMMETRIC KEY {SYMMETRICKEY}";
                    context.Database.ExecuteSqlRaw(sql);
                    dbContextTransaction.Commit();
                }
                catch (Exception exp)
                {
                    var x = exp.ToString(); //do something with exception
                }
            }
        }
        public static void Example2_Cer_Insert(DatabaseContext context)
        {
            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                try
                {
                    var SYMMETRICKEY = "SymmetricKeyCer";
                    var CERTIFICATE = "MyCertificate";
                    //var sql = String.Format($"OPEN SYMMETRIC KEY {SYMMETRICKEY} DECRYPTION BY CERTIFICATE {1}");
                    var sql = $"OPEN SYMMETRIC KEY {SYMMETRICKEY} DECRYPTION BY CERTIFICATE {CERTIFICATE}";
                    context.Database.ExecuteSqlRaw(sql);

                    var CardId = "1252233215152";
                    var FirstName = "abc";
                    var LastName = "def";
                    sql = @$"INSERT INTO [dbo].[Member]
    ([FirstName]
    ,[LastName]
    ,[CardId]
    ,[FirstNameEnc]
    ,[LastNameEnc]
    ,[CardIdEnc]
	,[Delflag])
VALUES
    (N'{FirstName}'
    ,N'{LastName}'
    ,N'{CardId}'
    ,EncryptByKey( Key_GUID('{SYMMETRICKEY}'), CONVERT(varchar,N'{FirstName}') )
    ,EncryptByKey( Key_GUID('{SYMMETRICKEY}'), CONVERT(varchar,N'{LastName}') )
    ,EncryptByKey( Key_GUID('{SYMMETRICKEY}'), CONVERT(varchar,N'{CardId}') )
    ,0)
";

                    context.Database.ExecuteSqlRaw(sql);

                    sql = $"CLOSE SYMMETRIC KEY {SYMMETRICKEY}";
                    context.Database.ExecuteSqlRaw(sql);
                    dbContextTransaction.Commit();
                }
                catch (Exception exp)
                {
                    var x = exp.ToString(); //do something with exception
                }
            }
        }
        public static void Example2_Cer_Select(DatabaseContext context)
        {
            var SYMMETRICKEY = "SymmetricKeyCer";
            var CERTIFICATE = "MyCertificate";
            //var sql = String.Format($"OPEN SYMMETRIC KEY {SYMMETRICKEY} DECRYPTION BY CERTIFICATE {1}");

            var sql = @$"OPEN SYMMETRIC KEY {SYMMETRICKEY} DECRYPTION BY CERTIFICATE {CERTIFICATE}
SELECT TOP (1000) [Id]
      ,[FirstName]
      ,[LastName]
      ,[CardId]
	  ,CONVERT(varchar, DecryptByKey(FirstNameEnc)) AS FirstNameEnc
	  ,CONVERT(varchar, DecryptByKey(LastNameEnc)) AS LastNameEnc
	  ,CONVERT(varchar, DecryptByKey(CardIdEnc)) AS CardIdEnc
      ,[DelFlag]
      ,[CreatedBy]
      ,[CreatedDate]
      ,[UpdatedBy]
      ,[UpdatedDated]
  FROM [EncryptionDb].[dbo].[Member]
CLOSE SYMMETRIC KEY {SYMMETRICKEY};
";
            var listMember = context.Member.FromSqlRaw(sql).AsQueryable();
            foreach (var item in listMember)
            {
                Console.Write(item.Id);
                Console.Write(item.CardIdEnc);
                Console.Write(item.FirstNameEnc);
                Console.Write(item.LastNameEnc);
            }
        }
        public static void Example3_Cer_WriteImage(DatabaseContext context)
        {

            var SYMMETRICKEY = "SymmetricKeyCer";
            var CERTIFICATE = "MyCertificate";
            var sql = @$"OPEN SYMMETRIC KEY {SYMMETRICKEY} DECRYPTION BY CERTIFICATE {CERTIFICATE}

            INSERT INTO ProfileImages (Image, ImageEnc) VALUES (@ImageData,EncryptByKey( Key_GUID('{SYMMETRICKEY}'), @ImageData) )

            CLOSE SYMMETRIC KEY {SYMMETRICKEY};
            ";

            byte[] imageData = File.ReadAllBytes(@".\images\start_button.png");
            //string base64ImageData = Convert.ToBase64String(imageData);
            var sqlParam = new SqlParameter("@ImageData", SqlDbType.VarBinary) { Value = imageData };

            context.Database.ExecuteSqlRaw(sql, sqlParam);

            // read image
            //byte[] imageData = File.ReadAllBytes(@".\images\start_button.png");
            //Entites.ProfileImages image = new Entites.ProfileImages {Image=imageData };
            //context.ProfileImages.Add(image);
            //context.SaveChanges();
        }
        public static void Example3_Cer_ReadImage(DatabaseContext context)
        {
            var SYMMETRICKEY = "SymmetricKeyCer";
            var CERTIFICATE = "MyCertificate";
            //var sql = String.Format($"OPEN SYMMETRIC KEY {SYMMETRICKEY} DECRYPTION BY CERTIFICATE {1}");

            var sql = @$"OPEN SYMMETRIC KEY {SYMMETRICKEY} DECRYPTION BY CERTIFICATE {CERTIFICATE}
SELECT [Id]
      ,[Image]
	  ,DecryptByKey(ImageEnc) AS ImageEnc
  FROM [EncryptionDb].[dbo].[ProfileImages]
CLOSE SYMMETRIC KEY {SYMMETRICKEY};
";
            var profileImg = context.ProfileImages.FromSqlRaw(sql).AsEnumerable().FirstOrDefault();//.Select(r => new { r.Id,r.Image,r.ImageEnc });
            if (profileImg != null)
            {
                byte[] imageData = profileImg.Image;
                byte[] imageDataEnc = profileImg.ImageEnc;
                File.WriteAllBytes(@"D:\temp.png", imageData);
                File.WriteAllBytes(@"D:\tempEnc.png", imageDataEnc);
            }
        }
    }
}
