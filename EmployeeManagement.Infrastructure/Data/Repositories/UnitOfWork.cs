using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Persistence.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace EmployeeManagement.Infrastructure.Data.Repositories;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    private readonly AppDbContext _context = context;
    private IDbContextTransaction? _transaction;

    private IRepository<Employee>? _employeeRepository;
    private IRepository<Department>? _departmentRepository;
    private IRepository<Designation>? _designationRepository;
    private IRepository<Salary>? _salaryRepository;
    private IRepository<User>? _userRepository;
    private IRepository<Role>? _roleRepository;
    private IRepository<Permission>? _permissionRepository;
    private IRepository<RolePermission>? _rolePermissionRepository;

    public IRepository<Employee> Employees =>
        _employeeRepository ??= new GenericRepository<Employee>(_context);

    public IRepository<Department> Departments =>
        _departmentRepository ??= new GenericRepository<Department>(_context);

    public IRepository<Designation> Designations =>
        _designationRepository ??= new GenericRepository<Designation>(_context);

    public IRepository<Salary> Salaries =>
        _salaryRepository ??= new GenericRepository<Salary>(_context);

    public IRepository<User> Users =>
        _userRepository ??= new GenericRepository<User>(_context);

    public IRepository<Role> Roles =>
        _roleRepository ??= new GenericRepository<Role>(_context);

    public IRepository<Permission> Permissions =>
        _permissionRepository ??= new GenericRepository<Permission>(_context);

    public IRepository<RolePermission> RolePermissions =>
        _rolePermissionRepository ??= new GenericRepository<RolePermission>(_context);

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
            await _transaction?.CommitAsync()!;
        }
        finally
        {
            _transaction?.Dispose();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        try
        {
            await _transaction?.RollbackAsync()!;
        }
        finally
        {
            _transaction?.Dispose();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context?.Dispose();
    }
}

