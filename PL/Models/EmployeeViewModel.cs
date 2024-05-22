﻿using DAL.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PL.Models
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public string Address { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Column(TypeName = "money")]
        public double Salary { get; set; }
        public bool IsActive { get; set; }
        public DateTime HireDate { get; set; } = DateTime.Now;

        public int DepartmentId { get; set; }

        public IFormFile? Image { get; set; }
		public string? ImageUrl { get; set; }

	}
}
