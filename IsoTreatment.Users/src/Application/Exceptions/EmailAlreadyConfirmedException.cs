namespace Application.Exceptions;

public sealed class EmailAlreadyConfirmedException() : Exception("Email has already been confirmed");
