namespace Alpha.Reports.Models
{
    using System.Runtime.Serialization;

    [DataContract]
    public class Product
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "active")]
        public bool Active { get; set; }

        [DataMember(Name = "permission")]
        public string Permission { get; set; }

        [DataMember(Name = "storeProcedureParameter")]
        public string StoreProcedureParameter { get; set; }
    }
}
