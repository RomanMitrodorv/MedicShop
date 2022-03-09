﻿global using Autofac;
global using Autofac.Extensions.DependencyInjection;
global using Basket.API.Infrastructure.ActionResults;
global using Basket.API.Infrastructure.Exceptions;
global using Basket.API.Infrastructure.Filters;
global using Basket.API.Infrastructure.Middlewares;
global using Basket.API.IntegrationEvents.EventHandling;
global using Basket.API.IntegrationEvents.Events;
global using Basket.API.Model;
global using Grpc.Core;
global using GrpcBasket;
global using HealthChecks.UI.Client;
global using MedicShop.EventBus;
global using MedicShop.EventBus.Abstractions;
global using MedicShop.EventBus.Events;
global using MedicShop.EventBusRabbitMQ;
global using Microsoft.AspNetCore;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Diagnostics.HealthChecks;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.Filters;
global using Microsoft.AspNetCore.Server.Kestrel.Core;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Diagnostics.HealthChecks;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Options;
global using Microsoft.OpenApi.Models;
global using RabbitMQ.Client;
global using Serilog;
global using Serilog.Context;
global using Services.Basket.API;
global using Services.Basket.API.Controllers;
global using Services.Basket.API.Infrastructure.Repositories;
global using Services.Basket.API.Model;
global using Services.Basket.API.Services;
global using StackExchange.Redis;
global using Swashbuckle.AspNetCore.SwaggerGen;
global using System;
global using System.Collections.Generic;
global using System.ComponentModel.DataAnnotations;
global using System.IdentityModel.Tokens.Jwt;
global using System.IO;
global using System.Linq;
global using System.Net;
global using System.Security.Claims;
global using System.Text.Json;
global using System.Threading.Tasks;
