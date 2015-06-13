using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace AuthSharp.Models
{
    public class UserToken
    {
        [Key]
        public Guid Token { get; set; }
        public DateTime UpdateTime { get; set; }
        public ApplicationUser User { get; set; }
        public override string ToString()
        {
            return Token.ToString();
        }
    }
}