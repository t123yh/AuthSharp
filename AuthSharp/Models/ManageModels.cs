using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace AuthSharp.Models
{
    public class RechargeRequest
    {
        [Key]
        public Guid RequestID { get; set; }
        public virtual ApplicationUser User { get; set; }
        public DataSize Amount { get; set; }
        public DateTime CreationTime { get; set; }
    }
}