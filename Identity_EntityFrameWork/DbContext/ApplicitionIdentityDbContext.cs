using Identity_EntityFrameWork.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity_EntityFrameWork.DbContext
{
    public class ApplicitionIdentityDbContext : IdentityDbContext<AppUser>
    {
        public ApplicitionIdentityDbContext(DbContextOptions<ApplicitionIdentityDbContext> options) : base(options)
        {

        }
        

        
    }
}
