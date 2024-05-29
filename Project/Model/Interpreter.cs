using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.RightsManagement;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
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
            MatchCollection composites = Regex.Matches(Query, "\\\"(.*?)\\\"");
            string[] tokens = Query.Split(' ');

            if (composites.Count == 1 && 
                tokens.Where((t) => t == "и" || t == "или").FirstOrDefault() == null)
            {
                Node single = new Node(Operator.OR, composites[0].Value.Trim('"'), "");
                return new BinaryTree(single);
            }

            if (tokens.Length == 1)
            {
                Node single = new Node(Operator.OR, tokens[0], "");
                return new BinaryTree(single);
            }

            Node? prevNode = null;
            List<Node> nodes = new List<Node>();
            int compositeIndex = 0;
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
                    string left;
                    string right;

                    if (tokens[i - 1].EndsWith('"'))
                        left = composites[compositeIndex++].Value.Trim('"');
                    else
                        left = tokens[i - 1];

                    if (tokens[i + 1].StartsWith('"'))
                        right = composites[compositeIndex++].Value.Trim('"');
                    else
                        right = tokens[i + 1];

                    node = new Node(op, left, right);
                }
                else
                {
                    if (tokens[i + 1].StartsWith('"'))
                        node = new Node(op, prevNode, composites[compositeIndex++].Value);
                    else
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
            List<CountryPopularityView> popularities;

            using (TourismDbContext dbContext = new TourismDbContext())
            {
                peculiarities = dbContext.Peculiarities.ToList();
                countryPecs = dbContext.PecularitiesCountries.ToList();
                countries = dbContext.Countries.ToList();
                popularities = dbContext.CountryPopularityViews.ToList();
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

            List<Country> result = new List<Country>();

            foreach (var row in popularities)
            {
                if (isoList.Contains(row.Iso31661))
                {
                    result.Add(countries.Where((c) => c.Iso31661 == row.Iso31661).First());
                    isoList.Remove(row.Iso31661);
                }
            }

            foreach (var iso in isoList)
            {
                result.Add(countries.Where((c) => c.Iso31661 == iso).First());
            }

            return result;
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
            List<RegionPopularityView> popularities;

            using (TourismDbContext dbContext = new TourismDbContext())
            {
                peculiarities = dbContext.Peculiarities.ToList();
                regionPecs = dbContext.PecularitiesRegions.ToList();
                regions = dbContext.Regions.ToList();
                popularities = dbContext.RegionPopularityViews.ToList();
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

            List<Region> result = new List<Region>();

            foreach (var row in popularities)
            {
                if (idList.Contains(row.IdRegion))
                {
                    result.Add(regions.Where((r) => r.IdRegion == row.IdRegion).First());
                    idList.Remove(row.IdRegion);
                }
            }

            foreach (var id in idList)
            {
                result.Add(regions.Where((r) => r.IdRegion == id).First());
            }

            return result;
        }

        public List<City> SearchCities()
        {
            BinaryTree ast = GetAST();
            List<Peculiarity> peculiarities;
            List<PecularitiesCity> cityPecs;
            List<City> cities;

            List<DestinationPopularityView> popularities;

            using (TourismDbContext dbContext = new TourismDbContext())
            {
                peculiarities = dbContext.Peculiarities.ToList();
                cityPecs = dbContext.PecularitiesCities.ToList();
                cities = dbContext.Cities.ToList();
                popularities = dbContext.DestinationPopularityViews.ToList();
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

            List<City> result = new List<City>();

            foreach (var row in popularities)
            {
                if (idList.Contains(row.ToIdCity))
                {
                    result.Add(cities.Where((c) => c.IdCity == row.ToIdCity).First());
                    idList.Remove(row.ToIdCity);
                }
            }

            foreach(var id in idList)
            {
                result.Add(cities.Where((c) => c.IdCity == id).First());
            }

            return result;
        }

        public Interpreter(string query) 
        {
            this.query = query;
        }

        public static List<RouteCitiesView> FindRoutes(City? fromCity, 
            Country? toCountry, Region? toRegion, City? toCity)
        {
            if (fromCity == null) return new List<RouteCitiesView>();

            User? user = AuthManager.CurrentUser;

            if (user == null) return new List<RouteCitiesView>();

            List<UserPreferenceView> preferences;

            using (TourismDbContext dbContext = new TourismDbContext())
            {
                preferences = dbContext.UserPreferenceViews.Where((p) => p.IdUser == user.IdUser).ToList();
            }

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

                    var result = routes.Where((r) => cities.Find(
                        (c) => c.IdCity == r.ToCityId) != null).ToList();

                    List<RouteCitiesView> negatives = new List<RouteCitiesView>();

                    for (int i = 0; i < result.Count; i++)
                    {
                        double totalPref = 0;
                        foreach (var pec in result[i].AllPeculiarities)
                        {
                            var prefRow = preferences.Where((p) => p.IdPeculiarity == pec.IdPeculiarity).FirstOrDefault();
                            double? pref = prefRow == null ? 0 : prefRow.TotalPreference;

                            if (pref != null)
                                totalPref += pref.Value;
                        }

                        var temp = result[i];

                        if (totalPref < 0)
                        {
                            result.RemoveAt(i);
                            negatives.Insert(0, temp);
                        }

                        if (totalPref > 0)
                        {
                            result.RemoveAt(i);
                            result.Insert(0, temp);
                        }
                    }

                    result.AddRange(negatives);
                    return result;
                }
            }
            else if (toCountry != null)
            {
                using (TourismDbContext dbContext = new TourismDbContext())
                {
                    List<RouteCitiesView> routes = dbContext.RouteCitiesViews.Where((r) =>
                        r.FromCityId == fromCity.IdCity).ToList();
                    List<CityCountryView> cities = dbContext.CityCountryViews.Where((c) =>
                        c.CountryIso31661 == toCountry.Iso31661).ToList();

                    var result = routes.Where((r) => cities.Find(
                        (c) => c.IdCity == r.ToCityId) != null).ToList();

                    List<RouteCitiesView> negatives = new List<RouteCitiesView>();

                    for (int i = 0; i < result.Count; i++)
                    {
                        double totalPref = 0;
                        foreach (var pec in result[i].AllPeculiarities)
                        {
                            var prefRow = preferences.Where((p) => p.IdPeculiarity == pec.IdPeculiarity).FirstOrDefault();
                            double? pref = prefRow == null ? 0 : prefRow.TotalPreference;

                            if (pref != null)
                                totalPref += pref.Value;
                        }

                        var temp = result[i];

                        if (totalPref < 0)
                        {
                            result.RemoveAt(i);
                            negatives.Insert(0, temp);
                        }

                        if (totalPref > 0)
                        {
                            result.RemoveAt(i);
                            result.Insert(0, temp);
                        }
                    }

                    result.AddRange(negatives);
                    return result;
                }
            }
            else
            {
                return new List<RouteCitiesView>();
            }
        }
    }
}
