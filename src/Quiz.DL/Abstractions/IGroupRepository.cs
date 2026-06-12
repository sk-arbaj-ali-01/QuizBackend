using Quiz.DL.Entities;

namespace Quiz.DL.Abstractions;
public interface IGroupRepository
{
    Task CreateGroup(GroupEntity entity);
}
