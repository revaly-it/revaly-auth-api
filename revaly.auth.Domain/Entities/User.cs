using revaly.auth.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using revaly.auth.Domain.Entities.Enums;

namespace revaly.auth.Domain.Entities
{
    public class User : BaseEntity
    {
        [Column("FullName")]
        [StringLength(300)]
        public string? FullName { get; set; }

        [Column("Email")]
        [EmailAddress]
        [StringLength(300)]
        public string? Email { get; set; }

        [Column("PasswordHash")]
        [StringLength(300)]
        public string? PasswordHash { get; set; }

        [Column("Role")]
        [StringLength(50)]
        public Role Role { get; set; }

        [Column("IsActive")]
        public bool IsActive { get; set; }

        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; }

    }
}
