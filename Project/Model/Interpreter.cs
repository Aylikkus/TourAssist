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
        public Node? Head { get; private set; }

        public BinaryTree(Node? head)
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

            if (tokens.Length == 1)
            {
                Node node = new Node(Operator.OR, tokens[0], "");
                return new BinaryTree(node);
            }

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

            return new BinaryTree(nodes.LastOrDefault());
        }

        // output: { country_iso's }
        private List<string> searchCountriesRecurs(Node? node, 
            Dictionary<string, List<string>> map,
            Dictionary<string, List<string>> revMap)
        {
            List<string>? leftList = null;
            List<string>? rightList = null;

            List<string> result = new List<string>();

            if (node == null) return result;

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

                        List<string> leftISOs;
                        if (map.ContainsKey(left))
                            leftISOs = map[left];
                        else
                            leftISOs = new List<string>();

                        List<string> rightISOs;
                        if (map.ContainsKey(right))
                            rightISOs = map[right];
                        else
                            rightISOs = new List<string>();

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
                if (leftList == null && rightList == null)
                {
                    if (node.Left is string && node.Right is string)
                    {
                        string left = (string)node.Left;
                        string right = (string)node.Right;

                        List<string> leftISOs;
                        if (map.ContainsKey(left))
                            leftISOs = map[left];
                        else
                            leftISOs = new List<string>();

                        List<string> rightISOs;
                        if (map.ContainsKey(right))
                            rightISOs = map[right];
                        else
                            rightISOs = new List<string>();

                        result.AddRange(leftISOs.Intersect(rightISOs));
                    }
                }

                if (leftList == null && rightList != null)
                {
                    if (node.Left is string)
                    {
                        string left = (string)node.Left;
                        List<string> right = rightList;

                        result.AddRange(right.Intersect(right.Where((iso) =>
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

                        result.AddRange(left.Intersect(left.Where((iso) =>
                        {
                            return revMap[iso].Contains(right);
                        })));
                    }
                }
            }

            if (leftList != null && rightList != null)
            {
                List<string> left = leftList;
                List<string> right = rightList;

                result.AddRange(left);
                result.AddRange(right);
            }

            return result;
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
                    revMap[pec.CountryIso31661] = new List<string>();

                revMap[pec.CountryIso31661].Add(pecMap[pec.PeculiarityIdPeculiarity]);
            }

            List<string> isoList = searchCountriesRecurs(ast.Head, map, revMap);

            return countries.Where((c) => isoList.Contains(c.Iso31661)).ToList();
        }

        // output: { id's }
        private List<int> searchIdsRecurs(Node? node,
            Dictionary<string, List<int>> map,
            Dictionary<int, List<string>> revMap)
        {
            List<int>? leftList = null;
            List<int>? rightList = null;

            List<int> result = new List<int>();

            if (node == null) return result;

            if (node.Left is Node)
            {
                leftList = searchIdsRecurs((Node)node.Left, map, revMap);
            }

            if (node.Right is Node)
            {
                rightList = searchIdsRecurs((Node)node.Right, map, revMap);
            }

            if (node.Operator == Operator.OR)
            {
                if (leftList == null && rightList == null)
                {
                    if (node.Left is string && node.Right is string)
                    {
                        string left = (string)node.Left;
                        string right = (string)node.Right;

                        List<int> leftIds;
                        if (map.ContainsKey(left))
                            leftIds = map[left];
                        else
                            leftIds = new List<int>();


                        List<int> rightIds;
                        if (map.ContainsKey(right))
                            rightIds = map[right];
                        else
                            rightIds = new List<int>();

                        result.AddRange(leftIds.Union(rightIds));
                    }
                }

                if (leftList == null && rightList != null)
                {
                    if (node.Left is string)
                    {
                        string left = (string)node.Left;
                        List<int> right = rightList;

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
                        List<int> left = leftList;
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
                if (leftList == null && rightList == null)
                {
                    if (node.Left is string && node.Right is string)
                    {
                        string left = (string)node.Left;
                        string right = (string)node.Right;

                        List<int> leftIds;
                        if (map.ContainsKey(left))
                            leftIds = map[left];
                        else
                            leftIds = new List<int>();

                        List<int> rightIds;
                        if (map.ContainsKey(right))
                            rightIds = map[right];
                        else
                            rightIds = new List<int>();

                        result.AddRange(leftIds.Intersect(rightIds));
                    }
                }

                if (leftList == null && rightList != null)
                {
                    if (node.Left is string)
                    {
                        string left = (string)node.Left;
                        List<int> right = rightList;

                        result.AddRange(right.Intersect(right.Where((iso) =>
                        {
                            return revMap[iso].Contains(left);
                        })));
                    }
                }

                if (leftList != null && rightList == null)
                {
                    if (node.Right is string)
                    {
                        List<int> left = leftList;
                        string right = (string)node.Right;

                        result.AddRange(left.Intersect(left.Where((iso) =>
                        {
                            return revMap[iso].Contains(right);
                        })));
                    }
                }
            }

            if (leftList != null && rightList != null)
            {
                List<int> left = leftList;
                List<int> right = rightList;

                result.AddRange(left);
                result.AddRange(right);
            }

            return result;
        }

        public List<Region> SearchRegions()
        {
            BinaryTree ast = GetAST();
            List<Peculiarity> peculiarities;
            List<PecularitiesRegion> regionPecs;
            List<Region> regions;

            using (TourismDbContext dbContext = new TourismDbContext())
            {
                peculiarities = dbContext.Peculiarities.ToList();
                regionPecs = dbContext.PecularitiesRegions.ToList();
                regions = dbContext.Regions.ToList();
            }

            // peculiarity_id -> peculiarity_description
            Dictionary<int, string> pecMap = new Dictionary<int, string>();

            foreach (Peculiarity pec in peculiarities)
            {
                pecMap[pec.IdPeculiarity] = pec.Description;
            }

            // peculiarity_description -> { region_id's }
            Dictionary<string, List<int>> map = new Dictionary<string, List<int>>();

            foreach (PecularitiesRegion pec in regionPecs)
            {
                string name = pecMap[pec.PeculiarityIdPeculiarity];

                if (!map.ContainsKey(name))
                    map[name] = new List<int>();

                map[name].Add(pec.RegionIdRegion);
            }

            // region_id -> { peculiarity_description's }
            Dictionary<int, List<string>> revMap = new Dictionary<int, List<string>>();

            foreach (PecularitiesRegion pec in regionPecs)
            {
                if (!revMap.ContainsKey(pec.RegionIdRegion))
                    revMap[pec.RegionIdRegion] = new List<string>();

                revMap[pec.RegionIdRegion].Add(pecMap[pec.PeculiarityIdPeculiarity]);
            }

            List<int> idList = searchIdsRecurs(ast.Head, map, revMap);

            return regions.Where((r) => idList.Contains(r.IdRegion)).ToList();
        }

        public List<City> SearchCities()
        {
            BinaryTree ast = GetAST();
            List<Peculiarity> peculiarities;
            List<PecularitiesCity> cityPecs;
            List<City> cities;

            using (TourismDbContext dbContext = new TourismDbContext())
            {
                peculiarities = dbContext.Peculiarities.ToList();
                cityPecs = dbContext.PecularitiesCities.ToList();
                cities = dbContext.Cities.ToList();
            }

            // peculiarity_id -> peculiarity_description
            Dictionary<int, string> pecMap = new Dictionary<int, string>();

            foreach (Peculiarity pec in peculiarities)
            {
                pecMap[pec.IdPeculiarity] = pec.Description;
            }

            // peculiarity_description -> { city_id's }
            Dictionary<string, List<int>> map = new Dictionary<string, List<int>>();

            foreach (PecularitiesCity pec in cityPecs)
            {
                string name = pecMap[pec.PeculiarityIdPeculiarity];

                if (!map.ContainsKey(name))
                    map[name] = new List<int>();

                map[name].Add(pec.CityIdCity);
            }

            // city_id -> { peculiarity_description's }
            Dictionary<int, List<string>> revMap = new Dictionary<int, List<string>>();

            foreach (PecularitiesCity pec in cityPecs)
            {
                if (!revMap.ContainsKey(pec.CityIdCity))
                    revMap[pec.CityIdCity] = new List<string>();

                revMap[pec.CityIdCity].Add(pecMap[pec.PeculiarityIdPeculiarity]);
            }

            List<int> idList = searchIdsRecurs(ast.Head, map, revMap);

            return cities.Where((c) => idList.Contains(c.IdCity)).ToList();
        }

        public Interpreter(string query) 
        {
            this.query = query;
        }

        public static List<RouteCitiesView> FindRoutes(City? fromCity, 
            Country? toCountry, Region? toRegion, City? toCity)
        {
            if (fromCity == null) return new List<RouteCitiesView>();

            if (toCity != null)
            {
                using (TourismDbContext dbContext = new TourismDbContext())
                {
                    return dbContext.RouteCitiesViews.Where((r) => r.FromCityId == fromCity.IdCity &&
                        r.ToCityId == toCity.IdCity).ToList();
                }
            }
            else if (toRegion != null)
            {
                using (TourismDbContext dbContext = new TourismDbContext())
                {
                    List<RouteCitiesView> routes = dbContext.RouteCitiesViews.Where((r) => 
                        r.FromCityId == fromCity.IdCity).ToList();
                    List<CityCountryView> cities = dbContext.CityCountryViews.Where((c) =>
                        c.RegionName == toRegion.FullName &&
                        c.CountryIso31661 == toRegion.CountryIso31661).ToList();

                    return routes.Where((r) => cities.Find(
                        (c) => c.IdCity == r.ToCityId) != null).ToList();
                }
            }
            else if (toCountry != null)
            {
                using (TourismDbContext dbContext = new TourismDbContext())
                {
                    List<RouteCitiesView> routes = dbContext.RouteCitiesViews.Where((r) =>
                        r.ToCityId == fromCity.IdCity).ToList();
                    List<CityCountryView> cities = dbContext.CityCountryViews.Where((c) =>
                        c.CountryIso31661 == toCountry.Iso31661).ToList();

                    return routes.Where((r) => cities.Find(
                        (c) => c.IdCity == r.ToCityId) != null).ToList();
                }
            }
            else
            {
                return new List<RouteCitiesView>();
            }
        }
    }
}
