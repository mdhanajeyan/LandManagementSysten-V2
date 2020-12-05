﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LandBankManagement.Data
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string loginName { get; set; }
        public string UserPassword { get; set; }
        public string Code { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsAgent { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime Updated { get; set; }
        public string UpdatedBy { get; set; }
        [NotMapped]
        public string SearchTerms { get; set; }
        public string BuildSearchTerms() => $"{UserName} {loginName}".ToLower();
    }
}