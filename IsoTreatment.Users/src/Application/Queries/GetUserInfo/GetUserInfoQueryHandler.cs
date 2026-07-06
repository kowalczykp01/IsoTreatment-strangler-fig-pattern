using Application.Abstractions;
using Application.Exceptions;
using Domain.UnitOfWork;

namespace Application.Queries.GetUserInfo;

public sealed class GetUserInfoQueryHandler(IUnitOfWork unitOfWork)
    : IQueryHandler<GetUserInfoQuery, GetUserInfoQueryResult>
{
    public async Task<GetUserInfoQueryResult> HandleAsync(GetUserInfoQuery query)
    {
        var user =
            await unitOfWork.UserRepository.GetByIdAsync(query.UserId)
            ?? throw new UserNotFoundException();

        var result = new GetUserInfoQueryResult(
            user.FirstName,
            user.LastName,
            user.Email,
            user.Weight,
            user.ClimaxDoseInMiligramsPerKilogramOfBodyWeight,
            user.DailyDose
        );

        return result;
    }
}
