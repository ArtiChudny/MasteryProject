﻿using System;
using System.Collections.Generic;

namespace FileStorage.DAL.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public byte[] HashPassword { get; set; }
        public byte[] Salt { get; set; }
        public DateTime CreationDate { get; set; }
        public List<StorageDirectory> Directories { get; set; }

        public User()
        {
            Directories = new List<StorageDirectory>();
        }
    }
}
