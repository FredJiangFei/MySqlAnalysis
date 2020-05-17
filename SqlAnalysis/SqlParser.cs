using Microsoft.SqlServer.TransactSql.ScriptDom;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SqlAnalysis
{
    public class SqlParser
    {
        public SQLVisitor ParseScript(string script)
        {
            IList<ParseError> parseErrors;
            TSqlParser tsqlParser = new TSql140Parser(true);
            TSqlFragment result;
            using (var stringReader = new StringReader(script))
            {
                result = tsqlParser.Parse(stringReader, out parseErrors);

                var myVisitor = new SQLVisitor();
                result.Accept(myVisitor);
                return myVisitor;
            }
        }

        public class SQLVisitor : TSqlFragmentVisitor
        {
            public List<SelectStatement> SelectNodes { get; private set; }

            public SQLVisitor() { this.SelectNodes = new List<SelectStatement>(); }

            public override void Visit(SelectStatement node)
            {
                base.Visit(node);
                this.SelectNodes.Add(node);
            }
        }
    }
}
