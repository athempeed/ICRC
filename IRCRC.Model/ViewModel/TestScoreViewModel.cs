﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCRC.Model.ViewModel
{
    public class TestScoreViewModel
    {
      
       
        public int ID { get; set; }
        public string Exam { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Middle Name")]
        public string MiddleName { get; set; }
        [DisplayName("Email Address")]
        public string EmailAddress { get; set; }
        [DisplayName("Exam Date")]
        public string ExamDate { get; set; }
        public string Status { get; set; }
        [DisplayName("Address 1")]
        public string Address1 { get; set; }
        public string City { get; set; }
        public string State{ get; set; }
        [DisplayName("Address 2")]
        public string Address2 { get; set; }
        
        public string TestingCompanyName { get; set; }
        [DisplayName("Testing Company")]
        public Nullable<int> TestingCompany { get; set; }
        [DisplayName("Board Name")]
        public string BoardName { get; set; }
        
        public string ZipPlus { get; set; }
        
        public string ZipCode { get; set; }
        public string Score { get; set; }
    }
}
