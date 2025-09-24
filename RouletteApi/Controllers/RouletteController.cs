using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RouletteApi.DbContext;
using RouletteApi.DbContext.Responses;
using RouletteApi.Enums;
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
    [ProducesResponseType(typeof(SpinResultResponse), 200)]
    [ProducesResponseType(404)]
    public IActionResult Spin()
    {
        var number = _random.Next(0, 37); // 0–36
        var color = _random.Next(0, 2) == 0 ? RouletteColor.Red : RouletteColor.Black;

        return Ok(new SpinResultResponse(number, color));
    }

    /// <summary>
    /// Calculate the prize of a bet and update the player's wallet.
    /// </summary>
    [HttpPost("prize")]
    [ProducesResponseType(typeof(BetResponse), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Prize([FromBody] BetRequest bet)
    {
        if (string.IsNullOrWhiteSpace(bet.PlayerName) || bet.Amount <= 0)
            return BadRequest(new { message = "Invalid bet request" });

        var player = await _context.Wallets
            .FirstOrDefaultAsync(x => x.Name == bet.PlayerName.Trim().ToUpper());

        if (player is null)
            return NotFound(new { message = "Player not found" });

        // Verify insufficient funds
        if (player.Balance < bet.Amount)
            return BadRequest(new { message = "Insufficient funds" });

        // Deduct the bet amount
        player.Balance -= bet.Amount;

        bool won = false;
        decimal prize = 0;

        switch (bet.BetType)
        {
            case BetType.Color:
                won = bet.ColorChoice == bet.SpinColor;
                prize = won ? bet.Amount * 1.5m : 0;
                break;

            case BetType.ColorParity:
                var isEven = bet.SpinNumber % 2 == 0;
                won = bet.ColorChoice == bet.SpinColor &&
                      bet.ParityChoice == (isEven ? Parity.Even : Parity.Odd);
                prize = won ? bet.Amount * 2m : 0;
                break;

            case BetType.NumberColor:
                won = bet.NumberChoice == bet.SpinNumber &&
                      bet.ColorChoice == bet.SpinColor;
                prize = won ? bet.Amount * 3m : 0;
                break;

            default:
                return BadRequest(new { message = "Invalid bet type" });
        }

        // Update balance if won
        if (prize > 0)
            player.Balance += prize;

        await _context.SaveChangesAsync();

        return Ok(new BetResponse(
            bet.PlayerName,
            bet.SpinNumber,
            bet.SpinColor,
            won,
            prize,
            player.Balance
            )
        );
    }
}
