using System.Collections.Generic;

namespace Gizmo.NodeFramework
{
    public class NamesHelper
    {
        public static string GenerateName(List<string> names, string pattern)
        {
            string result;
            uint index = 0;
            do
            {
                index++;
                result = pattern + "_" + index.ToString();
            }
            while (names.Contains(result));

            return result;
        }
    }
}
