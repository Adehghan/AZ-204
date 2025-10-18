using infrastructure.Dto.UserManagement;

namespace DeployOnAzure.Repository
{
    public class UserRepository : IUserRepository
    {
        public async Task<List<UserDto>> GetAllAsync()
        {
            List<UserDto> result = new List<UserDto>()
            {
                new UserDto() { Id = 1, FirstName="Azadeh", LastName="Dehghan", Active = true, CreateDate = DateTime.Today, Gender = 1, NationalCode = "012364789" },
                new UserDto() { Id = 2,FirstName="Soodabeh", LastName="Rezaie", Active=true, CreateDate = DateTime.Today, Gender= 1, NationalCode= "9516238471"}

            };
            return result;
        }
    }
}
