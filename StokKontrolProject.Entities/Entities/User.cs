﻿using StokKontrolProject.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokKontrolProject.Entities.Entities
{
    public class User :BaseEntity
    {
        public User()
        {
            Siparisler = new List<Order>();
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string? PhotoURL { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string Password { get; set; }
        public UserRole  Role { get; set; }

        //Navigation Property
        public virtual List<Order> Siparisler { get; set; }
    }
}
