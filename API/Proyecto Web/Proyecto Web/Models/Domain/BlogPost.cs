﻿namespace Proyecto_Web.Models.Domain
{
    public class BlogPost
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string  ShortDescription { get; set; }

        public string Contet { get; set; }

        public string FeaturedImageUrl{ get; set; }

        public DateTime PublishedDate { get; set; }

        public string Author { get; set; }

        public bool IsVisible { get; set; }
    }
}
