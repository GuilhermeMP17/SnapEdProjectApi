using Microsoft.Practices.Unity;
using SnapEd.Infra.DataContexts;
using SnapEd.Infra.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SnapEd.Api.Dependency
{
    public class DependencyResolver
    {
        public static void Resolve(UnityContainer container)
        {
            container.RegisterType<SnapEdDataContext, SnapEdDataContext>(new HierarchicalLifetimeManager());
            //container.RegisterType<IUserService, UserService>(new HierarchicalLifetimeManager());
            container.RegisterType<User, User>(new HierarchicalLifetimeManager());
        }
    }
}