using Dapper;
using SqlAnalysis.Models;
using SqlAnalysis.Resources;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SqlAnalysis.Services
{
    public class QuerySqlObjectService
    {
        private string _connectionString = "Server=.;Database=SA_Test;Trusted_Connection=True;";
        public List<ScriptObject> GetObjects()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var objects = connection.Query<ScriptObject>(
                    DbScript.GetScriptObjectDefinition);

                return objects.ToList();
            }
        } 
        
        public List<ScriptObjectDependency> GetDependencies(string objectName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var dependencies = connection.Query<ScriptObjectDependency>(
                    DbScript.GetDependencies,
                     new
                     {
                         objectName = string.Format(
                                    "{0}.{1}",
                                    "dbo",
                                    objectName)
                     });

                return dependencies.ToList();
            }
        }

        public List<TableColumn> GetTableColumns()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var tableColumns = connection.Query<TableColumn>(
                    DbScript.GetTableColumns);

                return tableColumns.ToList();
            }
        }
    }
}
