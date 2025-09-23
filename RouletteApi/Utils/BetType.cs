namespace RouletteApi.Enums;

/// <summary>
/// Types of bets available in the roulette game.
/// </summary>
public enum BetType
{
    Color = 1,        // Bet on a color (Red/Black)
    ColorParity = 2,  // Bet on even/odd of a color (Red even, Black odd, etc.)
    NumberColor = 3   // Bet on number + color (e.g., 13-Red, 7-Black, etc.)
}