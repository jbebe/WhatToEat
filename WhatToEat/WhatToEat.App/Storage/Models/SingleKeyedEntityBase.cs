using System.ComponentModel.DataAnnotations.Schema;
using WhatToEat.App.Common;

namespace WhatToEat.App.Storage.Models;

public abstract class SingleKeyedEntityBase<T>
{
    public abstract string Id { get; set; }

    [NotMapped]
    public Id<T> IdTyped => new(Id);
}
