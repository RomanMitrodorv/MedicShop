﻿global using MedicShop.EventBus.Extensions;
global using MedicShop.EventBus.Events;
global using Autofac.Extensions.DependencyInjection;
global using Autofac;
global using Azure.Core;
global using Azure.Identity;
global using Dapper;
global using FluentValidation;
global using Google.Protobuf.Collections;
global using Grpc.Core;
global using HealthChecks.UI.Client;
global using MediatR;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Diagnostics.HealthChecks;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Mvc.Authorization;
global using Microsoft.AspNetCore.Mvc.Filters;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Server.Kestrel.Core;
global using Microsoft.AspNetCore;
global using Azure.Messaging.ServiceBus;
global using Microsoft.EntityFrameworkCore.Design;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Diagnostics.HealthChecks;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Options;
global using Microsoft.OpenApi.Models;
global using Polly.Retry;
global using Polly;
global using RabbitMQ.Client;
global using Serilog.Context;
global using Serilog;
global using Swashbuckle.AspNetCore.SwaggerGen;
global using System.Collections.Generic;
global using System.Data.Common;
global using System.Data.SqlClient;
global using System.IdentityModel.Tokens.Jwt;
global using System.IO;
global using System.Linq;
global using System.Net;
global using System.Reflection;
global using System.Runtime.Serialization;
global using System.Threading.Tasks;
global using System.Threading;
global using System;
global using Ordering.Domain.Exceptions;
global using IntegrationEventLogEF.Services;
global using MedicShop.EventBus.Abstractions;
global using Ordering.Infastructure;
global using Ordering.API;
global using Ordering.API.Application.IntegrationsEvents;
global using Ordering.API.Application.Models;
global using Ordering.API.Commands;
global using Ordering.API.Extensions;
global using Ordering.API.Infastructure.Services;
global using Ordering.Domain.AggregatesModel.OrderAggregate;
global using Ordering.Infastructure.Idempotency;
global using Ordering.API.Application.Commands;
global using Ordering.API.Queries;
global using IntegrationEventLogEF;
global using Ordering.API.Infastructure.ActionResults;
global using Ordering.Infastructure.Repositories;
global using Ordering.Domain.AggregatesModel.BuyerAggregate;
global using Ordering.API.Application.Behaviors;
global using Ordering.API.Application.Validations;
global using Ordering.API.Application.IntegrationsEvents.Events;
global using MedicShop.EventBus;
global using MedicShop.EventBusRabbitMQ;
global using Ordering.API.Controllers;
global using Ordering.API.Infastructure.Filters;
global using Ordering.API.Infastructure.AutofacModules;
global using Ordering.API.Application.IntegrationsEvents.EventHandling;
global using ApiModels = Ordering.API.Application.Models;
global using AppCommand = Ordering.API.Application.Commands;
