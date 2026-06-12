using Quiz.Shared.DTO.Group.Request;

namespace Quiz.BL.Abstractions;
public interface IGroupService
{
    Task CreateGroup(GroupCreateRequestDto reqDto);
}
