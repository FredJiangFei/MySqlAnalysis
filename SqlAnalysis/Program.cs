using Dapper;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using SqlAnalysis.Models;
using SqlAnalysis.Resources;
using SqlAnalysis.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SqlAnalysis
{
    class Program
    {

        static void Main(string[] args)
        {
            QuerySqlObjectService service = new QuerySqlObjectService();
            //var objs = service.GetObjects();
            var sp = "sp_AddProduct";
            var des = service.GetDependencies(sp);
            var columns = service.GetTableColumns();


            foreach (var o in des)
            {
                Console.WriteLine($"{o.Type}, {o.Name}");

                FindColumns(o, columns);
                if (o.Type == DataObjectType.Table)
                {
                    var tableColumns = columns.Where(x => x.TableName == o.Name);
                    foreach (var c in tableColumns)
                    {
                        Console.WriteLine($"  Column {c.ColumnName}");
                    }
                }
                else
                {
                    var de = service.GetDependencies(o.Name);
                    foreach (var d in de)
                    {
                        Console.WriteLine($"  {d.Type}, {d.Name}");
                    }
                }
            }

            //var tab = "cat_product";
            //var columns = service.GetTableColumns();
            //foreach (var c in columns)
            //{
            //    Console.WriteLine($"{c.TableName},{c.ColumnName}");
            //}

        }

        private static void FindColumns(ScriptObjectDependency sod, List<TableColumn> columns)
        {
            if (sod.Type == DataObjectType.Table)
            {
                var tableColumns = columns.Where(x => x.TableName == sod.Name);
                foreach (var c in tableColumns)
                {
                    Console.WriteLine($"  Column {c.ColumnName}");
                }
            }
        }
    }
}
