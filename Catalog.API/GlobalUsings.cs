global using Azure.Core;
global using Azure.Identity;
global using Autofac.Extensions.DependencyInjection;
global using Autofac;
global using Grpc.Core;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Mvc.Filters;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Server.Kestrel.Core;
global using Microsoft.AspNetCore;
global using Microsoft.Extensions.Logging;
global using Microsoft.EntityFrameworkCore.Design;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Options;
global using Polly.Retry;
global using Polly;
global using Serilog.Context;
global using Serilog;
global using System.Collections.Generic;
global using System.Data.Common;
global using System.Data.SqlClient;
global using System.Globalization;
global using System.IO.Compression;
global using System.IO;
global using System.Linq;
global using System.Net;
global using System.Text.RegularExpressions;
global using System.Threading.Tasks;
global using System;
global using HealthChecks.UI.Client;
global using Microsoft.AspNetCore.Diagnostics.HealthChecks;
global using Azure.Messaging.ServiceBus;
global using Microsoft.Extensions.Diagnostics.HealthChecks;
global using Microsoft.OpenApi.Models;
global using RabbitMQ.Client;
global using System.Reflection;
global using Catalog.API.Model;
global using Catalog.API.Infastructure.EntityConfiguration;
global using Catalog.API.Infastructure;
global using Catalog.API;
global using IntegrationEventLogEF;
global using Catalog.API.Infastructure.Filters;
global using MedicShop.EventBus.Events;
global using IntegrationEventLogEF.Services;
global using IntegrationEventLogEF.Utilities;
global using MedicShop.EventBus.Abstractions;
global using Catalog.API.ViewModel;
global using Catalog.API.Grpc;
global using Catalog.API.IntegrationEvents;
global using MedicShop.EventBus;
global using MedicShop.EventBusRabbitMQ;
global using Catalog.API.Infastructure.ActionResults;
global using Catalog.API.Infastructure.Exceptions;
global using Catalog.API.Extensions;
global using Microsoft.Extensions.Logging;
global using Catalog.API.IntegrationEvents.Events;
global using Catalog.API.IntegrationEvents.EventHandler;
