using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NievoEasyFin.Application.Data.Entities;

namespace NievoEasyFin.Application.Data.Views;

/// <summary>
/// Bank card type view
/// </summary>
public class BankCardTypeView
{
    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="entity"></param>
    public BankCardTypeView(BankCardTypeEntity entity)
    {
        Id = entity.Id;
        Name = entity.Name;
        Description = entity.Description;
        Active = entity.Active;
        CreatedAt = entity.CreatedAt;
        UpdatedAt = entity.UpdatedAt;
    }

    /// <summary>
    /// Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Description
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Active
    /// </summary>
    public bool Active { get; set; }

    /// <summary>
    /// Created at
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Updated at
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}
