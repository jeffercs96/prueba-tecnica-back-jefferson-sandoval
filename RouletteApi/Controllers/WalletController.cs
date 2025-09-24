using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RouletteApi.DbContext;
using RouletteApi.Entities;

namespace RouletteApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WalletController : ControllerBase
{
    private readonly AppDbContext _context;

    public WalletController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Creates a new wallet for a user.
    /// </summary>
    /// <param name="request">Wallet data (name, initial balance).</param>
    /// <returns>The created wallet.</returns>
    [HttpPost("save")]
    [ProducesResponseType(typeof(UserWallet), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Save([FromBody] UserWallet wallet)
    {
        if (string.IsNullOrWhiteSpace(wallet.Name))
            return BadRequest(new { message = "Name is required" });

        wallet.Name = wallet.Name.Trim().ToUpper();

        var existing = await _context.Wallets
            .FirstOrDefaultAsync(x => x.Name == wallet.Name);

        if (existing != null)
            return Conflict(new { message = "Wallet already exists" });

        _context.Wallets.Add(wallet);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetByName), new { name = wallet.Name }, wallet);
    }

    /// <summary>
    /// Updates the balance of an existing wallet.
    /// </summary>
    /// <param name="request">Wallet data (name and amount to add/subtract).</param>
    /// <returns>The updated wallet.</returns>
    [HttpPut("update")]
    [ProducesResponseType(typeof(UserWallet), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update([FromBody] UserWallet wallet)
    {
        if (string.IsNullOrWhiteSpace(wallet.Name))
            return BadRequest(new { message = "Name is required" });

        wallet.Name = wallet.Name.Trim().ToUpper();

        var existing = await _context.Wallets
            .FirstOrDefaultAsync(x => x.Name == wallet.Name);

        if (existing == null)
            return NotFound(new { message = "Wallet not found" });

        if (wallet.Balance < 0 && existing.Balance + wallet.Balance < 0)
            return BadRequest(new { message = "Insufficient funds" });

        existing.Balance += wallet.Balance;

        await _context.SaveChangesAsync();
        return Ok(existing);
    }

    /// <summary>
    /// Gets a wallet by name.
    /// </summary>
    /// <param name="name">The name of the wallet owner.</param>
    /// <returns>The wallet if found.</returns>
    [HttpGet("{name}")]
    [ProducesResponseType(typeof(UserWallet), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetByName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return BadRequest(new { message = "Name is required" });

        var wallet = await _context.Wallets
            .FirstOrDefaultAsync(x => x.Name == name.Trim().ToUpper());

        return wallet is null ? NotFound(new { message = "Wallet not found" }) : Ok(wallet);
    }

    /// <summary>
    /// Get all wallets.
    /// </summary>
    [HttpGet("all")]
    [ProducesResponseType(typeof(List<UserWallet>), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetAll()
    {
        var wallets = await _context.Wallets.ToListAsync();
        return Ok(wallets);
    }
}
