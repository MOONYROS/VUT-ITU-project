using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore.Internal;
using project.BL.Facades;
using project.BL.Facades.Interfaces;
using project.BL.Models;
using project.DAL.UnitOfWork;
using project.DAL.Factories;

namespace project.App.ViewModels
{

    public partial class MainViewModel : ViewModelBase
    {
        DbContextFactory = new DbContextSqLiteTestingFactory(GetType().FullName!);

        private readonly IUnitOfWorkFactory _unitOfWorkFactory = new UnitOfWorkFactory();

        private readonly IUserFacade _userFacade = new UserFacade();

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