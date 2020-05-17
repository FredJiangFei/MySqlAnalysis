using System;
using System.Collections.Generic;
using System.Text;

namespace SqlAnalysis.Models
{
    public class ScriptObject
    {
        public string SchemaName { get; set; }
        public string ObjectName { get; set; }
        public DataObjectType ObjectType { get; set; }
        public string Script { get; set; }
    }

    public sealed class ScriptObjectDependency
    {
        public string SchemaName { get; set; }
        public string Name { get; set; }
        public DataObjectType Type { get; set; }
        public DependencyType DependencyType { get; set; }
    }

    public sealed class TableColumn
    {
        public string SchemaName { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
    }

    public enum DependencyType
    {
        Up,
        Down
    }

    public enum DataObjectType
    {
        StoredProcedure,
        Table,
        View,
        Trigger,
        Function
    }
}
