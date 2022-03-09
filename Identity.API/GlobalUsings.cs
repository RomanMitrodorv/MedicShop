﻿global using Autofac;
global using Autofac.Extensions.DependencyInjection;
global using HealthChecks.UI.Client;
global using IdentityModel;
global using IdentityServer4;
global using IdentityServer4.EntityFramework.DbContexts;
global using IdentityServer4.EntityFramework.Mappers;
global using IdentityServer4.EntityFramework.Options;
global using IdentityServer4.Models;
global using IdentityServer4.Services;
global using IdentityServer4.Stores;
global using IdentityServer4.Validation;
global using Microsoft.AspNetCore;
global using Microsoft.AspNetCore.Authentication;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Diagnostics.HealthChecks;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.Rendering;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Design;
global using Microsoft.EntityFrameworkCore.Infrastructure;
global using Microsoft.EntityFrameworkCore.Migrations;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Diagnostics.HealthChecks;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Options;
global using Polly;
global using Serilog;
global using Services.Identity.API;
global using Services.Identity.API.Certificates;
global using Services.Identity.API.Configuration;
global using Services.Identity.API.Data;
global using Services.Identity.API.Devspaces;
global using Services.Identity.API.Extensions;
global using Services.Identity.API.Models;
global using Services.Identity.API.Models.AccountViewModels;
global using Services.Identity.API.Services;
global using StackExchange.Redis;
global using System;
global using System.Collections.Generic;
global using System.ComponentModel.DataAnnotations;
global using System.Data.SqlClient;
global using System.IdentityModel.Tokens.Jwt;
global using System.IO;
global using System.IO.Compression;
global using System.Linq;
global using System.Reflection;
global using System.Security.Claims;
global using System.Security.Cryptography.X509Certificates;
global using System.Text.RegularExpressions;
global using System.Threading.Tasks;
