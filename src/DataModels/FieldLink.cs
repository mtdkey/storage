using System.ComponentModel.DataAnnotations;


namespace MtdKey.Storage.DataModels
{
    internal class FieldLink
    {
        [Key]
        public long FieldId { get; set; }
        /// <summary>
        /// Parent Bunch for select from list
        /// </summary>
        public long BunchId { get; set; }

        public virtual Field Field { get; set; }
        public virtual Bunch Bunch { get; set; }

    }
}
