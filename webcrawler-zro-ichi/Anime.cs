using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace webcrawler_zro_ichi
{
    [Table("anime")]
    public class Anime 
    {
        public string title { get; set; }
        public string link { get; set; }
        public string img_src { get; set; }
        public int id { get; set; }
        public DateTime release_date { get; set; }

        public Anime(string title, string img_src, string link,int id)
        {
            this.title = title;
            this.link = link;
            this.img_src = img_src;
            this.id = id;
        }
    }
}
