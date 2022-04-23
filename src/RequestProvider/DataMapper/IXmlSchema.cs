using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MtdKey.Storage
{
    public interface IXmlSchema
    {
        string GetName();
        long GetVersion();
        XmlDocument GetXmlDocument();
        List<FieldTag> GetFields();
        List<BunchPattern> GetBunches();
    }
}
