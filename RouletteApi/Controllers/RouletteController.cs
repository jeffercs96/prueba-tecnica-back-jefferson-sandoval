using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RouletteApi.DbContext;
using RouletteApi.Entities;
using RouletteApi.Models;

namespace RouletteApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RouletteController : ControllerBase
{
    private readonly AppDbContext _context;
    private static readonly Random _random = new();

    public RouletteController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Spin the roulette and return a random number (0-36) with a random color.
    /// </summary>
    [HttpGet("spin")]
    public IActionResult Spin()
    {
        var number = _random.Next(0, 37); // 0–36
        var color = _random.Next(0, 2) == 0 ? "RED" : "BLACK";

        return Ok(new { Number = number, Color = color });
    }

    /// <summary>
    /// Calculate the prize of a bet and update the player's wallet.
    /// </summary>
    [HttpPost("prize")]
    public async Task<IActionResult> Prize([FromBody] BetRequest bet)
    {
        if (string.IsNullOrWhiteSpace(bet.PlayerName) || bet.Amount <= 0)
            return BadRequest(new { message = "Invalid bet request" });

        var player = await _context.Wallets
            .FirstOrDefaultAsync(x => x.Name == bet.PlayerName.Trim().ToUpper());

        if (player is null)
            return NotFound(new { message = "Player not found" });

        // Verificar fondos suficientes
        if (player.Balance < bet.Amount)
            return BadRequest(new { message = "Insufficient funds" });

        // Restar la apuesta al inicio
        player.Balance -= bet.Amount;

        bool won = false;
        decimal prize = 0;

        switch (bet.BetType.ToLower())
        {
            case "color":
                won = bet.Choice.Equals(bet.SpinColor, StringComparison.OrdinalIgnoreCase);
                prize = won ? bet.Amount * 1.5m : 0;
                break;

            case "paritycolor":
                var isEven = bet.SpinNumber % 2 == 0;
                var expected = bet.Choice.ToUpper(); // Ej: "RED-EVEN"
                var actual = $"{bet.SpinColor.ToUpper()}-{(isEven ? "EVEN" : "ODD")}";
                won = expected == actual;
                prize = won ? bet.Amount * 2m : 0;
                break;

            case "numbercolor":
                var expectedCombo = bet.Choice.ToUpper(); // Ej: "13-RED"
                var actualCombo = $"{bet.SpinNumber}-{bet.SpinColor.ToUpper()}";
                won = expectedCombo == actualCombo;
                prize = won ? bet.Amount * 3m : 0;
                break;

            default:
                return BadRequest(new { message = "Invalid bet type" });
        }

        // Si ganó, sumar el premio
        if (prize > 0)
            player.Balance += prize;

        await _context.SaveChangesAsync();

        return Ok(new
        {
            bet.PlayerName,
            bet.SpinNumber,
            bet.SpinColor,
            Won = won,
            Prize = prize,
            CurrentBalance = player.Balance
        });
    }
}
