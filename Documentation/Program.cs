using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Api;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Documentation
{
    class Program
    {
        static void Main(string[] args)
        {
            BrowserOptions options = new BrowserOptions();
            WebClient webClient = new WebClient(options);

            //UCI
            XrmApp app = new XrmApp(webClient);
            PropertyInfo[] elements = app.GetType().GetProperties();
            List<EasyReproItem> items = ParseElementsAndMethods(elements);
            SaveRows(items, "UCI");

            //Web
            Browser xrmBrowser = new Browser(options);
            PropertyInfo[] browserElements = xrmBrowser.GetType().GetProperties();
            List<EasyReproItem> browserItems = ParseElementsAndMethods(browserElements);
            SaveRows(browserItems, "Web");
        }

        static List<EasyReproItem> ParseElementsAndMethods(PropertyInfo[] elements)
        {
            List<EasyReproItem> items = new List<EasyReproItem>();

            var interestingElements = elements.Where(e => e.PropertyType.Namespace.Contains("UIAutomation"));

            foreach (PropertyInfo element in interestingElements)
            {
                EasyReproItem item = new EasyReproItem();
                item.ElementName = element.Name;

                MethodInfo[] methods = element.PropertyType.GetMethods();
                var interestingMethods = methods.Where(x => x.IsVirtual.Equals(false) && !x.Module.ScopeName.Equals("CommonLanguageRuntimeLibrary"));

                foreach (MethodInfo method in interestingMethods)
                {
                    var constructors = ((System.Reflection.TypeInfo)method.DeclaringType).DeclaredMethods;

                    EasyReproMethod thisMethod = new EasyReproMethod();
                    thisMethod.Name = method.Name;
                    thisMethod.Constructors = constructors.Count();

                    ParameterInfo[] parms = method.GetParameters();

                    foreach(ParameterInfo parm in parms)
                    {
                        EasyReproParameter thisParm = new EasyReproParameter();
                        thisParm.Name = parm.Name;
                        thisParm.DataType = parm.ParameterType.FullName;

                        thisMethod.Parameters.Add(thisParm);
                    }

                    item.Methods.Add(thisMethod);
                }

                items.Add(item);
            }

            return items;
        }
        static SqlConnection InitializeSql()
        {
            SqlConnection conn = new SqlConnection();

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = ".";
            builder.IntegratedSecurity = true;
            builder.InitialCatalog = "EasyRepro";

            conn.ConnectionString = builder.ConnectionString;

            return conn;
        }

        static void SaveRows(List<EasyReproItem> items, string clientName)
        {
            using (SqlConnection conn = InitializeSql())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Connection = conn;

                foreach (EasyReproItem item in items)
                {
                    foreach (EasyReproMethod method in item.Methods)
                    {
                        DataTable parmTable = new DataTable();
                        parmTable.Columns.Add("parameterName", typeof(string));
                        parmTable.Columns.Add("parameterDataType", typeof(string));

                        foreach (EasyReproParameter parm in method.Parameters)
                        {
                            parmTable.Rows.Add(parm.Name, parm.DataType);
                        }

                        cmd.Parameters.AddWithValue("@clientName", clientName);
                        cmd.Parameters.AddWithValue("@elementName", item.ElementName);
                        cmd.Parameters.AddWithValue("@methodName", method.Name);
                        cmd.Parameters.AddWithValue("@constructors", method.Constructors.ToString());

                        SqlParameter parmTblVariable = new SqlParameter();
                        parmTblVariable.ParameterName = "@parameters";
                        parmTblVariable.SqlDbType = System.Data.SqlDbType.Structured;
                        parmTblVariable.Value = parmTable;
                        cmd.Parameters.Add(parmTblVariable);

                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();

                    }
                }
            }
        }
    }

    class EasyReproItem
    {
        public string ElementName { get; set; }
        public List<EasyReproMethod> Methods { get; set; }
        public EasyReproItem()
        {
            this.Methods = new List<EasyReproMethod>();
        }
    }

    class EasyReproMethod
    {
        public string Name { get; set; }
        public int Constructors { get; set; }
        public List<EasyReproParameter> Parameters { get; set; }

        public EasyReproMethod()
        {
            this.Parameters = new List<EasyReproParameter>();
        }
    }

    class EasyReproParameter
    {
        public string DataType { get; set; }
        public string Name { get; set; }
    }
}
