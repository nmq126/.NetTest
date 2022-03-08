using IdentityTest.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdentityTest.Data
{
    public class MyIdentityDbContext: IdentityDbContext<User>
    {
        public MyIdentityDbContext() : base("ConnectionString")
        {

        }
    }
}