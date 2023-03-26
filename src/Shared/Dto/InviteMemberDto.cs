using Digitime.Shared.Contracts;

namespace Digitime.Shared.Dto;

public record InviteMemberDto(string ProjectId, string InviteeEmail, MemberRoleEnum Role);