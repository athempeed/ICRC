﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICRC.Model
{
    [Table("Studentviolations")]
    public class Studentviolations
    {
        public int ID { get; set; }
        [DisplayName("Originating Board")]
        public Nullable<int> Board { get; set; }
        [DisplayName("Type of Ethical Voilation")]
        public Nullable<int> EthicalViolationId { get; set; }
        [DisplayName("Date of Voilation")]
        public DateTime Date { get; set; }
        [DisplayName("Scanned & Saved")]
        public bool IsScanned { get; set; }
        [DisplayName("Letter Sent to Professional")]
        public bool IsLetterSent { get; set; }
        [DisplayName("Not to be shared with General Public")]
        public bool ISsharable { get; set;}
        [DisplayName("Comments")]
        public string Comments { get; set; }                
        [DisplayName("Person")]
        public Nullable<int> personID { get; set; }
        public string Notes { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<DateTime> CreatedAt { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<DateTime> ModifiedAt { get; set; }

        [NotMapped]
        public string BoardName { get; set; }
        [NotMapped]
        public string PersonName { get; set; }
        [NotMapped]
        public string EthicalViolation { get; set; }
    }
}
