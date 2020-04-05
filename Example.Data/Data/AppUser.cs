using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Example.Data.Data
{
    [Table("APP_User")]
    public class AppUser
    {
        [Key] [Column("User_Id")] public int UserId { get; set; }
        [Column("First_Name")] public string FirstName { get; set; }
        [Column("Last_Name")] public string LastName { get; set; }
        [Column("Age")] public int Age { get; set; }
        [Column("Created")] public DateTime Created { get; set; }
        [Column("Role_Id")] public int RoleId { get; set; }
    }
}
