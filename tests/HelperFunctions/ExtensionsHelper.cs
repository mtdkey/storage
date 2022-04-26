using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MtdKey.Storage.Tests
{
    public static class ExtensionsHelper
    {
        public static async Task<IRequestResult> TestCreateBunchAndFieldsAsync(this RequestProvider requestProvider, long index)
        {
            var directoryCreated = await requestProvider.BunchSaveAsync(bunch =>
            {
                bunch.Name = $"Bunch is Catalog {index}-first";
            });

            if (!directoryCreated.Success) return directoryCreated;
            var catalogId = directoryCreated.DataSet[0].BunchId;

            for (int i = 0; i < 5; i++)
            {
                var fieldCreated = await requestProvider.FieldSaveAsync(field =>
                {
                    field.BunchId = catalogId;
                    field.Name = $"Catalog Item-{index}-{i}";
                    field.FieldType = FieldType.Text;
                });

                if (!fieldCreated.Success) return fieldCreated;
            }

            var bunchCreated = await requestProvider.BunchSaveAsync(bunch =>
            {
                bunch.Name = $"Bunch for test fields {index}-second";
            });

            if (!bunchCreated.Success) return bunchCreated;
            var bunchId = bunchCreated.DataSet[0].BunchId;

            foreach (var xmlType in FieldType.XmlTypes)
            {
                long linkId = 0;
                if (FieldType.IsXmlTypeLink(xmlType))
                    linkId = directoryCreated.DataSet[0].BunchId;


                var fieldCreated = await requestProvider.FieldSaveAsync(field =>
                {
                    field.LinkId = linkId;
                    field.BunchId = bunchId;
                    field.Name = $"Feild-{index}-{xmlType}";
                    field.FieldType = FieldType.GetFromXmlType(xmlType);
                });

                if (!fieldCreated.Success) return fieldCreated;
            }

            return new RequestResult<IRequestResult>(true);
        }

    }
}
