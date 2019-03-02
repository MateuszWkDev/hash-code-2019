using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HashCode2019.Models
{
    public class Slide
    {
        public Slide(Photo photo1, Photo photo2 =null)
        {
            var tags = new List<string>(photo1.Tags);
            Photos = new List<Photo>() { photo1 };
            
            if(photo2 != null)
            {
                Photos.Add(photo2);
                tags.AddRange(photo2.Tags);
            }
            Tags = new HashSet<string>(tags.Distinct().ToList());
            TagsCount = Tags.Count;
        }
        public List<Photo> Photos { get; set; }
        public HashSet<string> Tags { get; set; }
        public int TagsCount { get; set; }

    }
}
