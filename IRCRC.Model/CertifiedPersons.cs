﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICRC.Model
{
    public class CertifiedPersons
    {
        public int ID { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Middle Name")]
        public string MiddleName { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        [DisplayName("province")]
        public string province { get; set; }
        public string Country { get; set; }
        public string Zip { get; set; }
        public string SSN { get; set; }
        public string Email { get; set; }
        public string Notes { get; set; }  
        public string Phone { get; set; }             
        [DisplayName("Testing Company")]
        public int TestingCompany { get; set; }
        [DisplayName("Ethical Voilation")]
        public bool EthicalVoilation { get; set; }
        [DisplayName("Current Board")]
        public int CurrentBoardID { get; set; }
        [DisplayName("Other Board")]
        public int OtherBoardID { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<DateTime> CreatedAt { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<DateTime> ModifiedAt { get; set; }
        [NotMapped]
        public string FullName { get; set; }
    }
}
