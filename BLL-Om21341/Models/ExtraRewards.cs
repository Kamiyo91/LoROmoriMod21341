using System.Collections.Generic;

namespace BLL_Om21341.Models
{
    public class ExtraRewards
    {
        public string MessageId { get; set; }
        public List<DropIdQuantity> DroppedBooks { get; set; }
    }

    public class DropIdQuantity
    {
        public int BookId { get; set; }
        public int Quantity { get; set; }
    }
}