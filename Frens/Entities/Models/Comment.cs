﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frens.Entities.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public User User { get; set; }
        public Post Post { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
    }
}
