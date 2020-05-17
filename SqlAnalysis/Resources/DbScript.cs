using System;
using System.Collections.Generic;
using System.Text;

namespace SqlAnalysis.Resources
{
    public class DbScript
    {
        public static string GetScriptObjectDefinition = @"SELECT 
			SCHEMA_NAME(O.schema_id) AS 'SchemaName', 
			O.name AS 'ObjectName',
			CASE WHEN O.type = 'P' THEN 'StoredProcedure'
					WHEN O.type = 'V' THEN 'View'
					WHEN O.type = 'TR' THEN 'Trigger'
					WHEN O.type = 'TF' THEN 'Function'
					WHEN O.type = 'FN' THEN 'Function'
			ELSE 'Table' END AS 'ObjectType',
			ISNULL(M.definition,'') AS 'Script', 
			ISNULL(OBJECTproperty(O.object_id, N'IsEncrypted'), 0) AS 'IsEncrypted'
			FROM sys.sql_modules M WITH(NOLOCK)
			INNER JOIN sys.objects O WITH(NOLOCK) ON O.object_id = M.object_id
			WHERE O.type in ('U','P','V','TF','FN','TR') AND O.is_ms_shipped = 0";

		public static string GetDependencies = @"
				SELECT DISTINCT
					SCHEMA_NAME(O.schema_id) AS 'SchemaName',
					O.name AS 'Name',
					CASE O.type WHEN 'P' THEN 'StoredProcedure'
								WHEN 'V' THEN 'View'
								WHEN 'TR' THEN 'Trigger'
								WHEN 'TF' THEN 'Function'
								WHEN 'FN' THEN 'Function'
					ELSE 'Table' END AS 'Type',  
					'Down' AS 'DependencyType'
				FROM SYS.DM_SQL_REFERENCED_ENTITIES(@objectName, 'OBJECT') AS R
				LEFT JOIN SYS.OBJECTS AS O ON ISNULL(R.referenced_id, OBJECT_ID(R.referenced_entity_name)) = O.object_id
				WHERE SCHEMA_NAME(O.schema_id) IS NOT NULL
		";


		public static string GetTableColumns = @"SELECT 
				OBJECT_NAME(TB.[OBJECT_ID]) AS 'TableName',
				CONCAT('[', C.NAME, ']') AS 'ColumnName'
			FROM SYS.COLUMNS C WITH(NOLOCK)
			INNER JOIN SYS.TABLES TB WITH(NOLOCK) ON tb.[object_id] = C.[object_id]
			INNER JOIN SYS.TYPES T WITH(NOLOCK) ON C.system_type_id = T.user_type_id
			WHERE TB.[is_ms_shipped] = 0 
			ORDER BY TB.[Name], C.column_id";

	}
}
