using ASC.Model.BaseTypes;

namespace ASC.Model.Models
{
    public class MasterDataKey : BaseEntity
    {
        public MasterDataKey() { }

        public MasterDataKey(string key, string updatedBy)
        {
            this.RowKey = Guid.NewGuid().ToString();
            this.PartitionKey = key;
            //this.UpdatedBy = updatedBy; // Cung cấp giá trị cho UpdatedBy
        }

        public bool IsActive { get; set; }

        public string Name { get; set; }
    }

}