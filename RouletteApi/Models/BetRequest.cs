using RouletteApi.Enums;

namespace RouletteApi.Models;

/// <summary>
/// Represents a bet request made by a user.
/// </summary>
public class BetRequest
{
    public string PlayerName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public BetType BetType { get; set; }
    public RouletteColor? ColorChoice { get; set; } // Para Color o Color+Parity
    public Parity? ParityChoice { get; set; } // for Color+Parity
    public int? NumberChoice { get; set; } // for Number+Color
    public int SpinNumber { get; set; } // number that came up on the roulette
    public RouletteColor SpinColor { get; set; } // color that came up on the roulette
}