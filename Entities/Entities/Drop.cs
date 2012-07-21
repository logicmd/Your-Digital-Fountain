using System.Collections.Generic;

namespace Entities
{
    public class Drop
    {
        public IEnumerable<int> SelectedParts { get; set; }
        public byte[] Data { get; set; }
    }
}