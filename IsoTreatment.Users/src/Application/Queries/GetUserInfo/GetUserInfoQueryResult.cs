namespace Application.Queries.GetUserInfo;

public sealed record GetUserInfoQueryResult(
    string FirstName,
    string LastName,
    string Email,
    int Weight,
    int ClimaxDoseInMiligramsPerKilogramOfBodyWeight,
    int DailyDose
);
