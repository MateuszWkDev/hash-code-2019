using System;
using System.Collections.Generic;
using System.Text;

namespace HashCode2019.Models
{
    public class Slide
    {
        public Slide(Photo photo1, Photo photo2 =null)
        {
            Tags = new HashSet<string>();
            photo1.Tags.ForEach(tag=> Tags.Add(tag));
            Photos = new List<Photo>() { photo1 };
            
            if(photo2 != null)
            {
                Photos.Add(photo2);
                photo2.Tags.ForEach(tag => Tags.Add(tag));
            }
            TagsCount = Tags.Count;
        }
        public List<Photo> Photos { get; set; }
        public HashSet<string> Tags { get; set; }
        public int TagsCount { get; set; }

    }
}
