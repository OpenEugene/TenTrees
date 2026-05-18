using System.Collections.Generic;

namespace OpenEug.TenTrees.Models
{
    public class OrchardWithTreesDto
    {
        public Orchard Orchard { get; set; }
        public List<TreeDetailDto> Trees { get; set; } = new();
    }

    public class TreeDetailDto
    {
        public int TreeId { get; set; }
        public int OrchardId { get; set; }
        public int TreeTypeId { get; set; }
        public string TreeTypeName { get; set; }
        public string TreeTypeCategory { get; set; }
        public int Quantity { get; set; }
        public string? Notes { get; set; }
    }

    public class AddTreeRequest
    {
        public int GrowerId { get; set; }
        public int TreeTypeId { get; set; }
        public int Quantity { get; set; } = 1;
        public string? Notes { get; set; }
    }
}
