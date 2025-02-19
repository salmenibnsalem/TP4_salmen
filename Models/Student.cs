﻿using System;
using System.ComponentModel.DataAnnotations;

namespace TP4_salmen.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public required string StudentName { get; set; }

        [Range(1, 100)]
        public int Age { get; set; }

        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        public int SchoolID { get; set; }
        public School? School { get; set; }
    }
}
