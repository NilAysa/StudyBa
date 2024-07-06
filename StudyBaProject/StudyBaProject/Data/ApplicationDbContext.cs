using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudyBa.Models;
using StudyBaProject.Models;

namespace StudyBaProject.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<AdministratorSubject> AdministratorsSubjects { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Faculty> Faculties { get; set;}
        public DbSet<Location> Locations { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<NewsKeyword> NewsKeywords { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<TutorSubject> TutorsSubjects { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserNews> UserNews { get; set; }
        public DbSet<SessionRequest> SessionRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdministratorSubject>().ToTable("AdministratorSubject");
            modelBuilder.Entity<Department>().ToTable("Department");
            modelBuilder.Entity<Faculty>().ToTable("Faculty");
            modelBuilder.Entity<Location>().ToTable("Location");
            modelBuilder.Entity<Message>().ToTable("Message");
            modelBuilder.Entity<News>().ToTable("News");
            modelBuilder.Entity<NewsKeyword>().ToTable("NewsKeyword");
            modelBuilder.Entity<Review>().ToTable("Review");
            modelBuilder.Entity<Session>().ToTable("Session");
            modelBuilder.Entity<Subject>().ToTable("Subject");
            modelBuilder.Entity<TutorSubject>().ToTable("TutorSubject");
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<UserNews>().ToTable("UserNews");
            modelBuilder.Entity<SessionRequest>().ToTable("SessionRequest");

            base.OnModelCreating(modelBuilder);
        }



    }
}
