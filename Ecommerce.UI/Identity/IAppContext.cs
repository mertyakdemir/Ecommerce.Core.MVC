using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.UI.Identity
{
    public class IAppContext: IdentityDbContext<User>
    {
        public IAppContext(DbContextOptions<IAppContext> options): base(options)
        {

        }
    }
}
