namespace project.dal
{
    public class Project
    {
        public string Name { get; set; }
        public string Description { get; set; }//mozna string?
        public ICollection<UserProjectList> Users { get; set; }
        public ICollection<Activity> Activities { get; set; }
    }
}