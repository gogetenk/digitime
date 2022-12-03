using Digitime.Server.Domain.Models;

namespace Digitime.Shared.Dto;
public record MemberDto(string UserId, MemberRoleEnum Role)
{
    public static implicit operator MemberDto(Member member) =>
        new(member.UserId, member.Role);

    public enum MemberRoleEnum
    {
        WORKER,
        REVIEWER
    }
}
