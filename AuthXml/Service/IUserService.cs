using AuthXml.DTO;

namespace AuthXml.Service
    {
    public interface IUserService
    {
        Task<string> AddUserAsync(UserAddDto userDto);
    }
}