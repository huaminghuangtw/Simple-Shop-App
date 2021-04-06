    
using System;
using System.Diagnostics;

namespace SimpleShop
{
public class ShopParser
{
        private Keyword[] keywords = new Keyword[0];
            
        public void SetKeywords(Keyword[] tags)
        {
            // Yes, this is possible, as JIT Language Arrays are allocated during runtime.
            keywords = tags;
        }

        public Keyword[] GetKeywords()
        {
            return this.keywords;
        }
        
        /// <summary>
        /// Extracts a list of strings from a TAG file. 
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="serializedInput"></param>
        /// <returns>List of information provided in the TAG file</returns>
        public static KeywordPair[] ExtractFromTAG(ShopParser parser, string serializedInput)
        {
            var findings = new KeywordPair[0];

            foreach ( var keyword in parser.GetKeywords() )
            {
                var start = serializedInput.IndexOf(keyword.GetStart());
                
                // Check if keyword is in input string, if not continue with next keyword
                if (start <= -1) continue;
                
                var end = serializedInput.LastIndexOf(keyword.GetEnd());
                
                // Extract the thing between the tags. Tag excluded
                start += keyword.GetStart().Length;
                var substring = serializedInput.Substring(start, end - start);

                // Add substring to result list
                var tmp = new KeywordPair[findings.Length + 1];
                var i = 0;
                for (; i < findings.Length; ++i)
                {
                    tmp[i] = findings[i];
                }
                tmp[i] = new KeywordPair(keyword, substring);
                findings = tmp;
            }

            return findings;
        }

        /// <summary>
        /// Validates if the findings from an extracted tag list.
        /// A set is valid if there is only one tag.
        /// </summary>
        /// <param name="findings"></param>
        /// <returns>true is valid</returns>
        public static bool ValidateFindings(KeywordPair[] findings)
        {
            var keyword_table = new string[findings.Length];
            var head = 0;
            
            foreach (var pair in findings)
            {
                foreach (var test in keyword_table)
                {
                    if (pair.Key.GetString() == test)
                    {
                        return false;
                    }
                }
                keyword_table[head] = findings[head++].Key.GetString();
            }

            return true;
        }
}   // class ShopParser
}   // namespace SimpleShop