namespace Domain;

public class User
{
    public int Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public bool EmailConfirmed { get; private set; }
    public int Weight { get; private set; }
    public int ClimaxDoseInMiligramsPerKilogramOfBodyWeight { get; private set; }
    public int DailyDose { get; private set; }
    public DateTime MedicationStartDate { get; private set; }

    public string? ResetPasswordToken { get; private set; }

    private User(
        string firstName,
        string lastName,
        string email,
        string passwordHash,
        bool emailConfirmed,
        int weight,
        int climaxDoseInMiligramsPerKilogramOfBodyWeight,
        int dailyDose,
        DateTime medicationStartDate,
        string? resetPasswordToken
    )
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PasswordHash = passwordHash;
        EmailConfirmed = emailConfirmed;
        Weight = weight;
        ClimaxDoseInMiligramsPerKilogramOfBodyWeight = climaxDoseInMiligramsPerKilogramOfBodyWeight;
        DailyDose = dailyDose;
        MedicationStartDate = medicationStartDate;
        ResetPasswordToken = resetPasswordToken;
    }

    public static User Create(
        string firstName,
        string lastName,
        string email,
        string passwordHash,
        bool emailConfirmed,
        int weight,
        int climaxDoseInMiligramsPerKilogramOfBodyWeight,
        int dailyDose,
        DateTime medicationStartDate,
        string? resetPasswordToken = null
    )
    {
        return new User(
            firstName: firstName,
            lastName: lastName,
            email: email,
            passwordHash: passwordHash,
            emailConfirmed: emailConfirmed,
            weight: weight,
            climaxDoseInMiligramsPerKilogramOfBodyWeight: climaxDoseInMiligramsPerKilogramOfBodyWeight,
            dailyDose: dailyDose,
            medicationStartDate: medicationStartDate,
            resetPasswordToken: resetPasswordToken
        );
    }
}
