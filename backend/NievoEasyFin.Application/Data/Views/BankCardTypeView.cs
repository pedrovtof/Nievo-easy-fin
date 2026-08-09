using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NievoEasyFin.Application.Data.Entities;

namespace NievoEasyFin.Application.Data.Views;

public class BankCardTypeView
{
    public BankCardTypeView(BankCardTypeEntity entity)
    {
        Id = entity.Id;
        Name = entity.Name;
        Description = entity.Description;
        Active = entity.Active;
        CreatedAt = entity.CreatedAt;
        UpdatedAt = entity.UpdatedAt;
    }

    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public bool Active { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
