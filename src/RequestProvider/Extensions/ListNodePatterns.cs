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
        public static Dictionary<long, string> GetDictionary(this List<NodePattern> nodes, params long[] fieldId)
        {
            var result = new Dictionary<long, string>();
            foreach (var node in nodes)
            {
                var key = node.NodeId;
                var query = node.Items.Where(item => item.FieldType == FieldType.Text);

                if (fieldId.Count() > 0)
                    query = query.Where(item => fieldId.Contains(item.FieldId));

                var values = query.Select(x => (string)x.Data).ToList() ?? new();
                var value = string.Join(" ", values.ToArray());
                result.Add(key, value);
            }

            return result;
        }

    }


}
