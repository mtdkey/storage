using System.ComponentModel.DataAnnotations;


namespace MtdKey.Storage.DataModels
{
    internal class NodeToken
    {
        [Key]
        public long NodeId { get; set; }

        /// <summary>
        /// Row-level security access token
        /// </summary>
        public string ForRLS { get; set; }

        public virtual Node Node { get; set; }
    }
}
