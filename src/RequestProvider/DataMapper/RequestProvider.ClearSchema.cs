using Microsoft.EntityFrameworkCore;
using MtdKey.Storage.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MtdKey.Storage
{
    public partial class RequestProvider : IDisposable
    {
        public async Task<IRequestResult> ClearSchemasAsync()
        {

            var fieldResult = await ClearSchemaFieldsAsync();
            if (!fieldResult.Success) return fieldResult;

            var bunchResult = await ClearSchemaBunchesAsync();
            if (!bunchResult.Success) return bunchResult;

            return new RequestResult<IRequestResult>(true);
        }

        private async Task<IRequestResult> ClearSchemaFieldsAsync()
        {
            var schemas = await context.Set<SchemaName>().ToListAsync();
            var lostFields = new List<Field>();
            foreach (var schema in schemas)
            {
                var version = await context.Set<SchemaVersion>()
                      .Where(x => x.SchemaNameId == schema.Id)
                      .OrderByDescending(x => x.Id)
                      .FirstOrDefaultAsync();

                if (version == null) continue;

                var xmlSchema = new XmlSchema<IXmlSchema>();
                xmlSchema.LoadSchemaFromXml(version.XmlSchema);

                var schemaBunches = xmlSchema.GetBunches();
                var schemaFields = xmlSchema.GetFields();                
                foreach (var actualBunch in schemaBunches)
                {
                    //Actual fields from database
                    var bunchFields = await BunchQueryAsync(filter => filter.BunchNames.Add(actualBunch.Name));
                    if (!bunchFields.Success) return new RequestResult<IRequestResult>(false, bunchFields.Exception);
                    var actualFields = bunchFields.DataSet.FirstOrDefault()?.FieldPatterns;
                    if (actualFields == null) continue;

                    //Correct fields from schema
                    var correctFields = schemaFields.Where(field => field.BunchName == actualBunch.Name).Select(x => x.FieldPattern).ToList();

                    foreach (var actualField in actualFields)
                    {
                        //If the actual field is not in the schema, delete this file
                        if (!correctFields.Where(correctField => correctField.Name == actualField.Name).Any())
                        {
                            var field = new Field { Id = actualField.FieldId };
                            lostFields.Add(field);
                        }
                    }
                }
            }

            if (lostFields.Count > 0)
            {
                var ids = lostFields.Select(x => x.Id);
                var stackList = await context.Set<Stack>().Where(x => ids.Contains(x.FieldId)).ToListAsync();
                context.RemoveRange(stackList);
                context.RemoveRange(lostFields);
                await context.SaveChangesAsync();
            }

            return new RequestResult<IRequestResult>(true);
        }


        private async Task<IRequestResult> ClearSchemaBunchesAsync()
        {
            var schemas = await context.Set<SchemaName>().ToListAsync();
            var schemaBunches = new List<BunchPattern>();
            var lostBunches = new List<Bunch>();

            var bunchesRequest = await BunchQueryAsync(filter => filter.PageSize = int.MaxValue);
            if (!bunchesRequest.Success) return new RequestResult<IRequestResult>(false, bunchesRequest.Exception);

            foreach (var schema in schemas)
            {
                var version = await context.Set<SchemaVersion>()
                      .Where(x => x.SchemaNameId == schema.Id)
                      .OrderByDescending(x => x.Id)
                      .FirstOrDefaultAsync();

                if (version == null) continue;

                var xmlSchema = new XmlSchema<IXmlSchema>();
                xmlSchema.LoadSchemaFromXml(version.XmlSchema);

                var bunches = xmlSchema.GetBunches();
                schemaBunches.AddRange(bunches);
            }

            var actualBunches = bunchesRequest.DataSet.ToList();
            foreach (var actualBunch in actualBunches)
            {
                if (!schemaBunches.Where(schemaBunch => schemaBunch.Name == actualBunch.Name).Any())
                {
                    lostBunches.Add(new Bunch { Id = actualBunch.BunchId });
                }
            }

            if (lostBunches.Count > 0)
            {                
                var fields = await context.Set<Field>()
                        .Where(x => lostBunches.Select(x => x.Id)
                        .Contains(x.BunchId)).ToListAsync();

                var fieldLinks = await context.Set<FieldLink>()
                    .Where(x => lostBunches.Select(x => x.Id)
                    .Contains(x.BunchId)).ToListAsync();

                var stackList = await context.Set<Stack>()
                    .Where(x => fields.Select(x=>x.Id).Contains(x.FieldId)).ToListAsync();

                context.RemoveRange(stackList);                
                context.RemoveRange(fields);
                context.RemoveRange(fieldLinks);
                context.RemoveRange(lostBunches);
                await context.SaveChangesAsync();
            }

            return new RequestResult<IRequestResult>(true);
        }
    }
}
