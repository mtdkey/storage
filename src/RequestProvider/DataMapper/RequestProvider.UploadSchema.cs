using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MtdKey.Storage.DataModels;

namespace MtdKey.Storage
{
    public partial class RequestProvider : IDisposable
    {

        public async Task<IRequestResult> UploadSchemaAsync(List<IXmlSchema> schemas)
        {

            var bunchTags = new List<BunchPattern>();
            var fieldTags = new List<FieldTag>();

            foreach (var schema in schemas)
            {
                var bunches = schema.GetBunches();
                var fields = schema.GetFields();
                bunchTags.AddRange(bunches);
                fieldTags.AddRange(fields);
            }

            await BeginTransactionAsync();

            var uploadBunches = await UploadBunchesAsync(bunchTags);
            if (await ResultAsync(uploadBunches) is not true)
                return uploadBunches;

            var uploadFields = await UploadFieldsAsync(fieldTags);
            if (await ResultAsync(uploadFields) is not true)
                return uploadFields;
            try
            {
                foreach (var schema in schemas)
                    await VersionProcessingAsync(schema);
            }
            catch (Exception ex)
            {
                await RollbackTransactionAsync();
                return new RequestResult<IRequestResult>(false, ex);
            }

            await CommitTransactionAsync();

            return new RequestResult<IRequestResult>(true);
        }

        private async Task VersionProcessingAsync(IXmlSchema schema)
        {
            var uniqueName = schema.GetName();
            var schemaName = context.Set<SchemaName>()
                .Where(schema => schema.UniqueName.Equals(uniqueName)).FirstOrDefault();

            if (schemaName is null)
            {
                schemaName = new SchemaName
                {
                    UniqueName = uniqueName,
                };

                await context.AddAsync(schemaName);
                await context.SaveChangesAsync();
            }

            var uploadVersion = schema.GetVersion();

            var versionExists = await context.Set<SchemaVersion>()
                .Where(x => x.SchemaNameId == schemaName.Id && x.Version == uploadVersion).AnyAsync();

            if (versionExists) return;

            var schemaVersion = new SchemaVersion
            {
                Version = schema.GetVersion(),
                XmlSchema = schema.GetXmlDocument().InnerXml
            };

            schemaName.SchemaVersions.Add(schemaVersion);

            await context.SaveChangesAsync();
        }

        private async Task<bool> ResultAsync(IRequestResult result)
        {
            if (!result.Success)
            {
                await RollbackTransactionAsync();
                return result.Success;
            }
            return result.Success;

        }

        private async Task<IRequestResult> UploadBunchesAsync(List<BunchPattern> bunchTags)
        {
            try
            {
                await ParsingBunchTagsAsync(bunchTags);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new RequestResult<IRequestResult>(false, ex);
            }

            return new RequestResult<IRequestResult>(true);
        }

        private async Task ParsingBunchTagsAsync(List<BunchPattern> bunchTags)
        {
            var query = context.Set<Bunch>();

            foreach (var bunchTag in bunchTags)
            {
                var bunchExists = query
                    .Where(bunch => bunch.Name.ToLower() == bunchTag.Name.ToLower())
                    .Any();
                if (bunchExists) continue;

                var bunch = new Bunch { Name = bunchTag.Name };
                var bunchEx = new BunchExt { Counter = 0 };
                bunch.BunchExt = bunchEx;
                await context.AddAsync(bunch);
            }
        }

        private async Task<IRequestResult> UploadFieldsAsync(List<FieldTag> fieldTags)
        {
            try
            {
                await ParsingFieldTagsAsync(fieldTags);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new RequestResult<IRequestResult>(false, ex);
            }

            return new RequestResult<IRequestResult>(true);
        }

        private async Task ParsingFieldTagsAsync(List<FieldTag> fieldTags)
        {
            var bunchQuery = context.Set<Bunch>();
            var fieldQuery = context.Set<Field>();

            foreach (var fieldTag in fieldTags)
            {
                long bunchId = bunchQuery.FirstOrDefault(x => x.Name.Equals(fieldTag.BunchName)).Id;

                var fieldExists = fieldQuery
                    .Where(field => field.Name.ToLower() == fieldTag.FieldPattern.Name.ToLower()
                        && field.BunchId == bunchId)
                    .Any();
                if (fieldExists) continue;

                var field = new Field
                {
                    BunchId = bunchId,
                    Name = fieldTag.FieldPattern.Name,
                    FieldType = fieldTag.FieldPattern.FieldType,
                };

                if (fieldTag.FieldPattern.FieldType == FieldType.LinkSingle)
                {
                    var linkId = bunchQuery.FirstOrDefault(x => x.Name.Equals(fieldTag.BunchList)).Id;
                    var fieldLink = new FieldLink { BunchId = linkId };
                    field.FieldLink = fieldLink;
                }

                await context.AddAsync(field);
            }
        }

    }
}
