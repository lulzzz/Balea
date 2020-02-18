﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Balea;
using Balea.EntityFrameworkCore.Store.DbContexts;
using Balea.EntityFrameworkCore.Store.Entities;
using WebApp.Models;

namespace WebApp.Infrastucture.Data.Seeders
{
    public static class BaleaSeeder
    {
        public static async Task Seed(StoreDbContext db)
        {
            if (!db.Roles.Any())
            {
                var john = new SubjectEntity("John", "1");
                var mary = new SubjectEntity("Mary", "2");

                db.Add(john);
                db.Add(mary);

                await db.SaveChangesAsync();

                var application = new ApplicationEntity(BaleaConstants.DefaultApplicationName, "Default application");
                var viewGradesPermission = new PermissionEntity(Policies.ViewGrades);
                var editGradesPermission = new PermissionEntity(Policies.EditGrades);
                application.Permissions.Add(viewGradesPermission);
                application.Permissions.Add(editGradesPermission);
                var teacherRole = new RoleEntity("Teacher", "Teacher role");
                teacherRole.Subjects.Add(new RoleSubjectEntity { SubjectId = john.Id });
                teacherRole.Permissions.Add(new RolePermissionEntity { Permission = viewGradesPermission });
                teacherRole.Permissions.Add(new RolePermissionEntity { Permission = editGradesPermission });
                application.Roles.Add(teacherRole);
                application.Delegations.Add(new DelegationEntity(john.Id, mary.Id, DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(1), true));
                db.Applications.Add(application);
                await db.SaveChangesAsync();
            }
        }
    }
}