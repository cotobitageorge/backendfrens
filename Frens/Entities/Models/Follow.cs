using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frens.Entities.Models
{
    public class Follow
    {
        public int Id { get; set; }
        public int FollowedId { get; set; }
        public int FollowedById { get; set; }

        public DateTime DateOfFollow { get; set; }

        public User Follows { get; set; }
        public User Followed{ get; set; }
    }
}
