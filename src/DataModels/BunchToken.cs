using System.ComponentModel.DataAnnotations;

namespace MtdKey.Storage.DataModels
{
    /// <summary>
    /// This model is use to control access when saving, editing, or deleting Node and Stack models.
    /// </summary>
    internal class BunchToken
    {
        [Key]
        public long BunchId {get;set;}
        public string TokenToCreate { get; set; }
        public string TokenToEdit { get; set; }
        public string TokenoDelete { get; set; }

        public virtual Bunch Bunch { get; set; }

    }
}
