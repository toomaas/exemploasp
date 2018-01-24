using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.ExceptionServices;

namespace exemploasp.Models
{
    public class UserAccountExposicao
    {
        [Key]
        [Column(Order = 0)]
        public int UserAccountID { get; set; }
        [Key]
        [Column(Order = 1)]
        public int ExposicaoID { get; set; }
        [ForeignKey("UserAccountID")]
        public UserAccount UserAccount { get; set; }
        [ForeignKey("ExposicaoID")]
        public Exposicao Exposicao { get; set; }

        public int Assigned { get; set; }
    }
}