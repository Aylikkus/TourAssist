using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TourAssist.Model
{
    public enum Operator
    {
        AND,
        OR
    }

    public class Node
    {
        public Node(Operator op, object? left, object? right)
        {
            Operator = op;
            Left = left;
            Right = right;
        }

        public Operator Operator { get; set; }
        public object? Left { get; set; }
        public object? Right { get; set; }


    }

    public class BinaryTree
    {
        public Node Head { get; private set; }

        public BinaryTree(Node head)
        {
            Head = head;
        }
    }

    public class Interpreter
    {
        private string query;

        public string Query
        {
            get
            {
                return query;
            }
            set
            {
                query = value;
            }
        }

        public BinaryTree GetAST()
        {
            string[] tokens = Query.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            Node? prevNode = null;
            List<Node> nodes = new List<Node>();
            for (int i = 1; i < tokens.Length - 1; i++)
            {
                Operator op;

                switch (tokens[i])
                {
                    case "и":
                        op = Operator.AND;
                        break;
                    case "или":
                        op = Operator.OR;
                        break;
                    default:
                        continue;
                }

                Node node;

                if (prevNode == null)
                {
                    node = new Node(op, tokens[i - 1], tokens[i + 1]);
                }
                else
                {
                    node = new Node(op, prevNode, tokens[i + 1]);
                }

                prevNode = node;
                nodes.Add(node);
            }

            return new BinaryTree(nodes.Last());
        }

        public Interpreter(string query) 
        {
            Query = query;
        }

    }
}
