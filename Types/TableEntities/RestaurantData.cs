using WhatToEat.Types.Enums;

namespace WhatToEat.Types.TableEntities
{
  public class RestaurantData: TableEntityBase
  {
    public const string PrimaryKey = "restaurant";

    public string Name { get; set; }

    public PaymentMethod PaymentMethod { get; set; }

    public RestaurantData(){}

    public RestaurantData(string restaurantId, string name, PaymentMethod paymentMethod): base(PrimaryKey, restaurantId)
    {
      Name = name;
      PaymentMethod = paymentMethod;
    }

    public string GetId() => RowKey;
  }
}
