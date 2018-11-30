using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wedding_Planner.Models{
    public class MainUser
    {
        [Key]
        public int userID {get;set;}

        [Required]
        [Display(Name="First Name")]
        public string f_name{get;set;}

        [Required]
        [Display(Name="Last Name")]
        public string l_name{get;set;}

        [Required]
        [Display(Name="Email")]
        [EmailAddress]
        public string email {get;set;}

        [Required]
        [Display(Name="Password")]
        [DataType(DataType.Password)]
        public string password{get;set;}

        [Required]
        [DataType(DataType.Password)]
        [Display(Name="Confirm Password")]
        [NotMapped]
        [Compare("password")]
        public string confirm{get;set;}

        public DateTime created_at{get;set;}= DateTime.Now;
        public DateTime updated_at{get;set;}=DateTime.Now;

        public List<Wedding> wedding{get;set;}
        public List<Guest> guest{get;set;}

    }

    public class Login
    {
        [Required]
        [Display(Name="Email")]
        public string log_email{get;set;}
        
        [Required]
        [Display(Name="Password")]
        [DataType(DataType.Password)]
        public string log_password{get;set;}
    }

    public class Wedding
    {
        [Key]
        public int weddingID{get;set;}

        public int userID {get;set;}

        [Required]
        [Display(Name="Wedder One")]
        public string wedder1{get;set;}

        [Required]
        [Display(Name="Wedder Two")]
        public string wedder2{get;set;}

        [Required(ErrorMessage="Date must not be empty!!!")]
        [DataType(DataType.Date)]
        [Display(Name="Date")]
        public DateTime date{get;set;}

        [Required]
        [Display(Name="Address")]
        public string address{get;set;}

        public MainUser wedding_creator {get;set;}
        public List<Guest> guest{get;set;}
    }

    public class Guest
    {
        [Key]
        public int guestID{get;set;}
        public int userID {get;set;}
        public int weddingID{get;set;}

        public MainUser user{get;set;}      
        public Wedding wedding{get;set;}

        public int is_available{get;set;}

    }
}