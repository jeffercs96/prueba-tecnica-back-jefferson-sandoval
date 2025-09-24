using RouletteApi.Enums;

namespace RouletteApi.DbContext.Responses;

public record _();

public record SpinResultResponse(int Number, RouletteColor Color);

public record BetResponse(
    string PlayerName,
    int SpinNumber,
    RouletteColor SpinColor,
    bool IsWin,
    decimal Payout,
    decimal NewBalance
    );