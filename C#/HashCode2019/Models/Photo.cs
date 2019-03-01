using System;
using System.Collections.Generic;
using System.Text;

namespace HashCode2019.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public string Orientation { get; set; }
        public int NumberOfTags { get; set; }
        public List<string> Tags { get; set; }
    }
}
