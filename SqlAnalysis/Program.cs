using Microsoft.SqlServer.TransactSql.ScriptDom;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace SqlAnalysis
{
    class Program
    {
        static void Main(string[] args)
        {
            //var script = "SELECT UserName, Password FROM Account";
            var path = @"E:\MySolutions\MySqlAnalysis\SqlAnalysis\Files\test.sql";
            var script = File.ReadAllText(path);

            var fragment = SqlParser.ParseScript(script);
            //var s = fragment.OfType<DeleteStatement>().FirstOrDefault();
            foreach (TSqlBatch batch in fragment.Batches)
            {
                if (batch.Statements.Count == 0) continue;

                foreach (TSqlStatement statement in batch.Statements)
                {
                    foreach (var token in statement.ScriptTokenStream)
                    {
                        if(token.TokenType == TSqlTokenType.Identifier)
                        {
                            Console.WriteLine(token.Text);
                        }
                    }
                }
            }

        }

       
    }
}
