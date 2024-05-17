using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourAssist.Model
{
    public class Parser
    {
        List<string> includedPeculiarities = new List<string>();
        List<string> excludedPeculiarities = new List<string>();

        void parseString(string query)
        {
            string[] tokens = query.Split("", StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < tokens.Length; i++)
            {
                string token = tokens[i];

                bool hasPrev = i - 1 >= 0;
                bool hasNext = i + 1 < tokens.Length;

                
            }
        }
    }
}
