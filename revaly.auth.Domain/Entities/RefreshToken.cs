using revaly.auth.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace revaly.auth.Domain.Entities
{
    public class RefreshToken : BaseEntity
    {
        [Column("Token")]
        public string? Token { get; set; }

        [Column("ExpiresAt")]
        public DateTime ExpiresAt { get; set; }

        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; }

        [ForeignKey("UserId")]
        public Guid UserId { get; set; }

        public virtual User? User { get; set; }
    }
}
