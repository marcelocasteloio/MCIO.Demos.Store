﻿using MCIO.Demos.Store.Identity.WebApi.Config.Services.HttpServices;
using System.ComponentModel.DataAnnotations;

namespace MCIO.Demos.Store.Identity.WebApi.Config.Services;

public class ServicesConfig
{
    [Required]
    public HttpServiceCollectionConfig HttpServiceCollection { get; set; } = null!;
}
