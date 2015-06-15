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

        [Display(Name = "数量")]
        public DataSize Amount { get; set; }

        [Display(Name = "创建时间")]
        public DateTime CreationTime { get; set; }
    }
}