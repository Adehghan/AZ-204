using infrastructure.Dto.UserManagement;

namespace DeployOnAzure.Repository
{
    public interface IUserRepository
    {
        public Task<List<UserDto>> GetAllAsync();
    }
}
