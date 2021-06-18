using System;
using System.Collections;
using System.Text;
using Microsoft.ApplicationBlocks.Data;

namespace ObjectBuilder
{
    public class Column
    {
        private string name;
        private string dataType;
        private string sqlDataType;

        public string Name
        { 
            get{
                return name;
            }
            set{ name = value; }
        }

        public string DataType
        {
            get{ return dataType; }
            set{ dataType = value; }
        }


        public string SQLDataType
        {
            get { return sqlDataType; }
            set 
            {
                if (value == "nvarchar" || value == "varchar")
                {
                    sqlDataType = value + "(50)";
                }
                else
                {
                    sqlDataType = value;
                }
            }
        }
  
        public string GetPublicProperty
        {
            get 
            {
                System.Text.StringBuilder s = new System.Text.StringBuilder(256);


                s.AppendFormat("public {0} {1}\r\n", dataType,  Utility.RemoveNonAlpha(name));
                s.Append("{\r\n");
                s.AppendFormat("    get{{\r\nreturn {0};\r\n}}\r\n", GetPrivatePropertyName);
                s.AppendFormat("    set{{\r\n{0} = value;\r\n}}\r\n", GetPrivatePropertyName);
                s.Append("}\r\n");
                return s.ToString();
            }      
        }

        public string GetPrivateProperty
        {
            get
            {
                System.Text.StringBuilder s = new System.Text.StringBuilder(256);
                s.AppendFormat("private {0} {1};\r\n", dataType, GetPrivatePropertyName);
                return s.ToString();
            }        
        }

        private string GetPrivatePropertyName
        {
            get
            {
                    return Utility.RemoveNonAlpha(name.Substring(0,1).ToLower() + name.Substring(1));
            }
        }

        public static System.Collections.Generic.List<Column> GetColumns(string tableName)
        {
            
            System.Text.StringBuilder SQL = new System.Text.StringBuilder(256);
            SQL.AppendFormat("SELECT Top 1 * FROM dbo.[{0}]", tableName);

            //string ConnectionString = "Trusted_Connection=yes;Data Source=WEBSQL;Initial Catalog=FlexAmerica;";

            string ConnectionString = @"Data Source=.\SQLEXPRESS; Integrated Security=True; AttachDbFilename=C:\INETPUB\WWWROOT\DIRTYMONKEYS\APP_DATA\CORE.MDF;Initial Catalog=Core";

            System.Data.SqlClient.SqlDataReader dr;
            dr = SqlHelper.ExecuteReader(ConnectionString, System.Data.CommandType.Text, SQL.ToString());

            System.Collections.Generic.List<Column> columns = new System.Collections.Generic.List<Column>();
            int i = 0;
            while (i < dr.FieldCount) {
                if (i != 0) {

                    Column c = new Column();
                    c.Name = dr.GetName(i);
                    c.DataType = dr.GetFieldType(i).FullName;
                    c.SQLDataType = dr.GetDataTypeName(i);

                    columns.Add(c);
                }
                i++;
            }

            dr.Close();

            return columns;
        }


    }
}
