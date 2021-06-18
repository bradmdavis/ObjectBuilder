using System;
using System.Collections;
using System.Text;

namespace ObjectBuilder
{
    public class Procedure
    {
        private string name;
        private System.Collections.Generic.List<Column> columns;

        public Procedure(string Name, System.Collections.Generic.List<Column> Columns)
        { 
            name = Name;
            columns = Columns;
        }


        public string GetSaveProcedure()
        {
                System.Text.StringBuilder s =  new System.Text.StringBuilder(256);
                s.AppendFormat("CREATE PROCEDURE dbo.{0}Save\r\n", name);
                s.Append(GetParameters());
                s.Append("IF ISNULL(@Id, 0) = 0\r\n");
                s.Append(GetInsert());
                s.Append("ELSE\r\n");
                s.Append(GetUpdate());
                s.Append("GO\r\n");

                return s.ToString();

        }

        public string GetInsertProcedure
        {
            get 
            {
                System.Text.StringBuilder s =  new System.Text.StringBuilder(256);
                s.AppendFormat("CREATE PROCEDURE dbo.{0}Insert\r\n", name);
                s.Append(GetParameters());
                s.Append(GetInsert());
                s.Append("GO\r\n");

                return s.ToString();
            }
        }

        private string GetInsert(){
            System.Text.StringBuilder s = new System.Text.StringBuilder(256);
            s.Append("BEGIN\r\n");
            s.AppendFormat("INSERT INTO [{0}](\r\n", name);
            foreach (Column C in columns)
            {
                s.AppendFormat("    {0},\r\n", C.Name);
            }
            s.Remove(s.Length - 3, 3);
            s.Append("\r\n");
            s.Append(")VALUES(\r\n");
            foreach (Column C in columns)
            {
                s.AppendFormat("    @{0},\r\n", C.Name);
            }
            s.Remove(s.Length - 3, 3);
            s.Append("\r\n");
            s.Append(")\r\n");
            s.Append("SET @Id = SCOPE_IDENTITY()\r\n");
            s.Append("END\r\n");
            return s.ToString();
        }


        public string GetParameters()
        {
            System.Text.StringBuilder s = new System.Text.StringBuilder(256);
            s.Append("    @Id int output,\r\n");
            foreach (Column C in columns)
            {
                s.AppendFormat("    @{0} {1},\r\n", C.Name, C.SQLDataType);
            }
            s.Remove(s.Length - 3, 3);
            s.Append("\r\n");
            s.Append("AS\r\n");

            return s.ToString();

        }

        public string GetUpdateProcedure
        {
            get
            {
                System.Text.StringBuilder s = new System.Text.StringBuilder(256);
                s.AppendFormat("CREATE PROCEDURE dbo.{0}Update\r\n", name);
                s.Append(GetParameters());
                s.Append(GetUpdate());
                s.Append("GO\r\n");

                return s.ToString();
            }
        }

        public string GetUpdate()
        {
            System.Text.StringBuilder s = new System.Text.StringBuilder(256);
            s.Append("BEGIN\r\n");
            s.AppendFormat("    UPDATE [{0}] SET\r\n", name);

            foreach (Column C in columns)
            {
                s.AppendFormat("        {0} = @{0},\r\n", C.Name);
            }

            s.Remove(s.Length - 3, 3);
            s.Append("\r\n");
            s.Append("    WHERE \r\n");
            s.AppendFormat("        {0}Id = @Id;\r\n", name);
            s.Append("END\r\n");
            return s.ToString();
        }


        public string GetDelete
        {
            get
            {
                System.Text.StringBuilder s = new System.Text.StringBuilder(256);
                s.AppendFormat("CREATE PROCEDURE dbo.Delete{0}\r\n", name);
                s.Append("    @Id int \r\n");
                s.Append("AS\r\n");
                s.Append("BEGIN\r\n");
                s.AppendFormat("    DELETE FROM [{0}]\r\n", name);
                s.Append("    WHERE \r\n");
                s.AppendFormat("        {0}Id = @Id;\r\n", name);
                s.Append("END\r\n");
                s.Append("GO\r\n");

                return s.ToString();

            }
        }

        public string GetSelect
        {
            get
            {
                System.Text.StringBuilder s = new System.Text.StringBuilder(256);
                s.AppendFormat("CREATE PROCEDURE dbo.{0}_Get_By_Id\r\n", name);
                s.Append("    @Id int \r\n");
                s.Append("AS\r\n");
                s.Append("BEGIN\r\n");
                s.Append("    SELECT");
                foreach(Column C in columns)
                {
                    s.AppendFormat("        {0},\r\n", C.Name);
                }
                s.Remove(s.Length - 3, 3);
                s.Append("\r\n");
                s.AppendFormat("    FROM \r\n [{0}] \r\n", name);
                s.Append("    WHERE \r\n");
                s.AppendFormat("        {0}Id = @Id;\r\n", name);
                s.Append("END\r\n");
                s.Append("GO\r\n");

                return s.ToString();
            }
        }

        public static string GetProcedure(string tableName, System.Collections.Generic.List<Column> columns) {
            Procedure p = new Procedure(tableName, columns);

            System.Text.StringBuilder s = new System.Text.StringBuilder(256);

            s.AppendFormat("{0} \r\n", p.GetInsertProcedure);
            s.AppendFormat("{0} \r\n", p.GetDelete);
            s.AppendFormat("{0} \r\n", p.GetSelect);
            s.AppendFormat("{0} \r\n", p.GetUpdateProcedure);
            s.AppendFormat("{0} \r\n", p.GetSaveProcedure());

            return s.ToString();
        }

    }
}
