using System.Collections.Generic;
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
