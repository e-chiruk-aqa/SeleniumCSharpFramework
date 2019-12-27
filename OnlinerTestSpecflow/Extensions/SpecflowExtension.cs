using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace OnlinerTestSpecflow.Extensions
{
    public static class SpecflowExtension
    {
        public static Dictionary<string, List<string>> ToDictionaryList(this Table dt)
        {
            var dict = new Dictionary<string, List<string>>();

            if (dt != null)
            {
                var headers = dt.Header;

                foreach (var row in dt.Rows)
                {
                    foreach (var header in headers)
                    {
                        if (dict.ContainsKey(header))
                        {
                            dict[header].Add(row[header]);
                        }
                        else
                        {
                            dict.Add(header, new List<string> { row[header] });
                        }
                    }
                }
            }
            return dict;
        }
    }
}
