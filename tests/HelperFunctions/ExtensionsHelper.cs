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

            var directoryCreated = await requestProvider.BunchSaveAsync(bunch => {
                bunch.Name = $"Bunch is Catalog {index}-first";
            });

            if (!directoryCreated.Success) return directoryCreated;
            var catalogId = directoryCreated.DataSet[0].BunchId;

            for (int i = 0; i < 5; i++)
            {
                var fieldCreated = await requestProvider.FieldSaveAsync(field =>
                {
                    field.BunchId = catalogId;
                    field.Name = $"Catalog Item {i}";
                    field.FieldType = FieldType.Text;
                });

                if (!fieldCreated.Success) return fieldCreated;
            }


            var bunchCreated = await requestProvider.BunchSaveAsync(bunch => {
                bunch.Name = $"Bunch for test fields {index}-second";
            });

            if (!bunchCreated.Success) return bunchCreated;
            var bunchId = bunchCreated.DataSet[0].BunchId;

            foreach (var fieldType in FieldType.AllTypes)
            {
                long linkId = 0;
                var linkType = LinkType.Single;
                if (fieldType == FieldType.Link)
                {
                    linkId = directoryCreated.DataSet[0].BunchId;
                    linkType = LinkType.Multiple;
                }
                    
                var fieldCreated = await requestProvider.FieldSaveAsync(field =>
                {
                    field.LinkId = linkId;
                    field.BunchId = bunchId;
                    field.Name = $"Feild{FieldType.GetName(fieldType)}";
                    field.FieldType = fieldType;
                    field.LinkType = LinkType.Multiple;
                });

                if (!fieldCreated.Success) return fieldCreated;

                if ((int)fieldCreated.DataSet[0].FieldType == (int)FieldType.Link && (int)fieldCreated.DataSet[0].LinkType == (int)LinkType.Single)
                    return new RequestResult<IRequestResult>(false, new Exception("Link type is not multiple!"));
            }

            return new RequestResult<IRequestResult>(true);
        }

    }
}
