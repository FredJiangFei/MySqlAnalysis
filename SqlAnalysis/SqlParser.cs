using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Collections.Generic;
using System.IO;

namespace SqlAnalysis
{
   public class SqlParser
    {
        public static TSqlScript ParseScript(string script)
        {
            IList<ParseError> parseErrors;
            TSqlParser tsqlParser = new TSql140Parser(true);
            TSqlFragment fragment;
            using (var stringReader = new StringReader(script))
            {
                fragment = tsqlParser.Parse(stringReader, out parseErrors);
            }
            return (TSqlScript)fragment;
        }
    }
}
