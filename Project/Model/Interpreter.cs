using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TourAssist.Model.Scaffold;

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

        // output: { country_iso's }
        private List<string> searchCountriesRecurs(Node node, 
            Dictionary<string, List<string>> map,
            Dictionary<string, List<string>> revMap)
        {
            List<string>? leftList = null;
            List<string>? rightList = null;

            List<string> result = new List<string>();

            if (node.Left is Node)
            {
                leftList = searchCountriesRecurs((Node)node.Left, map, revMap);
            }
            
            if (node.Right is Node)
            {
                rightList = searchCountriesRecurs((Node)node.Right, map, revMap);
            }

            if (node.Operator == Operator.OR)
            {
                if (leftList == null && rightList == null)
                {
                    if (node.Left is string && node.Right is string)
                    {
                        string left = (string)node.Left;
                        string right = (string)node.Right;

                        List<string> leftISOs = map[left];
                        List<string> rightISOs = map[right];

                        result.AddRange(leftISOs.Union(rightISOs));
                    }
                }
                
                if (leftList == null && rightList != null)
                {
                    if (node.Left is string)
                    {
                        string left = (string)node.Left;
                        List<string> right = rightList;

                        result.AddRange(right.Union(right.Where((iso) =>
                        {
                            return revMap[iso].Contains(left);
                        })));
                    }
                }
                
                if (leftList != null && rightList == null)
                {
                    if (node.Right is string)
                    {
                        List<string> left = leftList;
                        string right = (string)node.Right;

                        result.AddRange(left.Union(left.Where((iso) =>
                        {
                            return revMap[iso].Contains(right);
                        })));
                    }
                }
            }
            else
            {

            }

            if (leftList != null && rightList != null)
            {
                List<string> left = leftList;
                List<string> right = rightList;

                result.AddRange(left);
                result.AddRange(right);
            }
        }

        public List<Country> SearchCountries()
        {
            BinaryTree ast = GetAST();
            List<Peculiarity> peculiarities;
            List<PecularitiesCountry> countryPecs;
            List<Country> countries;

            using (TourismDbContext dbContext = new TourismDbContext())
            {
                peculiarities = dbContext.Peculiarities.ToList();
                countryPecs = dbContext.PecularitiesCountries.ToList();
                countries = dbContext.Countries.ToList();
            }

            // peculiarity_id -> peculiarity_description
            Dictionary<int, string> pecMap = new Dictionary<int, string>();

            foreach (Peculiarity pec in peculiarities)
            {
                pecMap[pec.IdPeculiarity] = pec.Description;
            }

            // peculiarity_description -> { country_iso's }
            Dictionary<string, List<string>> map = new Dictionary<string, List<string>>();

            foreach (PecularitiesCountry pec in countryPecs)
            {
                string name = pecMap[pec.PeculiarityIdPeculiarity];

                if (! map.ContainsKey(name))
                    map[name] = new List<string>();

                map[name].Add(pec.CountryIso31661);
            }

            // country_iso -> { peculiarity_description's }
            Dictionary<string, List<string>> revMap = new Dictionary<string, List<string>>();

            foreach (PecularitiesCountry pec in countryPecs)
            {
                if (!map.ContainsKey(pec.CountryIso31661))
                    map[pec.CountryIso31661] = new List<string>();

                map[pec.CountryIso31661].Add(pecMap[pec.PeculiarityIdPeculiarity]);
            }

            return searchCountriesRecurs(ast.Head, map, revMap);
        }

        public Interpreter(string query) 
        {
            Query = query;
        }

    }
}
