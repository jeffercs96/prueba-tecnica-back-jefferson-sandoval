namespace RouletteApi.Models;

/// <summary>
/// Represents a bet request made by a user.
/// </summary>
public class BetRequest
{
    public string PlayerName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string BetType { get; set; } = string.Empty; // "color", "parityColor", "numberColor"
    public string Choice { get; set; } = string.Empty;  // Ej: "RED", "BLACK", "RED-EVEN", "13-RED"
    public int SpinNumber { get; set; } // number that came up on the roulette
    public string SpinColor { get; set; } = string.Empty; // color that came up on the roulette
}