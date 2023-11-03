using WpfApp1.BL.Models;
using WpfApp1.DAL.Entities;

namespace WpfApp1.BL.Facades.Interfaces;

public interface IProjectFacade : IFacade<ProjectEntity, ProjectListModel, ProjectDetailModel>
{
}