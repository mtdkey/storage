using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MtdKey.Storage
{
    public static class ListNodePatterns
    {
        /// <summary>
        /// Function for getting data as a dictionary. It gets the first text field in the field list of the node.
        /// Use this function for data of Catalog type.
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        public static Dictionary<long, string> GetDictionary(this List<NodePattern> nodes)
        {
            var result = new Dictionary<long, string>();
            nodes.ForEach(node => {
                var key = node.NodeId;
                var value = node.Items.FirstOrDefault(item => item.FieldType == FieldType.Text)?.Data ?? string.Empty;
                result.Add(key, (string)value);
            });

            return result;
        }
    }
}
