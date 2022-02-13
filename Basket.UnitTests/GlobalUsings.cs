﻿global using Basket.API.IntegrationEvents.Events;
global using Basket.API.Model;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Mvc;
global using Services.Basket.API.Controllers;
global using Services.Basket.API.Model;
global using Microsoft.Extensions.Logging;
global using Moq;
global using System;
global using System.Collections.Generic;
global using System.Security.Claims;
global using System.Threading.Tasks;
global using Xunit;
global using IBasketIdentityService = Services.Basket.API.Services.IIdentityService;
global using MedicShop.EventBus.Abstractions;