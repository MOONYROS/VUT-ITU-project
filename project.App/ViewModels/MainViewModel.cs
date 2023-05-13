using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using project.BL.Facades;
using project.BL.Facades.Interfaces;
using project.BL.Mappers;
using project.BL.Mappers.Interfaces;
using project.BL.Models;
using project.DAL;
using project.DAL.UnitOfWork;
using project.DAL.Factories;

namespace project.App.ViewModels
{

    public partial class MainViewModel : ViewModelBase
    {
        private IDbContextFactory<ProjectDbContext> _dbContext;

        private IUnitOfWorkFactory _unitOfWorkFactory { get; set; }

        private IUserModelMapper _userModelMapper { get; set; }
        private IUserFacade _userFacade { get; set; }

        public MainViewModel()
        {
            _dbContext = new DbContextSqLiteFactory(GetType().FullName!);
            _unitOfWorkFactory = new UnitOfWorkFactory(_dbContext);
            _userModelMapper = new UserModelMapper();
            _userFacade = new UserFacade(_unitOfWorkFactory,_userModelMapper);
        }
        public IEnumerable<UserListModel> Users { get; set; } = null!;

        public static UserDetailModel UserSeed() => new()
        {
            Id = Guid.NewGuid(),
            FullName = "Random name",
            UserName = "UserName"
        };


        [RelayCommand]
        private async Task GoToAddUser()
        {
            //Shell.Current.GoToAsync("main/newUser");
            var User = UserSeed();
            var idk = await _userFacade.SaveAsync(User);

        }
    }
}