using Application.Abstractions;

namespace Application.Queries.GetUserInfo;

public sealed record GetUserInfoQuery(int UserId) : IQuery<GetUserInfoQueryResult>;
