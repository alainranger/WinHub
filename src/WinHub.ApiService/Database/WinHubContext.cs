﻿using Microsoft.EntityFrameworkCore;

using WinHub.ApiService.Entities;

namespace WinHub.ApiService.Database;

public class WinHubContext(DbContextOptions<WinHubContext> options) : DbContext(options)
{
	public DbSet<Contest> Contests { get; set; }
}