using CCSANoteApp.DB.Repositories;
using CCSANoteApp.Domain;
using CCSANoteApp.Domain.DTOs;

namespace CCSANoteApp.Infrastructure
{
    public class UserService : IUserService
    {
        private readonly UserRepository _userRepository;
        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void CreateUser(string username, string email, string password)
        {
            _userRepository.Add(new User
            {
                Email = email,
                Username = username,
                Password = password
            });
        }

        public void CreateUser(User user)
        {
            _userRepository.Add(user);
        }

        public void DeleteUser(Guid id)
        {
            _userRepository.DeleteById(id);
        }

        public FetchUserDto GetUser(Guid id)
        {
            //Refactor to add user notes
            var user = _userRepository.GetById(id);

            var result = new FetchUserDto
            {
                Username = user.Username,
                Email = user.Email
            };
            result.UserNotes = CreateNoteList(user);
            return result;
        }

        private List<FetchNoteDto> CreateNoteList(User user)
        {
            List<FetchNoteDto> result = new();
            foreach (var note in user.Notes)
            {
                result.Add(new FetchNoteDto()
                {
                    Title = note.Title,
                    Content = note.Content,
                    GroupName = note.GroupName,
                    CreatedDate = note.CreatedDate,
                    UpdatedDate = note.UpdatedDate
                });
            }
            return result;
        }

        public List<FetchUserDto> GetUsers()
        {
            //Refactor
            var users = _userRepository.GetAll();
            List<FetchUserDto> _users = new();
            var _user = new FetchUserDto();
            foreach (var user in users)
            {
                _user = new FetchUserDto() { Email = user.Email, Username = user.Username, UserId = user.Id };
                _user.UserNotes = CreateNoteList(user);
                _users.Add(_user);
            }
            return _users;
        }

        public void UpdateUserEmail(Guid id, string email)
        {
            //var user = GetUser(id);
            //if (user != null)
            //{
            //    user.Email = email;
            //    _userRepository.Update(user);
            //}
        }

        public void UpdateUserName(Guid id, string name)
        {
            //var user = GetUser(id);
            //if (user != null)
            //{
            //    user.Username = name;
            //    _userRepository.Update(user);
            //}
        }
    }
}
