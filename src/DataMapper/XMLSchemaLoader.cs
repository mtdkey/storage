using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MtdKey.Storage.DataMapper
{
    public class XMLSchemaLoader<T> where T : class
    {
        private readonly RequestProvider requestProvider;

        public XMLSchemaLoader(RequestProvider requestProvider)
        {
            this.requestProvider = requestProvider;
        }

        public async Task DataLoadToDatabaseAsync()
        {
            var xmlSchema = new XmlSchema<T>();
            xmlSchema.LoadSchemaFromServer();
            var bunches = xmlSchema.GetBunches();
            var fields = xmlSchema.GetFields();

            foreach (var bunch in bunches)
            {
                var requestResult = await requestProvider.BunchSaveAsync(bunch.Value);
                var bunchId = requestResult.DataSet[0].BunchId;
                bunch.Value.BunchId = bunchId;
            }

            foreach (var field in fields)
            {
                long bunchId = bunches.Where(x=>x.Key.Equals(field.XmlBunchId)).FirstOrDefault().Value.BunchId;
                field.FieldSchema.BunchId = bunchId;

                if (field.FieldSchema.FieldType == FieldType.Link)
                {                    
                    var linkId = bunches.Where(x => x.Key.Equals(field.XmlLinkId)).FirstOrDefault().Value.BunchId;
                    field.FieldSchema.LinkId = linkId;
                }

                var requestResult = await requestProvider.FieldSaveAsync(field.FieldSchema);                
            }            

        }

    }
}
