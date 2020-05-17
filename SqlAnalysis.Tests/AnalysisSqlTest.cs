using Microsoft.SqlServer.TransactSql.ScriptDom;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SqlAnalysis.Tests
{
    [TestFixture]
    public class AnalysisSqlTest
    {
        private SqlParser parser;

        [SetUp]
        public void SetUp()
        {
            parser = new SqlParser();
        }

        [Test]
        public void Test_Select()
        {
            var sql = @"SELECT Customer.firstname, Customer.lastname, Address.Country, City 
                        FROM Customer 
                        INNER JOIN Address ON Address.CustNo = Customer.CustNo";
            var visitor =  parser.ParseScript(sql);
            var selectNode = visitor.SelectNodes.First();

            var tableVisitor = new TableVisitor();
            selectNode.Accept(tableVisitor);
            var tables = tableVisitor.SelectNodes;

            Console.WriteLine(
                "{0}: {1}",
                "Tables",
                String.Join(",", tables.Select(x => x.SchemaObject.BaseIdentifier.Value))
            );

            var columnsVisitor = new ColumnVisitor();
            selectNode.Accept(columnsVisitor);
            var columns = columnsVisitor.SelectNodes;

            Console.WriteLine(
                 "{0}: {1}",
                "Columns",
                String.Join(",", columns.Select(x => x))
               );
        }

        public class TableVisitor : TSqlFragmentVisitor
        {
            public List<NamedTableReference> SelectNodes { get; private set; }

            public TableVisitor() { this.SelectNodes = new List<NamedTableReference>(); }

            public override void Visit(NamedTableReference node)
            {
                base.Visit(node);
                this.SelectNodes.Add(node);
            }
        } 
        
        public class ColumnVisitor : TSqlFragmentVisitor
        {
            public List<string> SelectNodes { get; private set; }

            public ColumnVisitor() { this.SelectNodes = new List<string>(); }

            public override void Visit(SelectScalarExpression node)
            {
                base.Visit(node);

                var identifier = new IdentifierVisitor();
                node.Accept(identifier);

                this.SelectNodes.Add(
                    String.Join(".", identifier.SelectNodes.Select(x => x.Value)
                 ));
            }
        }

        public class IdentifierVisitor : TSqlFragmentVisitor
        {
            public List<Identifier> SelectNodes { get; private set; }

            public IdentifierVisitor() { this.SelectNodes = new List<Identifier>(); }

            public override void Visit(Identifier node)
            {
                base.Visit(node);
                this.SelectNodes.Add(node);
            }
        }
    }
}
