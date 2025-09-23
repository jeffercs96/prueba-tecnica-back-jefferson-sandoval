namespace RouletteApi.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// Represents a user's wallet in the roulette system.
/// Stores the player's name and current balance.
/// </summary>
[Table("WALLETS", Schema = "GAME")]
public class UserWallet
{
    /// <summary>
    /// Primary key of the wallet record.
    /// Auto-increment identity column.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Name of the wallet owner.
    /// Case-insensitive and unique across the system.
    /// </summary>
    [Required]
    [Column(TypeName = "citext")] // PostgreSQL extension for case-insensitive text
    [MaxLength(100)]

    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Current balance of the wallet.
    /// Represents the amount of money available to play.
    /// </summary>
    [Column(TypeName = "numeric(18,2)")]
    public decimal Balance { get; set; } = 0;
}