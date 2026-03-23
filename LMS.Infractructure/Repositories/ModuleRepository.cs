using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using LMS.Infractructure.Data;

namespace LMS.Infractructure.Repositories;

public class ModuleRepository(ApplicationDbContext context) : RepositoryBase<Module>(context), IModuleRepository
{
}