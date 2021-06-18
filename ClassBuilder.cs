using System;
using System.Collections;
using System.Text;
using System.Data;
using Microsoft.ApplicationBlocks.Data;


namespace ObjectBuilder {
    public class ClassBuilder {

        public static string Build(String TableName,  System.Collections.Generic.List<Column> columns) {


            System.IO.StreamReader sr = new System.IO.StreamReader(@"D:\Source\ObjectBuilder\template.cs");
            string s = sr.ReadToEnd();
            s = s.Replace("Template", TableName);
            s = s.Replace("template", TableName.ToLower());

            s = s.Replace("//Private Properties", BuildPrivateProperties(columns));
            s = s.Replace("//Public Accessors", BuildPublicProperties(columns));
            s = s.Replace("//Enum Values", BuildEnumValues(columns));


            //System.IO.StreamWriter SW;
            //SW = System.IO.File.CreateText("classes\\" + Utility.RemoveNonAlpha(TableName) + ".cs");
            //SW.WriteLine(s);

            //SW.Close();

            //BuildStoredProcedures(TableName, columns);
            return s;
        }

        public static string BuildProperties(System.Collections.Generic.List<Column> columns) {
            System.Text.StringBuilder properties = new System.Text.StringBuilder(256);

            properties.Append(BuildPrivateProperties(columns));
            properties.Append("\r\n");
            properties.Append(BuildPublicProperties(columns));
            return properties.ToString();
        }


        public static string BuildPublicProperties(System.Collections.Generic.List<Column> columns) {
            System.Text.StringBuilder publicProperties = new System.Text.StringBuilder(256);

            foreach (Column c in columns) {
                publicProperties.Append(c.GetPublicProperty);
            }
            return publicProperties.ToString();
        }

        public static string BuildPrivateProperties(System.Collections.Generic.List<Column> columns) {

            System.Text.StringBuilder privateProperties = new System.Text.StringBuilder(256);

            foreach (Column c in columns) {
                privateProperties.Append(c.GetPrivateProperty);
            }
            return privateProperties.ToString();
        }

        public static void BuildStoredProcedures(String Name, System.Collections.Generic.List<Column> Columns) {
            Procedure p = new Procedure(Name, Columns);
            Console.WriteLine(p.GetInsertProcedure);
            Console.WriteLine(p.GetDelete);
            Console.WriteLine(p.GetSelect);
            Console.WriteLine(p.GetUpdateProcedure);
            Console.WriteLine(p.GetSaveProcedure());
        }

        public static string BuildEnumValues(System.Collections.Generic.List<Column> columns) {
            StringBuilder s = new StringBuilder(256);
            int Counter = 0;

            foreach (Column c in columns) {
                Counter += 1;
                s.Append(string.Format("{0} = {1},\r\n", Utility.RemoveNonAlpha(c.Name), Counter));
            }

            //Remove Last ","
            s.Remove(s.Length - 3, 3);

            return s.ToString();
        }

    }
}
