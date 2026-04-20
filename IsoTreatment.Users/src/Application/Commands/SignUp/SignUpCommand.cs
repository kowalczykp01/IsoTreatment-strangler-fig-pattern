using Application.Abstractions;

namespace Application.Commands.SignUp;

public sealed record SignUpCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string ConfirmPassword,
    int Weight,
    int ClimaxDoseInMiligramsPerKilogramOfBodyWeight,
    int DailyDose,
    DateTime MedicationStartDate
) : ICommand;
