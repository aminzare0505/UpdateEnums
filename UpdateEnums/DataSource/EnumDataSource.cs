using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpdateEnums.Utility;
//using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UpdateEnums.DataSource
{
   public class EnumDataSource
    {
        
        public void InsertInDB(string connectionString, string enumName, List<EnumValueDto> enumValueDtos,string error)
        {

            string CheckTableQueryString = @"IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[enm].[" + enumName + @"]') AND type in (N'U'))
                                                         DROP TABLE[enm].[" + enumName + @"]";
            string CreateTableQueryString = @"CREATE TABLE[enm].[" + enumName + @"](
                                                   [ID][int] NOT NULL,
                                                  [Title] [nvarchar](500) NOT NULL,
                                                CONSTRAINT[PK_+" + enumName + @"] PRIMARY KEY CLUSTERED
                                                (
                                                 [ID] ASC
                                                )WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)       ON[PRIMARY]
                                                ) ON[PRIMARY]";

            string InsertDataQueryString = "";
            foreach (var item in enumValueDtos)
            {
                InsertDataQueryString += "INSERT INTO [enm].[" + enumName + "]([ID],[Title]) VALUES(" + item.Key + ",N'" + item.Value + "')";
            }
            try
            {


                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand CheckTablecommand = new SqlCommand(CheckTableQueryString, connection);
                SqlCommand CreateTablecommand = new SqlCommand(CreateTableQueryString, connection);
                SqlCommand InsertDatacommand = new SqlCommand(InsertDataQueryString, connection);
                connection.Open();
                CheckTablecommand.ExecuteNonQuery();
                CreateTablecommand.ExecuteNonQuery();
                InsertDatacommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                error += enumName + ex.Message;
            }

        }
        public void CheckScehma(string connectionString)
        {

            string CheckScehmaQueryString = @"If EXISTS (SELECT 1 WHERE SCHEMA_ID('enm') IS  NULL)
                                                SELECT 1 AS Value
                                                ELSE
                                                SELECT 0 AS Value ";
            string CreateScehmaQueryString = @"Create SCHEMA enm";
            string Ok = null;
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand CheckTablecommand = new SqlCommand(CheckScehmaQueryString, connection);
            SqlCommand CreateScehmacommand = new SqlCommand(CreateScehmaQueryString, connection);
            connection.Open();
            SqlDataReader DataReader = CheckTablecommand.ExecuteReader();
            while (DataReader.Read())
                Ok = DataReader["Value"].ToString();
            connection.Close();
            if (Ok == "1")
            {
                connection.Open();
                CreateScehmacommand.ExecuteNonQuery();
                connection.Close();
            }

        }
    }
}
